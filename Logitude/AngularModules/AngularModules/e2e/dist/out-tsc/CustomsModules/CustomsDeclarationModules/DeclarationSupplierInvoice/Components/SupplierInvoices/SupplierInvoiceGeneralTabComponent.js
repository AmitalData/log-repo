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
var CustomsVendorPMService_1 = require("../../../../../Customs/Services/StandardPMs/CustomsVendorPMService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var SupplierInvoiceItemPM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoiceItemPM");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../../../Infrastructure/Services/EntityListService");
var SupplierInvoiceFreightAmountPM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoiceFreightAmountPM");
var CustomsExchangeRateExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService");
var SupplierInvoiceService_1 = require("../../../../../Customs/Services/Others/SupplierInvoiceService");
var TermsOfSaleTypeListService_1 = require("../../../../../Customs/Services/StandardLists/TermsOfSaleTypeListService");
var LuhnAlgorithm_1 = require("../../../../../Customs/Utilities/LuhnAlgorithm");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var QuantityTypeMessageService_1 = require("../../../../../Customs/Services/WebServices/QuantityTypeMessageService");
var CustomsRequiredFieldListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var CustomsVendorListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsVendorListService");
var SupplierInvoiceExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService");
var SupplierInvoicePMService_1 = require("../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService");
var DateTimeToDatePipe_1 = require("../../../../../Controls/Pipes/DateTimeToDatePipe");
var SupplierInvoiceItemProcesTypePM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoiceItemProcesTypePM");
var ItemGovernmentProcedureTypeListService_1 = require("../../../../../Customs/Services/StandardLists/ItemGovernmentProcedureTypeListService");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var CustomsSettingExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomsCountryListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsCountryListService");
var GITITEMCacheService_1 = require("../../../../../Customs/Services/Others/GITITEMCacheService");
var SupplierInvoiceGeneralTabComponent = /** @class */ (function (_super) {
    __extends(SupplierInvoiceGeneralTabComponent, _super);
    function SupplierInvoiceGeneralTabComponent(cd) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.SupplierInvoice";
        _this.customsVendorPMService = new CustomsVendorPMService_1.CustomsVendorPMService();
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ChangeScrollPosition = new core_1.EventEmitter();
        _this.vendorNumber = "";
        _this.supplierInvoiceService = new SupplierInvoiceService_1.SupplierInvoiceService();
        _this.termsOfSaleTypeListService = new TermsOfSaleTypeListService_1.TermsOfSaleTypeListService();
        _this.customsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService_1.CustomsExchangeRateExtendedPMService();
        _this.supplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
        _this.IsDisplayOnly = false;
        _this.IsReadOnly = false;
        //public IsCountryPURForItems: boolean = false;
        _this.quantityTypeMessageService = new QuantityTypeMessageService_1.QuantityTypeMessageService();
        _this.ClasificationQtyTypes = {};
        _this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        _this.itemGovernmentProcedureTypeListService = new ItemGovernmentProcedureTypeListService_1.ItemGovernmentProcedureTypeListService();
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this.IsActionButtonsEnabled = true;
        _this.ReloadEntityEvent = new core_1.EventEmitter();
        _this.isMasterInvoic = false;
        _this.IsValueForCustomsOnlyVisible = false;
        _this.addedVehicles = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsNotForAccumaltionVisibile = true;
        _this.IsAccumulationStateVisibile = false;
        _this.originalItemSource = new ObservableCollection_1.ObservableCollection([]);
        _this.AccumulatedFilterSelectedValue = 'Accumulated';
        _this.opacity = 1;
        _this.FiltersList = ["Copy Now"];
        //private isFreightCurrencyTypeCodeEnabled: boolean;
        //public get IsFreightCurrencyTypeCodeEnabled() { return this.isFreightCurrencyTypeCodeEnabled; }
        //public set IsFreightCurrencyTypeCodeEnabled(newValue: boolean) { this.isFreightCurrencyTypeCodeEnabled = newValue; }
        _this.freightAmountGridEnabled = true;
        _this.addAmountEnabled = true;
        _this.incotermChanged = false;
        _this.allowToDelete = true;
        _this.totalFreightAmountInInvoiceCurrency = 0;
        _this.filters = null;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.AmountList = new ObservableCollection_1.ObservableCollection([]);
        _this._entityListService = new EntityListService_1.EntityListService();
        _this.FreightCopyList = [];
        _this.VendorFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.VendorFilterItems.addAdditionalFilter("StatusCode", "1", "NULL", null, "Equals", false, false, false, "string", false, true);
        //  this.VendorFilterItems.addAdditionalFilter("StatusCode", "NULL", null, null, "Equals", false, false, false, "string", false, true);
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        _this.accumulationFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCUMULATION") && f.ObjectTableId == table.Id; })[0];
        if (_this.accumulationFeature) {
            _this.IsAccumulationStateVisibile = true;
            _this.IsNotForAccumaltionVisibile = true;
        }
        _this.CheckRequrierdFieldsForSend();
        //this.CurrentSession.SubscriptionAdd(
        //this.CurrentSession.SelectInvoiceItemEvent.subscribe((res) => {
        //    var item: SupplierInvoiceItemLine = this.ItemsSource.Collection.filter(d => d.SequenceNumeric == res.filter)[0];
        //    this.SelectedRow = item;
        //    var index = this.ItemsSource.Collection.indexOf(item);
        //    this.ChangeScrollPosition.emit({ RowIndex: index});
        //    })
        //);
        _this.ikeaFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "IKEA") && f.ObjectTableId == table.Id; })[0];
        //FRITZ
        _this.IFritz_feature = FeatureLocator_1.FeatureLocator.Features.filter(function (d) { return d.Code == "IFRITZ"; })[0];
        console.log("IFritz feature: ", _this.IFritz_feature);
        return _this;
    }
    SupplierInvoiceGeneralTabComponent.prototype.SelectInvoiceItemMethod = function (res) {
        var item = this.ItemsSource.Collection.filter(function (d) { return d.SequenceNumeric == res.filter; })[0];
        //this.SelectedRow = item;
        this.OnSelectedItemChanged(item); // set this.SelectedRow
        var index = this.ItemsSource.Collection.indexOf(item);
        this.ChangeScrollPosition.emit({ RowIndex: index });
    };
    SupplierInvoiceGeneralTabComponent.prototype.ngOnDestroy = function () {
        console.log("SupplierInvoiceGeneralTabComponent:ngOnDestroy");
        this.cd = null;
        this.Parent = null;
        if (this.ItemsSource) {
            this.ItemsSource.Collection.forEach(function (item) {
                var siil = item;
                siil.Dispose();
                siil.Parent = null;
                siil.DataContext = null;
            });
            this.ItemsSource.Clear();
            this.ItemsSource = null;
        }
        if (this.AmountList) {
            this.AmountList.Collection.forEach(function (item) {
                var amm = item;
                amm.Parent = null;
                amm.DataContext = null;
            });
            this.AmountList.Clear();
            this.AmountList = null;
        }
        this.FreightCopyList = null;
        this.Dispose();
    };
    SupplierInvoiceGeneralTabComponent.prototype.BuildItemsList = function () {
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        this.ItemsSource.Clear();
        this.ParentItems = [];
        this.ChildrenItems = [];
        var TempItemSource = [];
        if (this.IsAccumulated) {
            this.AccumulatedMessageVisibility = true;
            this.ParentItems = this.EntityPM.SupplierInvoiceItems.filter(function (d) { return d.IsParent; });
            this.ChildrenItems = this.EntityPM.SupplierInvoiceItems.filter(function (d) { return !d.IsParent; });
            if (this.IsFromCustomsAnswer && !this.IsInvoiceAnswer) {
                this.ParentsCount = "(" + this.ParentItems.length.toString() + ")";
                this.ChildrenCount = "(" + this.ChildrenItems.length.toString() + ")";
            }
            if (this.AccumulatedFilterSelectedValue == 'Accumulated') {
                this.IsReadOnly = true;
                this.IsNotForAccumaltionVisibile = false;
                for (var i = 0; i < this.ParentItems.length; i++) {
                    TempItemSource.push(new SupplierInvoiceItemLine(this.ParentItems[i], this));
                }
            }
            else if (this.AccumulatedFilterSelectedValue == 'NotAccumulated') {
                this.ChildrenItems = this.EntityPM.SupplierInvoiceItems.filter(function (d) { return !d.IsParent; });
                if (!this.IsDisplayOnly) {
                    this.IsReadOnly = false;
                }
                if (this.accumulationFeature) {
                    this.IsNotForAccumaltionVisibile = true;
                }
                for (var i = 0; i < this.ChildrenItems.length; i++) {
                    TempItemSource.push(new SupplierInvoiceItemLine(this.ChildrenItems[i], this));
                }
            }
        }
        else {
            this.AccumulatedMessageVisibility = false;
            for (var i = 0; i < this.EntityPM.SupplierInvoiceItems.length; i++) {
                TempItemSource.push(new SupplierInvoiceItemLine(this.EntityPM.SupplierInvoiceItems[i], this));
            }
        }
        if (this.EntityPM.IsAccumalated) {
            //    this.CurrentSession.AccumulatedFilterChangedEvent.emit({ filter: this.AccumulatedFilterSelectedValue, ParentCount: this.ParentItems.length, childrenCount: this.ChildrenItems.length });
        }
        this.ItemsSource.InsertCollection(TempItemSource);
        this.CurrentSession.StopBusyIndicator();
        this.originalItemSource.InsertCollection(TempItemSource);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromClassificationJumpToSII)) {
            this.SelectInvoiceItemMethod({ filter: this.FromClassificationJumpToSII });
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.Search = function (text) {
        var itemsSource = this.originalItemSource;
        var itemSourceByItemPrice = this.originalItemSource;
        if (Tools_1.AppTool.IsNullOrEmpty(text)) {
            this.BuildItemsList();
        }
        else {
            itemsSource = itemsSource.Collection.filter(function (f) { return f.ClassificationCode != null || f.ItemCode != null; });
            itemSourceByItemPrice = itemSourceByItemPrice.Collection.filter(function (f) { return f.ItemPrice != null; });
            var TempItemSource = [];
            TempItemSource = itemsSource.filter(function (f) { return (!Tools_1.AppTool.IsNullOrEmpty(f.ClassificationCode) ? f.ClassificationCode.toUpperCase().includes(text.toUpperCase()) : null) || (!Tools_1.AppTool.IsNullOrEmpty(f.ItemCode) ? f.ItemCode.toUpperCase().includes(text.toUpperCase()) : null); });
            this.ItemsSource.InsertCollection(TempItemSource);
            if (itemSourceByItemPrice) {
                itemSourceByItemPrice = this.originalItemSource.Collection.filter(function (f) { return f.ItemPrice == (text); });
                if (itemSourceByItemPrice.length > 0) {
                    if (this.ItemsSource.Length > 0) {
                        var _loop_1 = function (item) {
                            exist = TempItemSource.filter(function (d) { return d.LineNumber == item.LineNumber; })[0];
                            if (!exist) {
                                TempItemSource.push(item);
                            }
                        };
                        var exist;
                        for (var _i = 0, itemSourceByItemPrice_1 = itemSourceByItemPrice; _i < itemSourceByItemPrice_1.length; _i++) {
                            var item = itemSourceByItemPrice_1[_i];
                            _loop_1(item);
                        }
                        this.ItemsSource.InsertCollection(TempItemSource);
                    }
                    else {
                        this.ItemsSource.InsertCollection(itemSourceByItemPrice);
                    }
                }
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(text)) {
            this.SearchFilterChangedEvent = this.CurrentSession.SearchFilterChangedEvent.emit({ count: this.ItemsSource.Length });
        }
        else {
            this.SearchFilterChangedEvent = this.CurrentSession.SearchFilterChangedEvent.emit({ count: null });
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.AccumulatedFilterItemClicked = function (itemValue) {
        var _this = this;
        if (this.AccumulatedFilterChangedEvent) {
            this.AccumulatedFilterChangedEvent.unsubscribe();
            this.AccumulatedFilterChangedEvent = null;
        }
        if (this.AccumulatedFilterSelectedValue != itemValue) {
            this.AccumulatedFilterSelectedValue = itemValue;
            if (this.AccumulatedFilterSelectedValue == 'Accumulated') {
                this.AccumulatedFilter = "parent";
                this.AccumulatedMessageText = "חשבון צבור - פרטי מכס ניתנים לעריכה רק במצב לא צבור";
                this.IsActionButtonsEnabled = false;
            }
            else if (this.AccumulatedFilterSelectedValue == 'NotAccumulated') {
                this.AccumulatedFilter = "child";
                this.AccumulatedMessageText = "חשבון צבור";
                if (!this.IsDisplayOnly) {
                    this.IsActionButtonsEnabled = true;
                }
            }
            if (!this.IsFromCustomsAnswer || this.IsInvoiceAnswer) {
                this.CurrentSession.StartBusyIndicator("");
                this.Parent.EntityPM = this.EntityPM;
                if (this.Parent.EntityPM.IsDirty) {
                    this.Parent.SaveChangesSync();
                }
                this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey, 0, 500, this.AccumulatedFilter).subscribe(function (response) {
                    var entityPM = _this.EntityPM;
                    _this.EntityPM = response.Result;
                    _this.BuildItemsList();
                    _this.Parent.EntityPM.FullChildrenCount = _this.EntityPM.FullChildrenCount;
                    _this.Parent.EntityPM.FullParentsCount = _this.EntityPM.FullParentsCount;
                    _this.AccumulatedFilterChangedEvent = _this.CurrentSession.AccumulatedFilterChangedEvent.emit({ filter: _this.AccumulatedFilterSelectedValue, ParentCount: _this.ParentItems.length, childrenCount: _this.ChildrenItems.length, entityPM: entityPM });
                    _this.Parent.EntityPM = _this.EntityPM;
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                this.BuildItemsList();
            }
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.calculateTotals = function (deleteItem, deletedItemPrice) {
        if (deleteItem) {
            if (deletedItemPrice == null)
                deletedItemPrice = 0;
            if (this.Parent.TotalForeignCurrency == null)
                this.Parent.TotalForeignCurrency = 0;
            this.Parent.TotalForeignCurrency = this.Parent.TotalForeignCurrency - deletedItemPrice;
            this.Parent.Difference = this.Parent.TotalForeignCurrency - (this.InvoiceAmount);
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.InitTab = function (entityPM, parent, isDisplayOnly, getFreightTotals, IsNewEntity, IsFromCustomsAnswer, IsInvoiceAnswer) {
        var _this = this;
        if (getFreightTotals === void 0) { getFreightTotals = true; }
        this.InvoiceTypeFocus = false;
        this.focusTimerToken = setTimeout(function () {
            _this.InvoiceTypeFocus = true;
        }, 1);
        this.EntityPM = entityPM;
        this.Parent = parent;
        this.IsDisplayOnly = isDisplayOnly;
        this.IsReadOnly = isDisplayOnly;
        this.IsActionButtonsEnabled = !isDisplayOnly;
        this.Pointers = this.Parent.pointers;
        this.declarationPM = parent.declarationPM;
        this.IsFromCustomsAnswer = IsFromCustomsAnswer;
        this.IsInvoiceAnswer = IsInvoiceAnswer;
        this.oldIncoterm = this.EntityPM.IncotermCode;
        //this.InvoiceNumber = entityPM.InvoiceNumber;
        this.IsChecked = false;
        this.isNewEntity = IsNewEntity;
        if (isDisplayOnly) {
            this.opacity = 0.5;
        }
        if (!this.IsFromCustomsAnswer || this.IsInvoiceAnswer) {
            this.ParentsCount = "(" + this.EntityPM.FullParentsCount + ")";
            this.ChildrenCount = "(" + this.EntityPM.FullChildrenCount + ")";
        }
        this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForDate(this.declarationPM.TaxationDateTime).subscribe(function (responseRate) {
            if (responseRate) {
                if (!responseRate.HasError) {
                    _this.ExchangeRates = responseRate.Result;
                }
            }
            _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(function (response) {
                _this.entityResourceService.getEntityResourceByTableName("Customs.CustomsPartnersItem").subscribe(function (response) {
                    if (_this.EntityPM.IsAccumalated) {
                        _this.IsAccumulated = true;
                        _this.AccumulatedMessageText = "חשבון צבור - פרטי מכס ניתנים לעריכה רק במצב לא צבור";
                        if (_this.AccumulatedFilterSelectedValue == 'Accumulated') {
                            _this.IsActionButtonsEnabled = false;
                        }
                        else if (!_this.IsDisplayOnly) {
                            _this.IsActionButtonsEnabled = true;
                            ;
                        }
                    }
                    else {
                        _this.IsAccumulated = false;
                        _this.AccumulatedMessageText = null;
                    }
                    _this.BuildFreightAmountsList();
                    _this.BuildItemsList();
                    //  this.CurrentSession.AccumulatedFilterChangedEvent.emit({ filter: this.AccumulatedFilterSelectedValue, ParentCount: this.ParentItems.length, childrenCount: this.ChildrenItems.length });
                    if (getFreightTotals) {
                        _this.GetFreightTotals();
                    }
                    //if (entityPM.InsruancePercentage) {
                    //    this.CalculateInsuranceAmount(entityPM.InsruancePercentage);
                    //}
                    if (_this.InsurancePercentage == null) {
                        _this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                        _this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                        if (_this.InsruanceCurrencyTypeCode != null || _this.InsuranceAmount != null) {
                            _this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        }
                        _this.GetInsurancePercentDefault();
                    }
                    else {
                        _this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        _this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                    }
                    if (_this.EntityPM.InsruancePercentage) {
                        _this.CalculateInsuranceAmount(_this.EntityPM.InsruancePercentage);
                    }
                    if (entityPM.SequenceNumeric != null && entityPM.SequenceNumeric != 1) {
                        _this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        _this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        _this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                    }
                    else {
                    }
                    _this.IncotermLogic(_this.EntityPM.IncotermCode);
                    _this.SetScreenFieldsEditability();
                    //this.cd.detectChanges();
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.VendorId)) {
                        _this.customsVendorPMService.get(_this.EntityPM.VendorId).subscribe(function (myResponse) {
                            _this.vendor = myResponse.Result;
                            if (_this.vendor != null) {
                                _this.vendorNumber = _this.vendor.VendorNumber;
                            }
                        });
                    }
                    _this.SetDepositionStatus();
                    //this.GetExchagneRates();
                });
            });
        });
        //FRITZ
        if (this.IFritz_feature) {
            if (this.declarationPM.PrimaryInvoiceCounterKey && this.EntityPM.InvoiceCounterKey) {
                if (this.EntityPM.InvoiceCounterKey.toString() == this.declarationPM.PrimaryInvoiceCounterKey) {
                    //yes, master invoice
                    this.isMasterInvoic = true;
                    this.IsValueForCustomsOnlyVisible = true;
                }
                else {
                    this.isMasterInvoic = false;
                    this.IsValueForCustomsOnlyVisible = false;
                }
            }
            else {
                // if it is the first created invoice, then its master 
                if (this.Parent.SaveAndNew) { // comes after click inittab
                    this.isMasterInvoic = false;
                    this.IsValueForCustomsOnlyVisible = false;
                }
                else if (this.declarationPM.SupplierInvoices.length == 0) {
                    this.isMasterInvoic = true;
                    this.IsValueForCustomsOnlyVisible = true;
                }
                else {
                    this.isMasterInvoic = false;
                    this.IsValueForCustomsOnlyVisible = false;
                }
            }
        }
        else {
            this.IsValueForCustomsOnlyVisible = false;
        }
        //after save n new
        if (this.Parent.SaveAndNew) {
            //open frieght
            this.AddAmountEnabled = true;
            this.FreightAmountGridEnabled = true;
            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);
            //resset is preference chck
            this.IsPreference = false;
        }
        //if (this.Parent.IsNewEntity) {
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        this.customsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString())
            .subscribe(function (customsSettingList) {
            if (customsSettingList) {
                _this.CurrentSession.StopBusyIndicator();
                if (_this.Parent.IsNewEntity) {
                    var autoFillAccountType = customsSettingList.Result ? customsSettingList.Result.AutoFillAccountType : false;
                    if (autoFillAccountType) {
                        _this.AccountTypeCode = "380";
                    }
                }
                var autoUnitMeasurement = customsSettingList.Result ? customsSettingList.Result.AutoUnitMeasurement : false;
                if (autoUnitMeasurement) {
                    _this.IsChecked = true;
                }
            }
        });
        //}
        //this.GetCountryPURForItems();
    };
    //GetExchagneRates() {
    //    this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForDate(this.declarationPM.TaxationDateTime).subscribe(response => {
    //        if (response) {
    //            if (!response.HasError) {
    //                this.ExchangeRates = response.Result;
    //            }
    //        }
    //            });
    //}
    SupplierInvoiceGeneralTabComponent.prototype.SetScreenFieldsEditability = function () {
        this.UIProperties.SetEnabled("AccountTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InvoiceNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ActualPayedAmount", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SequenceNumeric", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IssueDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("VendorId", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IssueCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InvoiceCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PreferenceDocumentTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsPreference", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PaymentTermsCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PaymentTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ActualPayedCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AccumalationStateCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("FreightAmountGridEnabled", this.ObjectTableName, !this.IsDisplayOnly);
        //this.UIProperties.SetEnabled("AddAmountEnabled", this.ObjectTableName, !this.IsDisplayOnly);
        //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IncotermCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InvoiceAmount", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TotalFreightAmountInInvoiceCurrency", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TotalFreightInFreightCurrency", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("InsurancePercentage", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TotalFreightAmountInNIS", this.ObjectTableName, !this.IsDisplayOnly);
        if (this.IsDisplayOnly) {
            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
            this.UIProperties.SetEnabled("InsuranceAmount", this.ObjectTableName, !this.IsDisplayOnly);
            this.UIProperties.SetEnabled("InsruancePercentage", this.ObjectTableName, !this.IsDisplayOnly);
            this.AddAmountEnabled = !this.IsDisplayOnly;
            this.FreightAmountGridEnabled = !this.IsDisplayOnly;
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.SetDepositionStatus = function () {
        var _this = this;
        this.supplierInvoiceExtendedPMService.GetImporterDespositionStatus(this.VendorId, this.declarationPM.ImporterId).subscribe(function (response) {
            if (response) {
                if (response.Result) {
                    _this.importer = response.Result;
                    _this.tootltip = new DateTimeToDatePipe_1.DateTimeToDatePipe().transform(_this.importer.EndDate) + "," + _this.importer.ImporterDespositionNumber;
                    if (_this.importer.Status == "GreenTick") {
                        _this.GreenVisibility = true;
                        _this.RedVisibility = false;
                        _this.OrangeVisibility = false;
                    }
                    else if (_this.importer.Status == "RedX") {
                        _this.RedVisibility = true;
                        _this.GreenVisibility = false;
                        _this.OrangeVisibility = false;
                    }
                    else if (_this.importer.Status == "OrangeTick") {
                        _this.OrangeVisibility = true;
                        _this.GreenVisibility = false;
                        _this.RedVisibility = false;
                    }
                    else {
                        _this.GreenVisibility = false;
                        _this.RedVisibility = false;
                        _this.OrangeVisibility = false;
                    }
                }
            }
        });
    };
    SupplierInvoiceGeneralTabComponent.prototype.CheckRequrierdFieldsForSend = function () {
        var _this = this;
        var customsRequiredFieldListService = new CustomsRequiredFieldListService_1.CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(function (d) { return d.Name == 'Customs.SupplierInvoice'; })[0];
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        customsRequiredFieldListService.getAllFromCache(filters).subscribe(function (response) {
            var requiredFields = response.Result;
            requiredFields.forEach(function (field) {
                var objectField = window.ObjectFields.filter(function (d) { return d.Id == field.ObjectfieldId; })[0];
                _this.UIProperties.SetWarning(objectField.FieldName, 'Customs.SupplierInvoice', true);
            });
        });
    };
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "SelectedFilter", {
        get: function () { return this.selectedFilter; },
        set: function (value) {
            if (this.selectedFilter != value) {
                this.selectedFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "AccountTypeCode", {
        get: function () { return this.EntityPM.AccountTypeCode; },
        set: function (newValue) { this.EntityPM.AccountTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InvoiceNumber", {
        get: function () { return this.EntityPM.InvoiceNumber; },
        set: function (newValue) { this.EntityPM.InvoiceNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "ActualPayedAmount", {
        get: function () { return this.EntityPM.ActualPayedAmount; },
        set: function (newValue) { this.EntityPM.ActualPayedAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "SequenceNumeric", {
        get: function () { return this.EntityPM.SequenceNumeric; },
        set: function (newValue) { this.EntityPM.SequenceNumeric = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "IssueDate", {
        get: function () { return this.EntityPM.IssueDate; },
        set: function (newValue) { this.EntityPM.IssueDate = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "VendorId", {
        get: function () { return this.EntityPM.VendorId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.VendorId != newValue) {
                //this.Parent.calculateCommission = true; //old code
                //this.Parent.CalculateCommissionPercentage();
            }
            this.EntityPM.VendorId = newValue;
            this.customsVendorPMService.get(newValue).subscribe(function (myResponse) {
                _this.vendor = myResponse.Result;
                if (_this.vendor != null) {
                    _this.EntityPM.IssueCountryCode = _this.vendor.CountryCode;
                    _this.EntityPM.IssueCountryName = "טאיוואן";
                    _this.vendorNumber = _this.vendor.VendorNumber;
                    _this.SetDepositionStatus();
                }
                else {
                    _this.GreenVisibility = false;
                    _this.OrangeVisibility = false;
                    _this.RedVisibility = false;
                }
            });
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "AccumalationStateCode", {
        get: function () { return this.EntityPM.AccumalationStateCode; },
        set: function (newValue) {
            this.EntityPM.AccumalationStateCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "IssueCountryCode", {
        get: function () { return this.EntityPM.IssueCountryCode; },
        set: function (newValue) { this.EntityPM.IssueCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InvoiceCurrency", {
        get: function () { return this.invoiceCurrency; },
        set: function (newValue) { this.invoiceCurrency = newValue; },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceGeneralTabComponent.prototype.InvoiceCurrencyChanged = function (currency) {
        this.InvoiceCurrency = currency;
        if (this.InvoiceCurrency)
            this.Parent.invoiceCurrencyName = this.InvoiceCurrency.LocalName;
        //this.Parent.CalculateCommissionPercentage();
    };
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InvoiceCurrencyTypeCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceCurrencyTypeCode != newValue) {
                //this.Parent.calculateCommission = true; //old code
            }
            this.EntityPM.InvoiceCurrencyTypeCode = newValue;
            var percentage;
            //if (this.declarationPM.SupplierInvoices.length > 0) {
            //    percentage = this.declarationPM.SupplierInvoices[0].InsruancePercentage;
            //}
            //else {
            //    percentage = this.InsurancePercentage;
            //}
            var firstInvoice = this.Parent.Get1SupplierInvoice();
            percentage = firstInvoice.InsruancePercentage;
            if (percentage != null) {
                this.CalculateInsuranceAmount(percentage);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "PreferenceDocumentTypeCode", {
        get: function () { return this.EntityPM.PreferenceDocumentTypeCode; },
        set: function (newValue) { this.EntityPM.PreferenceDocumentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "IsPreference", {
        get: function () { return this.EntityPM.IsPreference; },
        set: function (newValue) { this.EntityPM.IsPreference = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "PaymentTermsCode", {
        get: function () { return this.EntityPM.PaymentTermsCode; },
        set: function (newValue) { this.EntityPM.PaymentTermsCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "PaymentTypeCode", {
        get: function () { return this.EntityPM.PaymentTypeCode; },
        set: function (newValue) { this.EntityPM.PaymentTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "ActualPayedCurrencyTypeCode", {
        get: function () { return this.EntityPM.ActualPayedCurrencyTypeCode; },
        set: function (newValue) { this.EntityPM.ActualPayedCurrencyTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InsruanceCurrencyTypeCode", {
        get: function () { return this.EntityPM.InsruanceCurrencyTypeCode; },
        set: function (newValue) { this.EntityPM.InsruanceCurrencyTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InsuranceAmount", {
        get: function () { return this.EntityPM.InsuranceAmount; },
        set: function (newValue) { this.EntityPM.InsuranceAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InsuranceAmountEnabled", {
        get: function () { return this.insuranceAmountEnabled; },
        set: function (newValue) { this.insuranceAmountEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InsuranceCurrencyEnabled", {
        get: function () { return this.insuranceCurrencyEnabled; },
        set: function (newValue) { this.insuranceCurrencyEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InsurancePercentageEnabled", {
        get: function () { return this.insurancePercentageEnabled; },
        set: function (newValue) { this.insurancePercentageEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InsruancePercentage", {
        get: function () { return this.EntityPM.InsruancePercentage; },
        set: function (newValue) {
            this.EntityPM.InsruancePercentage = newValue;
            this.IncotermLogic(this.IncotermCode); // this code replaced all the comments below.
            //if (!newValue) {
            //    this.InsuranceAmount = null;
            //    if (this.IncotermCode) {
            //        if (!(this.IncotermCode.startsWith("D") || this.IncotermCode == "CIF" || this.IncotermCode == "CIP")) {
            //            this.InsuranceAmount = null;
            //            this.InsurancePercentage = null;
            //            this.InsuranceAmount = null;
            //            this.InsruanceCurrencyTypeCode = null;
            //            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
            //            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
            //        }
            //    }
            //}
            //else {
            //    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
            //    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
            //}
            if (newValue) {
                this.CalculateInsuranceAmount(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "FreightAmountGridEnabled", {
        get: function () { return this.freightAmountGridEnabled; },
        set: function (newValue) {
            if (this.IsDisplayOnly) {
                this.freightAmountGridEnabled = false;
            }
            else {
                this.freightAmountGridEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "AddAmountEnabled", {
        get: function () { return this.addAmountEnabled; },
        set: function (newValue) {
            if (this.IsDisplayOnly) {
                this.addAmountEnabled = false;
            }
            else {
                this.addAmountEnabled = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "FreightCurrencyTypeCode", {
        get: function () { return this.EntityPM.FreightCurrencyTypeCode; },
        set: function (newValue) {
            this.EntityPM.FreightCurrencyTypeCode = newValue;
            this.LoadCurrenciesExchangeRates(true);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "IncotermCode", {
        get: function () { return this.EntityPM.IncotermCode; },
        set: function (newValue) {
            if (this.EntityPM.IncotermCode != null) {
                this.oldIncoterm = this.EntityPM.IncotermCode;
            }
            this.EntityPM.IncotermCode = newValue;
            this.incotermChanged = true;
            if (this.allowToDelete) {
                this.IncotermLogic(this.EntityPM.IncotermCode);
            }
            this.incotermChanged = false;
            this.allowToDelete = true;
            this.GetInsurancePercentDefault();
        },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceGeneralTabComponent.prototype.GetInsurancePercentDefault = function () {
        //let goToInsuranceInUNF = false;
        var _this = this;
        //if (this.declarationPM.IsConnectedToUnifreight) {
        //    if (AmitalGatewayUtil.Instance.AmitalBrowserInUse && !AppTool.IsNullOrEmpty(this.declarationPM.CustomFileNo)) {
        //        goToInsuranceInUNF = true;
        //    }
        //}
        //}
        //if (!goToInsuranceInUNF || (this.EntityPM.InvoiceCounterKey.toString() != this.declarationPM.PrimaryInvoiceCounterKey && this.declarationPM.SupplierInvoices.length > 0) ||
        //    this.InsruancePercentage != null || this.InsuranceAmount != null || (!this.IncotermCode.startsWith("E") && !this.IncotermCode.startsWith("F"))) {
        //    return;
        //}
        //if (_OpInsurancePercent != null) {
        //    if (!_OpInsurancePercent.IsComplete) {
        //        _OpInsurancePercent.Cancel();
        //    }
        //}
        /////////////////////////
        //In case(IsconnectedToUnifreight = True) & (ICIM_INSUR_PERCncotermCode statrs with “E” or “F”) & (InsruancePercent + InsruanceAmount = Null)  - WI 26677
        //Check customer default “CIM_INSUR_PERC” , if has data , fill that value in InsruancePercent & calc the InsuranceValue
        //This process will be done only on first supplier invoice 
        /////////////////////////
        var firstInvoice = this.Parent.Get1SupplierInvoice();
        if (!this.IsFirstInvoice()) {
            return;
        }
        if (!this.declarationPM.IsConnectedToUnifreight) {
            return;
        }
        var inco = this.IncotermCode || "";
        if (inco.startsWith("E") || inco.startsWith("F") || inco.startsWith("CPT") || inco.startsWith("CFR")) {
        }
        else {
            return;
        }
        if (Tools_1.AppTool.IsNullOrZero(this.InsruancePercentage) && Tools_1.AppTool.IsNullOrZero(this.InsuranceAmount)) {
            //only if user not insert 
        }
        else {
            return;
        }
        if (this.declarationPM.SupplierInvoices.length > 0) {
            if (this.declarationPM.SupplierInvoices[0].SequenceNumeric == this.EntityPM.SequenceNumeric) {
                ///first
            }
            else {
                return;
            }
        }
        else {
            ///first 1
        }
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetInsurancePercentDefault(this.declarationPM.CustomerCode, this.declarationPM.Tenant)
            .subscribe(function (res) {
            _this._OpInsurancePercent_Completed(res.Result); // += _OpInsurancePercent_Completed;
        });
    };
    SupplierInvoiceGeneralTabComponent.prototype._OpInsurancePercent_Completed = function (res) {
        //if (!_OpInsurancePercent.IsCanceled) {
        //    if (!_OpInsurancePercent.HasError) {
        //        _OpInsurancePercent.Completed -= _OpInsurancePercent_Completed;
        //        if (_OpInsurancePercent.Value != null) {
        var stringInsurancePercentage = res.insurancePercent;
        var decimalInsurancePercentage;
        //decimal.TryParse(stringInsurancePercentage, out decimalInsurancePercentage);
        var insurancePerc = Number(stringInsurancePercentage);
        if (insurancePerc > 0) {
            this.InsruancePercentage = Number(stringInsurancePercentage); //decimal.TryParse(stringInsurancePercentage, out decimalInsurancePercentage) ? decimalInsurancePercentage : (decimal ?)null;
        }
        //FirePropertyChanged("InsurancePercentage");
        //        }
        //    }
        //}
    };
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InvoiceAmount", {
        //private GetCountryPURForItems() {
        //    this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        //    var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        //    myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_CTRY", "NON", "NON", this.declarationPM.Tenant)
        //        .subscribe(response => {
        //            this.CurrentSession.StopBusyIndicator();
        //            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
        //                this.IsCountryPURForItems = true;
        //            }
        //        });
        //}
        get: function () { return this.EntityPM.InvoiceAmount; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceAmount != newValue) {
                //this.Parent.calculateCommission = true; //old
            }
            this.EntityPM.InvoiceAmount = newValue;
            if (this.Parent.TotalForeignCurrency == null)
                this.Parent.TotalForeignCurrency = 0;
            this.Parent.Difference = this.Parent.TotalForeignCurrency - (newValue);
            var percentage;
            //if (this.declarationPM.SupplierInvoices.length > 0) {
            //    percentage = this.declarationPM.SupplierInvoices[0].InsruancePercentage;
            //}
            //else {
            //    percentage = this.InsurancePercentage;
            //}
            var firstInvoice = this.Parent.Get1SupplierInvoice();
            percentage = firstInvoice.InsruancePercentage;
            if (percentage != null) {
                this.CalculateInsuranceAmount(percentage);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "TotalFreightAmountInInvoiceCurrency", {
        get: function () { return this.totalFreightAmountInInvoiceCurrency; },
        set: function (newValue) { this.totalFreightAmountInInvoiceCurrency = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "TotalFreightInFreightCurrency", {
        get: function () { return this.EntityPM.TotalFreightInFreightCurrency; },
        set: function (newValue) { this.EntityPM.TotalFreightInFreightCurrency = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "InsurancePercentage", {
        get: function () { return this.EntityPM.InsruancePercentage; },
        set: function (newValue) {
            this.EntityPM.InsruancePercentage = newValue;
            if (newValue) {
                this.CalculateInsuranceAmount(newValue);
            }
            else {
                this.InsuranceAmount = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceGeneralTabComponent.prototype.ReCalculateInsuranceAmount = function () {
        if (this.EntityPM.InsruancePercentage) {
            this.CalculateInsuranceAmount(this.EntityPM.InsruancePercentage);
        }
    };
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "TotalFreightAmountInNIS", {
        get: function () { return this.EntityPM.TotalFreightInNIS; },
        set: function (newValue) { this.EntityPM.TotalFreightInNIS = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) { this.isChecked = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceGeneralTabComponent.prototype, "IsValueForCustomsOnly", {
        get: function () { return this.EntityPM.IsValueForCustomsOnly; },
        set: function (newValue) { this.EntityPM.IsValueForCustomsOnly = newValue; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    SupplierInvoiceGeneralTabComponent.prototype.OnInvoiceNumberLostFocus = function (invoiceNumberTextBox) {
        var _this = this;
        if (this.declarationPM != null && this.declarationPM.SupplierInvoices.length >= 0) {
            if (this.EntityPM.InvoiceNumber) {
                //server method
                //var supplierInvoiceService: SupplierInvoiceService = new SupplierInvoiceService();
                //supplierInvoiceService.GetCheckIfInvoiceNumberExists(this.EntityPM.DeclarationId, this.EntityPM.InvoiceNumber, this.EntityPM.InvoiceCounterKey).subscribe((resp: ServiceResponse) => {
                //    if (!resp.HasError) {
                //        if (resp.Result) {
                //            var newValue = this.InvoiceNumber;
                //            var confirm = new ConfirmWindow();
                //            confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");
                //            confirm.ShowNoButton = true;
                //            confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה" + resp.Result.SequenceNumeric + "- האם להמשיך ?");
                //            confirm.WindowClosed.subscribe((event: any) => {
                //                confirm.Close();
                //                this.InvoiceNumber = newValue;
                //                if (confirm.Yes) {
                //                    SessionLocator.SustainFocusOnCell = false;
                //                }
                //                else {
                //                    SessionLocator.SustainFocusOnCell = true;
                //                    console.log(invoiceNumberTextBox.InputId);
                //                    var element = document.getElementById(invoiceNumberTextBox.InputId);
                //                    if (element) {
                //                        element.focus();
                //                    }
                //                }
                //            });
                //        }
                //    }
                //});
                //client method
                var invoices = [];
                invoices = this.declarationPM.SupplierInvoices;
                invoices = invoices.concat(this.Parent.NewInvoices);
                var exist = invoices.find(function (d) { return d.InvoiceNumber == _this.EntityPM.InvoiceNumber; });
                if (exist) {
                    //show confirm window
                    var newValue = this.InvoiceNumber;
                    var confirm = new ConfirmWindow_1.ConfirmWindow();
                    confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
                    confirm.ShowNoButton = true;
                    confirm.Show(" קיים כבר חשבון ספק עם מספר חשבון זהה - שורה" + exist.SequenceNumeric + "- האם להמשיך ?");
                    confirm.WindowClosed.subscribe(function (event) {
                        confirm.Close();
                        _this.InvoiceNumber = newValue;
                        if (confirm.Yes) {
                            SessionLocator_1.SessionLocator.SustainFocusOnCell = false;
                        }
                        else {
                            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
                            console.log(invoiceNumberTextBox.InputId);
                            var element = document.getElementById(invoiceNumberTextBox.InputId);
                            if (element) {
                                element.focus();
                            }
                        }
                    });
                }
            }
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.CopyAmountList = function () {
        this.FreightCopyList.length = this.AmountList.Length;
        this.FreightCopyList = this.AmountList.Collection.concat();
    };
    SupplierInvoiceGeneralTabComponent.prototype.CopyNowClicked = function () {
        for (var _i = 0, _a = this.ItemsSource.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            if (item.InvoiceQuantityType == null) {
                if (item.QunatityTypeCode != null) {
                    var s = item.QunatityTypeCode.slice(1, item.QunatityTypeCode.length - 1);
                    //var s = item.QunatityTypeCode.split('(');
                    //var st = s[1].split(')');
                    item.InvoiceQuantityType = s;
                }
            }
        }
        if (this.ItemsSource.Collection.length == 500) {
            var msg = new MessageWindow_1.MessageWindow();
            msg.Show(" עודכנו רק 500 הפריטים המוצגים");
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.UpdateProcessClicked = function () {
        var _this = this;
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 500;
        logWindow.ShowCloseButton = true;
        windowArgs.SupplierInvoicePM = this.EntityPM;
        //  windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.UpdateProcessCode");
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.SelectionCompleted(comp);
                }
            });
        });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/UpdateProcessCodeComponent');
    };
    SupplierInvoiceGeneralTabComponent.prototype.SelectionCompleted = function (args) {
        if (args.UpdateAll) {
            for (var _i = 0, _a = this.EntityPM.SupplierInvoiceItems.filter(function (d) { return !d.IsParent; }); _i < _a.length; _i++) {
                var item = _a[_i];
                var exist = item.SupplierInvoiceItemProcesTypes.filter(function (d) { return d.ProcessTypeCode == args.ProcessTypeCode; })[0];
                if (!exist) {
                    var processType = new SupplierInvoiceItemProcesTypePM_1.SupplierInvoiceItemProcesTypePM(item);
                    processType.DeclarationId = this.declarationPM.Id;
                    processType.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
                    processType.InvoiceItemLineNumber = item.LineNumber;
                    processType.Tenant = this.EntityPM.Tenant;
                    processType.ProcessTypeCode = args.ProcessTypeCode;
                    this.itemGovernmentProcedureTypeListService.getSingleFromCache(args.ProcessTypeCode).subscribe(function (response) {
                        var result = response.Result;
                        processType.ProcessTypeName = result.LocalName;
                    });
                    item.ItemAdditionalStatus = true;
                    item.AddSupplierInvoiceItemProcesType(processType);
                }
            }
            for (var _b = 0, _c = this.ItemsSource.Collection; _b < _c.length; _b++) {
                var item = _c[_b];
                item.ItemAdditionalStatusVisibility = true;
            }
            if (this.EntityPM.IsAccumalated) {
                if (this.AccumulatedFilterSelectedValue != 'Accumulated') {
                    var msg = new MessageWindow_1.MessageWindow();
                    msg.RTL = true;
                    var count = this.EntityPM.SupplierInvoiceItems.length;
                    msg.Show("קוד תהליך נשמר בהצלחה ב-" + count + " שורות ");
                }
            }
            else {
                var msg = new MessageWindow_1.MessageWindow();
                msg.RTL = true;
                var count = this.EntityPM.SupplierInvoiceItems.length;
                msg.Show("קוד תהליך נשמר בהצלחה ב-" + count + " שורות ");
            }
        }
        else {
            if (args.ItemsSource) {
                var items = args.ItemsSource.Collection;
                var _loop_2 = function (item) {
                    number = args.ItemsSource.Collection.filter(function (d) { return d.Number == item.SequenceNumeric; })[0];
                    if (number) {
                        exist = item.SupplierInvoiceItemProcesTypes.filter(function (d) { return d.ProcessTypeCode == args.ProcessTypeCode; })[0];
                        if (!exist) {
                            processType = new SupplierInvoiceItemProcesTypePM_1.SupplierInvoiceItemProcesTypePM(item);
                            processType.DeclarationId = this_1.declarationPM.Id;
                            processType.InvoiceCounterKey = this_1.EntityPM.InvoiceCounterKey;
                            processType.InvoiceItemLineNumber = item.LineNumber;
                            processType.Tenant = this_1.EntityPM.Tenant;
                            processType.ProcessTypeCode = args.ProcessTypeCode;
                            this_1.itemGovernmentProcedureTypeListService.getSingleFromCache(args.ProcessTypeCode).subscribe(function (response) {
                                var result = response.Result;
                                processType.ProcessTypeName = result.LocalName;
                            });
                            item.ItemAdditionalStatus = true;
                            item.AddSupplierInvoiceItemProcesType(processType);
                        }
                    }
                };
                var this_1 = this, number, exist, processType;
                for (var _d = 0, _e = this.EntityPM.SupplierInvoiceItems.filter(function (d) { return !d.IsParent; }); _d < _e.length; _d++) {
                    var item = _e[_d];
                    _loop_2(item);
                }
                var _loop_3 = function (item) {
                    number = args.ItemsSource.Collection.filter(function (d) { return d.Number == item.SequenceNumeric; })[0];
                    if (number) {
                        item.ItemAdditionalStatusVisibility = true;
                    }
                };
                var number;
                for (var _f = 0, _g = this.ItemsSource.Collection; _f < _g.length; _f++) {
                    var item = _g[_f];
                    _loop_3(item);
                }
                if (this.EntityPM.IsAccumalated) {
                    if (this.AccumulatedFilterSelectedValue != 'Accumulated') {
                        var msg = new MessageWindow_1.MessageWindow();
                        var ItemSourceCount = args.ItemsSource.Length;
                        msg.RTL = true;
                        msg.Show("קוד תהליך נשמר בהצלחה ב-" + ItemSourceCount + " שורות ");
                        // msg.Show("קוד תהליך נשמר בהצלחה בשורות " + ItemSourceCount );
                    }
                }
                else {
                    var msg = new MessageWindow_1.MessageWindow();
                    var ItemSourceCount = args.ItemsSource.Length;
                    msg.RTL = true;
                    msg.Show("קוד תהליך נשמר בהצלחה ב-" + ItemSourceCount + " שורות ");
                    //  msg.Show("קוד תהליך נשמר בהצלחה בשורות " + ItemSourceCount );
                }
            }
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.GetFreightTotals = function () {
        var _this = this;
        this.supplierInvoiceService.GetTotalForeignCurrencyForInvoice(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey).subscribe(function (response) {
            _this.Parent.TotalForeignCurrency = response.Result;
            //for (let item of items)// this.entitypm(d=> d. SupplierInvoiceItemViewModel item in InvoiceItemsObslist.Where(d => d.entityPM.CounterKey == 0))
            //{
            //    if (item.ItemPrice != null)
            //        this.TotalForeignCurrency = (TotalForeignCurrency != null ? TotalForeignCurrency : 0) + item.ItemPrice;
            //}
            if (isNaN(_this.Parent.TotalForeignCurrency))
                _this.Parent.TotalForeignCurrency = 0;
            var amount = _this.InvoiceAmount;
            if (isNaN(_this.InvoiceAmount))
                amount = 0;
            _this.Parent.Difference = _this.Parent.TotalForeignCurrency - amount;
            if (_this.Parent.TotalForeignCurrency != 0) {
                if (_this.Parent.Difference != null) {
                    if (_this.Parent.Difference != 0) {
                        _this.Parent.DifferenceColor = Tools_1.FontTool.Red; //red
                    }
                    else {
                        _this.Parent.DifferenceColor = Tools_1.FontTool.Green; //green
                    }
                }
            }
            else {
                _this.Parent.DifferenceColor = Tools_1.FontTool.Black;
            }
        });
    };
    SupplierInvoiceGeneralTabComponent.prototype.IncotermLogic = function (IncotermCode) {
        var _this = this;
        var firstInvoice = this.Parent.Get1SupplierInvoice();
        if (this.EntityPM != null && this.EntityPM.IncotermCode != null) {
            //#region for insurance
            if (!this.IsFirstInvoice()) {
                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
            }
            else {
                if (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP") {
                    if (this.incotermChanged) {
                        if (this.InsuranceAmount == null && this.InsurancePercentage == null && this.InsruanceCurrencyTypeCode == null) {
                            if (this.InsuranceAmount != null) {
                                this.InsuranceAmount = null;
                            }
                            if (this.InsruanceCurrencyTypeCode != null) {
                                this.InsruanceCurrencyTypeCode = null;
                            }
                            if (this.InsurancePercentage != null) {
                                this.InsurancePercentage = null;
                            }
                            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        }
                        else {
                            this.timerToken = setTimeout(function () {
                                var confirm = new ConfirmWindow_1.ConfirmWindow();
                                confirm.Cancel = true;
                                confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                                confirm.ShowNoButton = true;
                                confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
                                confirm.WindowClosed.subscribe(function (event) {
                                    if (confirm.Yes) {
                                        if (_this.InsuranceAmount != null) {
                                            _this.InsuranceAmount = null;
                                        }
                                        if (_this.InsruanceCurrencyTypeCode != null) {
                                            _this.InsruanceCurrencyTypeCode = null;
                                        }
                                        if (_this.InsurancePercentage != null) {
                                            _this.InsurancePercentage = null;
                                        }
                                        _this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                                        _this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                                        _this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                                    }
                                    else {
                                        _this.allowToDelete = false;
                                        _this.IncotermCode = _this.oldIncoterm;
                                    }
                                });
                            }, 200);
                        }
                    }
                    else {
                        if (this.InsuranceAmount != null) {
                            this.InsuranceAmount = null;
                        }
                        if (this.InsruanceCurrencyTypeCode != null) {
                            this.InsruanceCurrencyTypeCode = null;
                        }
                        if (this.InsurancePercentage != null) {
                            this.InsurancePercentage = null;
                        }
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                    }
                }
                else if (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR") {
                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                    if (this.InsurancePercentage == null) {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                        if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        }
                    }
                    else {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                    }
                }
                else if (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F")) {
                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                    if (this.InsurancePercentage == null) {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                        if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                        }
                    }
                    else {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                    }
                }
                else {
                    if (this.InsurancePercentage != null) {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                    }
                    else if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                    }
                    else {
                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                    }
                }
            }
        }
        else {
            if (!this.IsFirstInvoice()) {
                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
            }
            else if (!(this.EntityPM.SequenceNumeric != 1 && this.EntityPM.SequenceNumeric != null) || (this.isNewEntity && this.declarationPM.SupplierInvoices.length > 0)) {
                this.AddAmountEnabled = true;
                this.FreightAmountGridEnabled = true;
                this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);
                if (this.InsurancePercentage == null) {
                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
                    if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
                    }
                    else {
                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
                    }
                }
                else {
                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
                }
            }
        }
    };
    //public IncotermLogic(IncotermCode: string) {
    //    var firstInvoice: SupplierInvoicePM = this.Parent.Get1SupplierInvoice();
    //    if (this.EntityPM != null && this.EntityPM.IncotermCode != null) {
    //        //#region For freight 
    //        if (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP") {
    //            if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0 && this.incotermChanged) {
    //                this.timerToken = setTimeout(() => {
    //                    var confirm = new ConfirmWindow();
    //                    confirm.Cancel = true;
    //                    confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
    //                    confirm.ShowNoButton = true;
    //                    confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
    //                    confirm.WindowClosed.subscribe((event: any) => {
    //                        if (confirm.Yes) {
    //                            //for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
    //                            //    this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
    //                            //}
    //                            //for (let item of this.FreightCopyList) {
    //                            //    this.AmountList.Remove(item);
    //                            //}
    //                            //this.FreightCurrencyTypeCode = null;
    //                            //this.EntityPM.TotalFreightInFreightCurrency = 0;
    //                            //this.AddAmountEnabled = false;
    //                            //this.FreightAmountGridEnabled = false;
    //                            //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                            if (this.InsuranceAmount != null) {
    //                                this.InsuranceAmount = null;
    //                            }
    //                            if (this.InsruanceCurrencyTypeCode != null) {
    //                                this.InsruanceCurrencyTypeCode = null;
    //                            }
    //                            if (this.InsurancePercentage != null) {
    //                                this.InsurancePercentage = null;
    //                            }
    //                            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                        }
    //                        else {
    //                            this.allowToDelete = false;
    //                            this.IncotermCode = this.oldIncoterm;
    //                            //this.termsOfSaleTypeListService.getSingle(IncotermCode).subscribe(response => {
    //                            //    var incoterm: TermsOfSaleTypeList = response.Result;
    //                            //    this.EntityPM.IncotermName = incoterm.LocalName;
    //                            //});
    //                        }
    //                    });
    //                }, 1);
    //            }
    //            else {
    //                //this.AddAmountEnabled = false;
    //                //this.FreightAmountGridEnabled = false;
    //                //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //            }
    //        }
    //        //else if (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR") {
    //        //    if (this.EntityPM.SupplierInvoiceFreightAmounts.length > 0 && this.incotermChanged) {
    //        //        this.timerToken = setTimeout(() => {
    //        //            var confirm = new ConfirmWindow();
    //        //            confirm.Cancel = true;
    //        //            confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
    //        //            confirm.ShowNoButton = true;
    //        //            confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
    //        //            confirm.WindowClosed.subscribe((event: any) => {
    //        //                if (confirm.Yes) {
    //        //                    for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
    //        //                        this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
    //        //                    }
    //        //                    this.CopyAmountList();
    //        //                    for (let item of this.FreightCopyList) {
    //        //                        this.AmountList.Remove(item);
    //        //                    }
    //        //                    this.FreightCurrencyTypeCode = null;
    //        //                    this.EntityPM.TotalFreightInFreightCurrency = 0;
    //        //                    this.AddAmountEnabled = false;
    //        //                    this.FreightAmountGridEnabled = false;
    //        //                    this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //        //                }
    //        //                else {
    //        //                    this.allowToDelete = false;
    //        //                    IncotermCode = this.oldIncoterm;
    //        //                }
    //        //            });
    //        //        }, 200);
    //        //    }
    //        //    else {
    //        //        this.AddAmountEnabled = false;
    //        //        this.FreightAmountGridEnabled = false;
    //        //        this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //        //    }
    //        //}
    //        else if (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F")) {
    //            this.AddAmountEnabled = true;
    //            this.FreightAmountGridEnabled = true;
    //            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //        }
    //        else {
    //            this.AddAmountEnabled = true;
    //            this.FreightAmountGridEnabled = true;
    //            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //        }
    //        //#endregion
    //        //#region for insurance
    //        if (!this.IsFirstInvoice()) {//if ((this.EntityPM.SequenceNumeric != 1 && this.EntityPM.SequenceNumeric != null) || (this.isNewEntity && this.declarationPM.SupplierInvoices.length > 0)) {
    //            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //        }
    //        else {
    //            if (this.EntityPM.IncotermCode.startsWith("D") || this.EntityPM.IncotermCode == "CIF" || this.EntityPM.IncotermCode == "CIP") {
    //                if (this.incotermChanged) {
    //                    if (this.EntityPM.SupplierInvoiceFreightAmounts.length == 0) {
    //                        if (this.InsuranceAmount == null && this.InsurancePercentage == null && this.InsruanceCurrencyTypeCode == null) {
    //                            if (this.InsuranceAmount != null) {
    //                                this.InsuranceAmount = null;
    //                            }
    //                            if (this.InsruanceCurrencyTypeCode != null) {
    //                                this.InsruanceCurrencyTypeCode = null;
    //                            }
    //                            if (this.InsurancePercentage != null) {
    //                                this.InsurancePercentage = null;
    //                            }
    //                            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                        }
    //                        else {
    //                            this.timerToken = setTimeout(() => {
    //                                var confirm = new ConfirmWindow();
    //                                confirm.Cancel = true;
    //                                confirm.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
    //                                confirm.ShowNoButton = true;
    //                                confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.DeleteAmounts"));
    //                                confirm.WindowClosed.subscribe((event: any) => {
    //                                    if (confirm.Yes) {
    //                                        //for (let item of this.EntityPM.SupplierInvoiceFreightAmounts) {
    //                                        //    this.EntityPM.RemoveSupplierInvoiceFreightAmount(item);
    //                                        //}
    //                                        //for (let item of this.FreightCopyList) {
    //                                        //    this.AmountList.Remove(item);
    //                                        //}
    //                                        //this.FreightCurrencyTypeCode = null;
    //                                        //this.EntityPM.TotalFreightInFreightCurrency = 0;
    //                                        //this.AddAmountEnabled = false;
    //                                        //this.FreightAmountGridEnabled = false;
    //                                        //this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                                        if (this.InsuranceAmount != null) {
    //                                            this.InsuranceAmount = null;
    //                                        }
    //                                        if (this.InsruanceCurrencyTypeCode != null) {
    //                                            this.InsruanceCurrencyTypeCode = null;
    //                                        }
    //                                        if (this.InsurancePercentage != null) {
    //                                            this.InsurancePercentage = null;
    //                                        }
    //                                        this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                                        this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                                    }
    //                                    else {
    //                                        this.allowToDelete = false;
    //                                        IncotermCode = this.oldIncoterm;
    //                                    }
    //                                });
    //                            }, 200);
    //                        }
    //                    }
    //                }
    //                else {
    //                    if (this.InsuranceAmount != null) {
    //                        this.InsuranceAmount = null;
    //                    }
    //                    if (this.InsruanceCurrencyTypeCode != null) {
    //                        this.InsruanceCurrencyTypeCode = null;
    //                    }
    //                    if (this.InsurancePercentage != null) {
    //                        this.InsurancePercentage = null;
    //                    }
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                }
    //            }
    //            else if (this.EntityPM.IncotermCode == "CPT" || this.EntityPM.IncotermCode == "CFR") {
    //                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                if (this.InsurancePercentage == null) {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                    if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                    }
    //                }
    //                else {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                }
    //                //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //            }
    //            else if (this.EntityPM.IncotermCode.startsWith("E") || this.EntityPM.IncotermCode.startsWith("F")) {
    //                this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                if (this.InsurancePercentage == null) {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                    if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                    }
    //                }
    //                else {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                }
    //                //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                //this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //            }
    //            else {
    //                if (this.InsurancePercentage != null) {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //                }
    //                else if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                }
    //                else {
    //                    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                }
    //            }
    //        }
    //        // #endregion
    //    }
    //    else {
    //        if (!this.IsFirstInvoice()) {
    //            this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //            this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //        }
    //        else if (!(this.EntityPM.SequenceNumeric != 1 && this.EntityPM.SequenceNumeric != null) || (this.isNewEntity && this.declarationPM.SupplierInvoices.length > 0)) {
    //            this.AddAmountEnabled = true;
    //            this.FreightAmountGridEnabled = true;
    //            this.UIProperties.SetEnabled("FreightCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //            if (this.InsurancePercentage == null) {
    //                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //                if (this.InsruanceCurrencyTypeCode != null || this.InsuranceAmount != null) {
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
    //                }
    //                else {
    //                    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //                }
    //            }
    //            else {
    //                this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
    //                this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    //            }
    //            //else{
    //            //    this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", true);
    //            //    this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", true);
    //            //    this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
    //            //}
    //        }
    //    }
    //}
    SupplierInvoiceGeneralTabComponent.prototype.AddFreightAmount = function () {
        if (this.IsDisplayOnly)
            return; // go back -_-
        if (this.AddAmountEnabled) {
            var item = new SupplierInvoiceFreightAmountPM_1.SupplierInvoiceFreightAmountPM(this.EntityPM);
            item.DeclarationId = this.EntityPM.DeclarationId;
            item.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
            item.Tenant = this.EntityPM.Tenant;
            item.ChangeSetOp = "Insert";
            this.EntityPM.AddSupplierInvoiceFreightAmount(item);
            this.BuildFreightAmountsList();
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.Add = function () {
        if (this.IsDisplayOnly)
            return; // go back -_-
        var line = 0;
        var sequence = 0;
        if (this.EntityPM.SupplierInvoiceItems.length > 0) {
            if (isNaN(this.EntityPM.InvoiceItemLastLineNumber))
                this.EntityPM.InvoiceItemLastLineNumber = 0;
            line = this.EntityPM.InvoiceItemLastLineNumber;
            this.EntityPM.InvoiceItemLastLineNumber = this.EntityPM.InvoiceItemLastLineNumber + 1;
        }
        else {
            this.EntityPM.InvoiceItemLastLineNumber = 1;
            line = 0;
        }
        //if (this.EntityPM.FullItemsCount < 500 || AppTool.IsNullOrEmpty(this.EntityPM.FullItemsCount)) {
        // var items = this.EntityPM.SupplierInvoiceItems..sort(d => d.SequenceNumeric);
        var items = this.EntityPM.SupplierInvoiceItems.sort(function (a, b) { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1; });
        if (items.length == 0)
            sequence = 0;
        else {
            sequence = items[this.EntityPM.SupplierInvoiceItems.length - 1].SequenceNumeric;
        }
        //}
        //else {
        //    sequence = this.EntityPM.FullItemsCount;
        //}
        line += 1;
        sequence += 1;
        var item = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(this.EntityPM);
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.CounterKey = this.EntityPM.InvoiceCounterKey;
        item.Tenant = this.EntityPM.Tenant;
        item.LineNumber = line;
        item.OrderByLineNo = line.toString();
        item.SequenceNumeric = sequence;
        item.LastCopyFromOrderNo = "10000";
        this.EntityPM.FullChildrenCount++;
        this.ChildrenCount = "(" + this.EntityPM.FullChildrenCount + ")";
        if (!this.EntityPM.SupplierInvoiceItems.includes(item)) {
            this.EntityPM.AddSupplierInvoiceItem(item);
            this.ItemsSource.Insert(new SupplierInvoiceItemLine(item, this));
            //this.CurrentSession.ResetRowIndex();
            if (isNaN(this.EntityPM.FullItemsCount))
                this.EntityPM.FullItemsCount = 0;
            this.EntityPM.FullItemsCount = this.EntityPM.FullItemsCount + 1;
            this.EntityPM.MaxSequence = this.EntityPM.MaxSequence + 1;
        }
        this.calculateTotals(false, null);
    };
    SupplierInvoiceGeneralTabComponent.prototype.Add___ = function () {
        if (this.IsDisplayOnly)
            return; // go back -_-
        var line = 0;
        var sequence = 0;
        if (this.EntityPM.SupplierInvoiceItems.length > 0) {
            if (isNaN(this.EntityPM.InvoiceItemLastLineNumber))
                this.EntityPM.InvoiceItemLastLineNumber = 0;
            line = this.EntityPM.InvoiceItemLastLineNumber;
        }
        if (this.EntityPM.FullItemsCount < 500 || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.FullItemsCount)) {
            // var items = this.EntityPM.SupplierInvoiceItems..sort(d => d.SequenceNumeric);
            var items = this.EntityPM.SupplierInvoiceItems.sort(function (a, b) { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1; });
            if (items.length == 0)
                sequence = 0;
            else {
                sequence = items[this.EntityPM.SupplierInvoiceItems.length - 1].SequenceNumeric;
            }
        }
        else {
            sequence = this.EntityPM.FullItemsCount;
        }
        line += 1;
        sequence += 1;
        var item = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(this.EntityPM);
        item.DeclarationId = this.EntityPM.DeclarationId;
        item.CounterKey = this.EntityPM.InvoiceCounterKey;
        item.Tenant = this.EntityPM.Tenant;
        item.LineNumber = line;
        item.OrderByLineNo = line.toString();
        item.SequenceNumeric = sequence;
        this.EntityPM.FullChildrenCount++;
        this.ChildrenCount = "(" + this.EntityPM.FullChildrenCount + ")";
        if (!this.EntityPM.SupplierInvoiceItems.includes(item)) {
            this.EntityPM.AddSupplierInvoiceItem(item);
            this.ItemsSource.Insert(new SupplierInvoiceItemLine(item, this));
            //this.CurrentSession.ResetRowIndex();
            if (isNaN(this.EntityPM.FullItemsCount))
                this.EntityPM.FullItemsCount = 0;
            this.EntityPM.FullItemsCount = this.EntityPM.FullItemsCount + 1;
            this.EntityPM.MaxSequence = this.EntityPM.MaxSequence + 1;
        }
        this.calculateTotals(false, null);
    };
    SupplierInvoiceGeneralTabComponent.prototype.BuildFreightAmountsList = function () {
        this.AmountList.Clear();
        for (var i = 0; i < this.EntityPM.SupplierInvoiceFreightAmounts.length; i++) {
            this.AmountList.Insert(new SupplierInvoiceFreightAmountLine(this.EntityPM.SupplierInvoiceFreightAmounts[i], this));
        }
        if (this.EntityPM.SupplierInvoiceFreightAmounts.length == 0) {
            this.FreightCurrencyTypeCode = null;
        }
        if (this.EntityPM.SupplierInvoiceFreightAmounts.length == 0) {
            var item = new SupplierInvoiceFreightAmountPM_1.SupplierInvoiceFreightAmountPM(this.EntityPM);
            item.DeclarationId = this.EntityPM.DeclarationId;
            item.InvoiceCounterKey = this.EntityPM.InvoiceCounterKey;
            item.Tenant = this.EntityPM.Tenant;
            item.ChangeSetOp = "Insert";
            this.EntityPM.IsDirty = false;
            this.AmountList.Insert(new SupplierInvoiceFreightAmountLine(item, this), false);
        }
        this.CopyAmountList();
    };
    SupplierInvoiceGeneralTabComponent.prototype.LoadCurrenciesExchangeRates = function (recalculateTotals) {
        var _this = this;
        var currencyRates = "";
        if (this.AmountList != null && this.AmountList.Length > 0) {
            for (var _i = 0, _a = this.AmountList.Collection; _i < _a.length; _i++) {
                var line = _a[_i];
                currencyRates = currencyRates + "," + line.CurrencyTypeCode;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.FreightCurrencyTypeCode)) {
                if (!currencyRates.includes(this.FreightCurrencyTypeCode)) {
                    currencyRates = currencyRates + "," + this.FreightCurrencyTypeCode;
                }
            }
            this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(currencyRates, this.declarationPM.TaxationDateTime).subscribe(function (response) {
                var result = response.Result;
                if (result) {
                    _this.customsExchangeRates = result;
                    if (recalculateTotals) {
                        _this.UpdateTotalFreightInInvoiceCurrencyAndInNIS();
                        _this.ReCalculateInsuranceAmount();
                    }
                }
            });
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.UpdateTotalFreightInInvoiceCurrencyAndInNIS = function () {
        var _this = this;
        var totalFreightInNIS = 0;
        var totalFreightInInvoice = 0;
        var _loop_4 = function (item) {
            if (item.CurrencyTypeCode == "ILS") {
                amountInNIS = item.Amount;
                totalFreightInNIS = totalFreightInNIS + amountInNIS;
                //if (totalFreightInNIS != null) {
                //    totalFreightInNIS = Math.round(totalFreightInNIS);
                //}
            }
            else {
                if (this_2.customsExchangeRates) {
                    rate = this_2.customsExchangeRates.filter(function (d) { return d.CurrencyTypeCode == item.CurrencyTypeCode; })[0];
                    if (rate != null) {
                        amountInNIS = item.Amount * rate.ExchangeRate;
                        totalFreightInNIS = totalFreightInNIS + amountInNIS;
                        //if (totalFreightInNIS != null) {
                        //    totalFreightInNIS = Math.round(totalFreightInNIS);
                        //}
                    }
                }
            }
        };
        var this_2 = this, amountInNIS, rate;
        for (var _i = 0, _a = this.AmountList.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            _loop_4(item);
        }
        if (this.FreightCurrencyTypeCode == "ILS") {
            totalFreightInInvoice = totalFreightInNIS;
        }
        else {
            var invoiceRate = this.customsExchangeRates.filter(function (d) { return d.CurrencyTypeCode == _this.FreightCurrencyTypeCode; })[0];
            if (invoiceRate != null) {
                totalFreightInInvoice = totalFreightInNIS / invoiceRate.ExchangeRate;
            }
        }
        //if (totalFreightInInvoice != null) {
        //    totalFreightInInvoice = Math.round(totalFreightInInvoice);
        //}
        if (this.EntityPM.TotalFreightInFreightCurrency != totalFreightInInvoice) {
            this.TotalFreightAmountInInvoiceCurrency = totalFreightInInvoice;
            this.TotalFreightInFreightCurrency = totalFreightInInvoice;
        }
        if (this.EntityPM.TotalFreightInNIS != totalFreightInNIS) {
            this.TotalFreightAmountInNIS = totalFreightInNIS;
        }
        var percentage;
        //if (this.declarationPM.SupplierInvoices.length > 0) {
        //    percentage = this.declarationPM.SupplierInvoices[0].InsruancePercentage;
        //}
        //else {
        //    percentage = this.InsurancePercentage;
        //}
        var firstInvoice = this.Parent.Get1SupplierInvoice();
        percentage = firstInvoice.InsruancePercentage;
        if (percentage != null) {
            this.CalculateInsuranceAmount(percentage);
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.CalculateInsuranceAmount = function (value) {
        var _this = this;
        var amount = (isNaN(this.InvoiceAmount)) ? 0 : this.InvoiceAmount;
        var total = (isNaN(this.TotalFreightAmountInInvoiceCurrency)) ? 0 : this.TotalFreightAmountInInvoiceCurrency;
        var firstInvoice = this.Parent.Get1SupplierInvoice();
        if (this.IsFirstInvoice()) { // if (this.SequenceNumeric == 1 || this.declarationPM.SupplierInvoices.length == 0 && !this.SequenceNumeric) {
            if (this.InsurancePercentage == null || this.InsurancePercentage == 0) {
                return;
            }
            // this.InsuranceAmount = (amount + total) * (this.InsurancePercentage / 100);
            this.InsruanceCurrencyTypeCode = this.InvoiceCurrencyTypeCode;
        }
        //if (value != null) {
        //    this.InsuranceAmount = Math.round(value);
        //}
        //else {
        //    this.InsuranceAmount = value;
        //}
        //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
        //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
        //this.InsruanceCurrencyTypeCode = this.InvoiceCurrencyTypeCode;
        var firstInvoiceRate = 0;
        var totalFreight = 0;
        var totalAmount = 0;
        var InvocieCurrencyRate = 0;
        var rate = 0;
        var amount = 0;
        var total = 0;
        var ratePM;
        var firstRatePM;
        if (this.ExchangeRates) { // this is a bug in errorslog filter of undefined, solution: if no exchange rate try to load them if not it will not be calculated----mohammad.
            ratePM = this.ExchangeRates.filter(function (d) { return d.CurrencyTypeCode == _this.InvoiceCurrencyTypeCode; })[0]; //.ExchangeRate;
            firstRatePM = this.ExchangeRates.filter(function (d) { return d.CurrencyTypeCode == firstInvoice.InvoiceCurrencyTypeCode; })[0]; //.FirstExchangeRate;
        }
        else {
            this.LoadCurrenciesExchangeRates(true);
            return;
        }
        if (firstRatePM) {
            firstInvoiceRate = firstRatePM.ExchangeRate;
        }
        if (ratePM) {
            InvocieCurrencyRate = ratePM.ExchangeRate;
        }
        var invoice;
        //if (this.SequenceNumeric == 1 || this.declarationPM.SupplierInvoices.length == 0 && !this.SequenceNumeric) {
        //    invoice = this.EntityPM;
        //}
        //else {
        //    invoice = this.declarationPM.SupplierInvoices[0];
        //}
        invoice = this.Parent.Get1SupplierInvoice(); // instead of doing the above code.
        amount = (isNaN(this.InvoiceAmount)) ? 0 : this.InvoiceAmount;
        total = (isNaN(this.TotalFreightInFreightCurrency)) ? 0 : this.TotalFreightInFreightCurrency;
        var FreightCurrencyRate = this.ExchangeRates.filter(function (d) { return d.CurrencyTypeCode == _this.FreightCurrencyTypeCode; })[0];
        if (FreightCurrencyRate) {
            rate = FreightCurrencyRate.ExchangeRate;
            if (InvocieCurrencyRate > 0) {
                total = total * (rate / InvocieCurrencyRate);
            }
            else {
                total = total * rate;
            }
        }
        var _loop_5 = function (item) {
            if (item.FreightCurrencyTypeCode != this_3.InvoiceCurrencyTypeCode) { //calculate the freightamount
                if (!Tools_1.AppTool.IsNullOrEmpty(item.TotalFreightInFreightCurrency)) {
                    result = this_3.ExchangeRates.filter(function (d) { return d.CurrencyTypeCode == item.FreightCurrencyTypeCode; })[0]; //.ExchangeRate;
                    if (result && InvocieCurrencyRate != 0) {
                        rate = result.ExchangeRate;
                        totalFreight = totalFreight + (item.TotalFreightInFreightCurrency * (rate / InvocieCurrencyRate));
                    }
                }
            }
            else {
                totalFreight = totalFreight + item.TotalFreightInFreightCurrency;
            }
            // calculate insurance
            if (invoice.InsruancePercentage) { // if no percentage then no automatic insurance.
                if (item.InvoiceCurrencyTypeCode != this_3.InvoiceCurrencyTypeCode) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item.InvoiceCurrencyTypeCode)) {
                        x = this_3.ExchangeRates.filter(function (d) { return d.CurrencyTypeCode == item.InvoiceCurrencyTypeCode; })[0];
                        if (x && InvocieCurrencyRate != 0) {
                            rate = x.ExchangeRate;
                            totalAmount = totalAmount + ((isNaN(item.InvoiceAmount)) ? 0 : item.InvoiceAmount * (rate / InvocieCurrencyRate));
                        }
                    }
                }
                else {
                    totalAmount = totalAmount + ((isNaN(item.InvoiceAmount)) ? 0 : item.InvoiceAmount);
                }
            }
        };
        var this_3 = this, result, x;
        for (var _i = 0, _a = this.declarationPM.SupplierInvoices.filter(function (d) { return d.SequenceNumeric != _this.EntityPM.SequenceNumeric; }); _i < _a.length; _i++) {
            var item = _a[_i];
            _loop_5(item);
        }
        var amountInCurrentInvoiceCurrency = (amount + total + totalAmount + totalFreight) * (invoice.InsruancePercentage / 100);
        var amountInFirstInoviceCurrency = 0;
        if (this.IsFirstInvoice()) {
            amountInFirstInoviceCurrency = amountInCurrentInvoiceCurrency;
        }
        else {
            amountInFirstInoviceCurrency = amountInCurrentInvoiceCurrency * InvocieCurrencyRate / firstInvoiceRate;
        }
        if (isNaN(amountInFirstInoviceCurrency)) {
            amountInFirstInoviceCurrency = 0;
        }
        var insAmt = amountInFirstInoviceCurrency == 0 ? null : amountInFirstInoviceCurrency; //(amount + total + totalAmount + totalFreight) * (invoice.InsruancePercentage / 100);
        if (insAmt == null) {
            invoice.InsuranceAmount = 0;
        }
        else {
            invoice.InsuranceAmount = Number(insAmt.toFixed(2));
        }
        if (this.IsFirstInvoice()) {
            this.InsuranceAmount = invoice.InsuranceAmount;
        }
        this.IncotermLogic(this.IncotermCode); // this is instead of just closing the two fields. below 
        //this.UIProperties.SetEnabled("InsuranceAmount", "Customs.SupplierInvoice", false);
        //this.UIProperties.SetEnabled("InsruanceCurrencyTypeCode", "Customs.SupplierInvoice", false);
    };
    SupplierInvoiceGeneralTabComponent.prototype.OnInsuranceAmountLostFocus = function (event) {
        this.IncotermLogic(this.IncotermCode); // insteaad of doing the code below run the original code.
        //if (AppTool.IsNullOrEmpty(this.InsuranceAmount)) {
        //    if (this.InsurancePercentage == null && this.InsruanceCurrencyTypeCode == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
        //        }
        //    }
        //    else {
        //    if (this.InsurancePercentage == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
        //        }
        //    }
    };
    SupplierInvoiceGeneralTabComponent.prototype.OnInsruanceCurrencyLostFocus = function (event) {
        this.IncotermLogic(this.IncotermCode); // insteaad of doing the code below run the original code.
        //if (this.InsruanceCurrencyTypeCode != null) {
        //    if (this.InsurancePercentage == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", false);
        //        }
        //    }
        //    else {
        //    if (this.InsurancePercentage == null && this.InsuranceAmount == null) {
        //        this.UIProperties.SetEnabled("InsruancePercentage", "Customs.SupplierInvoice", true);
        //        }
        //    }
    };
    SupplierInvoiceGeneralTabComponent.prototype.OnRowEnded = function ($event) {
        var _this = this;
        //console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            setTimeout(function () { return _this.Add(); }, 1);
            //this.Add();
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.OnFocus = function () {
        if (this.ItemsSource.Length == 0) {
            this.Add();
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.Dispose = function () {
        if (this.ItemsSource) {
            this.ItemsSource.Collection.forEach(function (item) {
                item.Dispose();
            });
        }
        if (this.AccumulatedFilterChangedEvent) {
            this.AccumulatedFilterChangedEvent.unsubscribe();
            this.AccumulatedFilterChangedEvent = null;
        }
        if (this.SearchFilterChangedEvent) {
            this.SearchFilterChangedEvent.unsubscribe();
            this.SearchFilterChangedEvent = null;
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.SearchMethod = function () {
        var _this = this;
        if (!this.IsDisplayOnly) {
            var windowArgs = {};
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 700;
            logWindow.Height = 500;
            windowArgs.ImporterId = this.declarationPM.ImporterId;
            logWindow.ShowCloseButton = true;
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            logWindow.WindowArgs = windowArgs;
            logWindow.Title = "חיפוש ספקים מורחב";
            logWindow.ComponentLoaded.subscribe(function (comp) {
                logWindow.WindowClosed.subscribe(function (s) {
                    if (s) {
                        _this.SetVendorId(comp);
                    }
                });
            });
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/VendorExtendedSearchComponent');
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.SetVendorId = function (args) {
        if (args.SelectedRow) {
            if (!Tools_1.AppTool.IsNullOrEmpty(args.SelectedRow.DBVendorID)) {
                this.VendorId = args.SelectedRow.DBVendorID;
            }
            else {
                this.VendorId = args.SelectedRow.VendorId;
            }
        }
    };
    // Action Buttons
    SupplierInvoiceGeneralTabComponent.prototype.UpdateCertificatesButtonClicked = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.EntityPM = this.EntityPM;
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.MultiCertificateUpdate");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 666;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function (event) {
            if (event == 'ok') {
                //client method
                if (_this.ItemsSource.Collection) {
                    _this.ItemsSource.Collection.forEach(function (item) {
                        item.SetCertificateStatusVisibility();
                    });
                }
                //server method - replaced with client method
                //this.ReloadEntity(); 
            }
        });
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/MultiCertificateUpdate/MultiCertificateUpdateComponent');
    };
    SupplierInvoiceGeneralTabComponent.prototype.ReloadEntity = function () {
        this.ReloadEntityEvent.emit();
    };
    SupplierInvoiceGeneralTabComponent.prototype.IsFirstInvoice = function () {
        var firstInvoice = this.Parent.Get1SupplierInvoice();
        return (this.EntityPM.InvoiceCounterKey == firstInvoice.InvoiceCounterKey);
    };
    SupplierInvoiceGeneralTabComponent.prototype.InvoiceAmountBlur = function (text) {
        if (this.old_amount != this.EntityPM.InvoiceAmount ||
            this.old_currency != this.EntityPM.InvoiceCurrencyTypeCode ||
            this.old_vendor != this.EntityPM.VendorId)
            this.Parent.CalculateCommissionPercentage();
        this.old_amount = this.EntityPM.InvoiceAmount;
        this.old_currency = this.EntityPM.InvoiceCurrencyTypeCode;
        this.old_vendor = this.EntityPM.VendorId;
    };
    //#region Remark tooltip
    SupplierInvoiceGeneralTabComponent.prototype.onCellSelected = function ($event, Item) {
        if (this.SelectedRow != Item) {
            this.OnSelectedItemChanged(Item);
        }
    };
    SupplierInvoiceGeneralTabComponent.prototype.OnSelectedItemChanged = function (selectedRow) {
        console.log("OnSelectedItemChanged > ", selectedRow);
        if (selectedRow) {
            if (this.SelectedRow != selectedRow) {
                if (this.SelectedRow) {
                    this.SelectedRow.ShowClassifierRemarkTooltip = false; // hide CR tooltip on prev selected row
                    selectedRow.closedManullay = false;
                    //this.SelectedRow.ShowTariffErrorTooltip = false; // hide Tariff tooltip on prev selected row
                }
            }
            if (!selectedRow.closedManullay)
                selectedRow.ShowClassifierRemarkTooltip = true;
            this.SelectedRow = selectedRow;
        }
        else {
            if (this.SelectedRow) {
                this.SelectedRow.ShowClassifierRemarkTooltip = false;
                this.SelectedRow.closedManullay = false;
            }
            this.SelectedRow = null;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SupplierInvoiceGeneralTabComponent.prototype, "ReloadEntityEvent", void 0);
    SupplierInvoiceGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SupplierInvoiceGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], SupplierInvoiceGeneralTabComponent);
    return SupplierInvoiceGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.SupplierInvoiceGeneralTabComponent = SupplierInvoiceGeneralTabComponent;
var SupplierInvoiceItemLine = /** @class */ (function (_super) {
    __extends(SupplierInvoiceItemLine, _super);
    function SupplierInvoiceItemLine(EntityPM, parent) {
        var _this = _super.call(this) || this;
        _this.entityPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceItem";
        _this.DataContext = _this;
        _this.IsBlueBorderVisibile = false;
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.customsVendorListService = new CustomsVendorListService_1.CustomsVendorListService();
        _this._CustomsCountryListService = new CustomsCountryListService_1.CustomsCountryListService();
        _this.ShowClassefierRemarkInfo = false;
        _this.ShowClassifierRemarkTooltip = false;
        _this.ShowTariffErrorInfo = false;
        _this.ShowTariffErrorTooltip = false;
        _this.closedManullay = false;
        _this.ClassefierRemarkToolTipWrapper = "ClassefierRemarkToolTipWrapper";
        _this.ClassefierRemarkToolTip = "ClassefierRemarkToolTip";
        _this.TariffErrorToolTipWrapper = "TariffErrorToolTipWrapper";
        _this.TariffErrorToolTip = "TariffErrorToolTip";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.itemAdditionalStatusVisibility = false;
        _this.okVisiblity = false;
        _this.vehicleOkVisiblity = false;
        _this.wrningVisiblity = false;
        _this.valid = true;
        _this.digit = null;
        _this.checkDigit = 0;
        _this.oldvalue = 0;
        _this.doCalculate = false;
        _this.entityPM = EntityPM;
        //calculate ids
        _this.ClassefierRemarkToolTipWrapper += EntityPM.SequenceNumeric;
        _this.ClassefierRemarkToolTip += EntityPM.SequenceNumeric;
        //this.TariffErrorToolTipWrapper += EntityPM.SequenceNumeric;
        //this.TariffErrorToolTip += EntityPM.SequenceNumeric;
        ////Fill tariff error text
        //this.TariffErrorText = "מדינה לא תואמת לקוד התעריף"; //"Tarrif doesnt match country” 
        ////get currenct customs country
        //if (this.OriginCountryCode) {
        //    this._CustomsCountryListService.getSingle(this.OriginCountryCode).subscribe((res) => {
        //        var entity = res.Result;
        //        if (entity) {
        //            this.CustomsCountry = entity;
        //            this.OriginCountryName = this.CustomsCountry.LocalName;
        //        }
        //    });
        //}
        if (_this.entityPM.ClasifiedRemarks) {
            _this.ShowClassefierRemarkInfo = true;
        }
        _this.Parent = parent;
        _this.oldvalue = _this.entityPM.ItemPrice;
        if (_this.Parent.accumulationFeature == null) {
            _this.Parent.IsNotForAccumaltionVisibile = false;
        }
        if (_this.Parent.IsReadOnly) {
            _this.UIProperties.SetEnabled("NotForAccumaltion", "Customs.SupplierInvoiceItem", false);
        }
        if (_this.entityPM.SupplierInvioceItemCertificats.length == 0) {
            _this.WarningVisiblity = false;
            _this.OkVisiblity = false;
            _this.IsBlueBorderVisibile = false;
        }
        else if (_this.entityPM.CertificatesStatusCode == "1") {
            _this.OkVisiblity = true;
        }
        else if (_this.entityPM.CertificatesStatusCode == "2") {
            _this.WarningVisiblity = true;
        }
        else if (_this.entityPM.CertificatesStatusCode == "3") {
            _this.IsBlueBorderVisibile = true;
            _this.OkVisiblity = true;
        }
        else if (_this.entityPM.CertificatesStatusCode == "4") {
            _this.WarningVisiblity = true;
            _this.IsBlueBorderVisibile = true;
        }
        if (_this.entityPM.ItemAdditionalStatus) {
            _this.ItemAdditionalStatusVisibility = true;
        }
        else {
            _this.ItemAdditionalStatusVisibility = false;
        }
        if (_this.entityPM.SupplierInvoiceItemVehicles.length > 0) {
            _this.VehicleOkVisiblity = true;
        }
        _this.GetQuantityType(false);
        _this.QuantityTypeCodeLoaded =
            //this.CurrentSession.SubscriptionAdd(
            _this.CurrentSession.QuantityTypeCodeLoadedEvent.subscribe(function (res) {
                if (_this.ClassificationCode == res.ClassificationCode) {
                    _this.QunatityTypeCode = res.QuantityTypeCode;
                }
            });
        return _this;
    }
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "EditButtonName", {
        get: function () { return this.editButtonName; },
        set: function (value) { this.editButtonName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ItemAdditionalStatusVisibility", {
        get: function () { return this.itemAdditionalStatusVisibility; },
        set: function (value) { this.itemAdditionalStatusVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "OkVisiblity", {
        get: function () { return this.okVisiblity; },
        set: function (value) { this.okVisiblity = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "VehicleOkVisiblity", {
        get: function () { return this.vehicleOkVisiblity; },
        set: function (value) { this.vehicleOkVisiblity = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "WarningVisiblity", {
        get: function () { return this.wrningVisiblity; },
        set: function (value) { this.wrningVisiblity = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "MeasurmentUnit", {
        get: function () { return this.measurmentUnit; },
        set: function (value) {
            if (this.measurmentUnit != value) {
                this.measurmentUnit = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.InvoiceQuantityTypeName = value.LocalName;
            }
            else {
                this.InvoiceQuantityTypeName = null;
                this.InvoiceQuantityType = null;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode) && GITITEMCacheService_1.GITITEMCacheService.Instance.IsUnitPURForItems) {
                var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); // .ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                if (itemCodeDetails != null) {
                    itemCodeDetails.InvoiceQuantityType = this.InvoiceQuantityType;
                    itemCodeDetails.IsNew = true;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "TradeAgreement", {
        get: function () { return this.tradeAgreement; },
        set: function (value) {
            if (this.tradeAgreement != value) {
                this.tradeAgreement = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.TradeAgreementName = value.LocalName;
            }
            else {
                this.TradeAgreementName = null;
                this.TradeAgreementCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "CustomsCountry", {
        get: function () { return this.customsCountry; },
        set: function (value) {
            if (this.customsCountry != value) {
                this.customsCountry = value;
                //this.CheckTariff();
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.OriginCountryName = value.LocalName;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode) && GITITEMCacheService_1.GITITEMCacheService.Instance.IsCountryPURForItems) {
                    //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                    var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); // .ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                    if (itemCodeDetails != null) {
                        itemCodeDetails.OriginCountryCode = value.Code;
                        itemCodeDetails.OriginCountryName = value.LocalName;
                        itemCodeDetails.IsNew = true;
                    }
                }
            }
            else {
                this.OriginCountryName = null;
                this.OriginCountryCode = null;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode) && GITITEMCacheService_1.GITITEMCacheService.Instance.IsCountryPURForItems) {
                    //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                    var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); //.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                    if (itemCodeDetails != null) {
                        itemCodeDetails.OriginCountryCode = null;
                        itemCodeDetails.OriginCountryName = null;
                        itemCodeDetails.IsNew = true;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "OrderByLineNo", {
        get: function () { return this.entityPM.OrderByLineNo; },
        set: function (newValue) { this.entityPM.OrderByLineNo = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "SequenceNumeric", {
        get: function () { return this.entityPM.SequenceNumeric; },
        set: function (newValue) { this.entityPM.SequenceNumeric = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "LineNumber", {
        get: function () { return this.entityPM.LineNumber; },
        set: function (newValue) { this.entityPM.LineNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ItemCode", {
        get: function () { return this.entityPM.ItemCode; },
        set: function (newValue) { this.entityPM.ItemCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ItemDescription", {
        get: function () { return this.entityPM.ItemDescription; },
        set: function (newValue) { this.entityPM.ItemDescription = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "NotForAccumaltion", {
        get: function () { return this.entityPM.NotForAccumaltion; },
        set: function (value) {
            this.entityPM.NotForAccumaltion = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "SearchFields", {
        get: function () { return this.entityPM.SearchFields; },
        set: function (value) {
            this.entityPM.SearchFields = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ClassificationCode", {
        get: function () { return this.entityPM.ClassificationCode; },
        set: function (newValue) {
            this.entityPM.ClassificationCode = newValue;
            if (newValue == null) {
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "TradeAgreementCode", {
        get: function () { return this.entityPM.TradeAgreementCode; },
        set: function (newValue) {
            this.entityPM.TradeAgreementCode = newValue;
            //this.CheckTariff();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "TradeAgreementName", {
        get: function () { return this.entityPM.TradeAgreementName; },
        set: function (newValue) { this.entityPM.TradeAgreementName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "InvoiceQuantity", {
        get: function () { return this.entityPM.InvoiceQuantity; },
        set: function (newValue) { this.entityPM.InvoiceQuantity = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "InvoiceQuantityType", {
        get: function () { return this.entityPM.InvoiceQuantityType; },
        set: function (newValue) { this.entityPM.InvoiceQuantityType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "InvoiceQuantityTypeName", {
        get: function () { return this.entityPM.InvoiceQuantityTypeName; },
        set: function (newValue) { this.entityPM.InvoiceQuantityTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ItemPrice", {
        get: function () { return this.entityPM.ItemPrice; },
        set: function (newValue) {
            if (newValue != this.entityPM.ItemPrice) {
                this.doCalculate = true;
                this.entityPM.ItemPrice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "OriginCountryCode", {
        get: function () { return this.entityPM.OriginCountryCode; },
        set: function (newValue) {
            this.entityPM.OriginCountryCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "OriginCountryName", {
        get: function () { return this.entityPM.OriginCountryName; },
        set: function (newValue) { this.entityPM.OriginCountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "QunatityTypeCode", {
        get: function () { return this.qunatityTypeCode; },
        set: function (newValue) { this.qunatityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "IsParent", {
        get: function () { return this.entityPM.IsParent; },
        set: function (newValue) { this.entityPM.IsParent = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ClasifiedRemarks", {
        get: function () { return this.entityPM.ClasifiedRemarks; },
        set: function (newValue) { this.entityPM.ClasifiedRemarks = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "TariffErrorText", {
        get: function () { return this._TariffErrorText; },
        set: function (newValue) { this._TariffErrorText = newValue; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    SupplierInvoiceItemLine.prototype.OnItemPriceLostFocus = function (value) {
        if (this.doCalculate) {
            if (isNaN(this.ItemPrice))
                this.ItemPrice = 0;
            if (this.Parent.Parent.TotalForeignCurrency == null)
                this.Parent.Parent.TotalForeignCurrency = 0;
            if (isNaN(this.oldvalue))
                this.oldvalue = 0;
            var totalFCurr = this.Parent.Parent.TotalForeignCurrency;
            this.Parent.Parent.TotalForeignCurrency = totalFCurr - this.oldvalue + this.ItemPrice;
            this.Parent.Parent.Difference = this.Parent.Parent.TotalForeignCurrency - (this.Parent.InvoiceAmount);
        }
        this.oldvalue = this.entityPM.ItemPrice;
        this.doCalculate = false;
    };
    SupplierInvoiceItemLine.prototype.GetQuantityType = function (isChangeInvoiceQuantityType) {
        var _this = this;
        if (isChangeInvoiceQuantityType === void 0) { isChangeInvoiceQuantityType = true; }
        if (this.ClassificationCode != null) {
            var keys = Object.keys(this.Parent.ClasificationQtyTypes);
            if (keys.indexOf(this.ClassificationCode) > -1) {
                var result = this.Parent.ClasificationQtyTypes[this.ClassificationCode];
                if (result) {
                    this.QunatityTypeCode = "(" + result + ")";
                    if (this.Parent.IsChecked && isChangeInvoiceQuantityType) {
                        if (this.InvoiceQuantityType == null && result != null) {
                            this.InvoiceQuantityType = result;
                        }
                    }
                }
            }
            else {
                this.Parent.ClasificationQtyTypes[this.ClassificationCode] = null;
                var code = this.ClassificationCode.toString().slice(0, this.ClassificationCode.toString().length - 1);
                this.Parent.quantityTypeMessageService.GetQuantityType(code).subscribe(function (myServiceResponse) {
                    if (!myServiceResponse.HasError) {
                        if (!myServiceResponse.HasError) {
                            if (myServiceResponse.Result) {
                                _this.QunatityTypeCode = "(" + myServiceResponse.Result + ")";
                            }
                            else {
                                _this.QunatityTypeCode = null;
                            }
                            if (_this.Parent.IsChecked && isChangeInvoiceQuantityType) {
                                if (_this.InvoiceQuantityType == null && myServiceResponse.Result != null) {
                                    _this.InvoiceQuantityType = myServiceResponse.Result;
                                }
                            }
                            if (!(keys.indexOf(_this.ClassificationCode) > -1)) {
                                _this.Parent.ClasificationQtyTypes[_this.ClassificationCode] = myServiceResponse.Result;
                            }
                            _this.CurrentSession.QuantityTypeCodeLoadedEvent.emit({ ClassificationCode: _this.ClassificationCode, QuantityTypeCode: _this.QunatityTypeCode });
                            //QuantityTypeCodeLoadedEvent quantityLoadedEvent = currentAssemlyLocator.EventAggregator.GetEvent<QuantityTypeCodeLoadedEvent>();
                            //quantityLoadedEvent.Publish(new QuantityTypeCodeLoadedEventArgs() { ClassificationCode = ClassificationCode, QuantityTypeCode = this.QunatityTypeCode });
                        }
                    }
                });
            }
        }
        else {
            this.QunatityTypeCode = null;
        }
    };
    SupplierInvoiceItemLine.prototype.NotForAccumaltionChecked = function (checked, item) {
        if (checked) {
            this.NotForAccumaltion = true;
        }
        else {
            this.NotForAccumaltion = false;
        }
    };
    SupplierInvoiceItemLine.prototype.DeleteButtonClicked = function () {
        var _this = this;
        this.pointers = this.Parent.Pointers;
        var pointer = this.pointers.filter(function (d) { return d.Child2EntityId === _this.entityPM.LineNumber.toString(); })[0];
        if (pointer == null) {
            this.Delete();
        }
        else {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            //     confirmWindow.DisplayWariningIconImage();
            confirmWindow.Width = 450;
            confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
            confirmWindow.Height = 190;
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
            confirmWindow.NoButtonText = "Cancel";
            confirmWindow.ShowWarningImage = true;
            confirmWindow.ShowNoButton;
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.InvoiceRelatedPoiner"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.Delete();
                }
                else if (confirmWindow.No) {
                }
            });
        }
    };
    SupplierInvoiceItemLine.prototype.Delete = function () {
        if (!this.Parent.IsReadOnly) {
            var deletedItemPrice = this.entityPM.ItemPrice;
            if (this.Parent.EntityPM.SupplierInvoiceItems.includes(this.entityPM)) {
                this.Parent.EntityPM.RemoveSupplierInvoiceItem(this.entityPM);
                var sequence;
                if (this.Parent.ItemsSource.Collection[0].SequenceNumeric == this.entityPM.SequenceNumeric) {
                    sequence = this.entityPM.SequenceNumeric;
                    this.Parent.ItemsSource.Remove(this, false); //the position of this line is important :after sequence is taken
                }
                else {
                    this.Parent.ItemsSource.Remove(this, false); //the position of this line is important before sequence is taken
                    sequence = this.Parent.ItemsSource.Collection[0].SequenceNumeric;
                }
                //var sortedCollection = this.Parent.ItemsSource.Collection.sort((a, b) => { return a.SequenceNumeric - b.SequenceNumeric });
                this.Parent.ItemsSource.Collection.forEach(function (item) {
                    item.SequenceNumeric = sequence;
                    sequence++;
                });
                this.Parent.EntityPM.FullItemsCount = this.Parent.EntityPM.FullItemsCount - 1;
                this.Parent.EntityPM.FullChildrenCount--;
                this.Parent.ChildrenCount = "(" + this.Parent.EntityPM.FullChildrenCount + ")";
            }
            //this.Parent.ItemsSource.Remove(this.entityPM); 
            this.Parent.calculateTotals(true, deletedItemPrice);
        }
    };
    SupplierInvoiceItemLine.prototype.CertificateButtonClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            var windowArgs = {};
            windowArgs.SupplierInvoiceItemPM = item.entityPM;
            windowArgs.IsDisplayOnly = this.Parent.IsReadOnly;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 600;
            if (item.ClassificationCode != null) {
                logWindow.Title = "אישורים לפרט מכס" + " " + item.ClassificationCode;
            }
            else {
                logWindow.Title = "אישורים לפרט מכס";
            }
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.SetCertificateStatusVisibility(); });
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
        }
    };
    SupplierInvoiceItemLine.prototype.EditItem = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            var windowArgs = {};
            windowArgs.SupplierInvoiceItemPM = item.entityPM;
            windowArgs.Parent = item;
            windowArgs.IsDisplayOnly = this.Parent.IsReadOnly;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.SetStatusVisibility(); });
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/EditSupplierInvoiceItem');
        }
        else {
            console.log("[!] There is no item to open", item);
        }
    };
    SupplierInvoiceItemLine.prototype.SetStatusVisibility = function () {
        if (this.entityPM.ItemAdditionalStatus) {
            this.ItemAdditionalStatusVisibility = true;
        }
        else {
            this.ItemAdditionalStatusVisibility = false;
        }
    };
    SupplierInvoiceItemLine.prototype.SetCertificateStatusVisibility = function () {
        if (this.entityPM.CertificatesStatusCode == "1") {
            this.OkVisiblity = true;
            this.WarningVisiblity = false;
            this.IsBlueBorderVisibile = false;
        }
        else if (this.entityPM.CertificatesStatusCode == "2") {
            this.WarningVisiblity = true;
            this.OkVisiblity = false;
            this.IsBlueBorderVisibile = false;
        }
        else if (this.entityPM.CertificatesStatusCode == "3") {
            this.OkVisiblity = true;
            this.WarningVisiblity = false;
            this.IsBlueBorderVisibile = true;
        }
        else if (this.entityPM.CertificatesStatusCode == "4") {
            this.WarningVisiblity = true;
            this.OkVisiblity = false;
            this.IsBlueBorderVisibile = true;
        }
        else {
            this.WarningVisiblity = false;
            this.OkVisiblity = false;
            this.IsBlueBorderVisibile = false;
        }
    };
    SupplierInvoiceItemLine.prototype.ClassificationKeyUp = function (event, logCellTemplate, classificationTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnClassificationLostFocus(logCellTemplate, classificationTextBox);
        }
    };
    SupplierInvoiceItemLine.prototype.OnClassificationLostFocus = function (logCellTemplate, classificationTextBox) {
        var newValue = this.ClassificationCode;
        this.valid = true;
        this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        }
        else if (newValue.toString().length > 11) {
            this.valid = false;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));
        }
        else if (newValue.toString().length < 8) {
            this.valid = false;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
        }
        else if (newValue.toString().length == 8) {
            newValue = newValue + "00";
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + this.checkDigit;
            this.valid = true;
            ;
        }
        else if (newValue.toString().length == 9) {
            this.digit = newValue.toString().substring(8);
            newValue = newValue.toString().substring(0, 8) + "00" + newValue.toString().substring(8);
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());
            }
            else {
                this.valid = true;
            }
        }
        else if (newValue.toString().length == 10) {
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue);
            newValue = newValue + "" + this.checkDigit;
            this.valid = true;
        }
        else if (newValue.toString().length == 11) {
            this.digit = newValue.toString().substring(10);
            this.checkDigit = LuhnAlgorithm_1.LuhnAlgorithm.CalculateLuhnAlgorithm(newValue.toString().substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.valid = false;
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString());
            }
            else {
                this.valid = true;
            }
        }
        else {
            this.valid = true;
            this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
        }
        this.ClassificationCode = newValue;
        classificationTextBox.TextValue = newValue;
        if (this.valid) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = false;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode)) {
                //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); //.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                if (itemCodeDetails == null) {
                    this.AdditemCodeDetail();
                }
                else {
                    if (itemCodeDetails.ClassificationCode != this.ClassificationCode || itemCodeDetails.ItemDescription != this.ItemDescription) {
                        itemCodeDetails.ClassificationCode = this.ClassificationCode;
                        itemCodeDetails.ItemDescription = this.ItemDescription;
                        itemCodeDetails.VendorNumber = this.Parent.vendorNumber;
                        if (GITITEMCacheService_1.GITITEMCacheService.Instance.IsUnitPURForItems) {
                            itemCodeDetails.InvoiceQuantityType = this.InvoiceQuantityType;
                        }
                        if (GITITEMCacheService_1.GITITEMCacheService.Instance.IsCountryPURForItems) {
                            itemCodeDetails.OriginCountryCode = this.OriginCountryCode;
                            itemCodeDetails.OriginCountryName = this.OriginCountryName;
                        }
                        itemCodeDetails.IsNew = true;
                    }
                }
            }
            this.GetQuantityType();
        }
        else {
            //var element = document.getElementById(logCellTemplate.OuterDivId);
            // element.focus();
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: classificationTextBox.InputId });
        }
        if (this.Parent.IsChecked) {
            if (this.InvoiceQuantityType == null && this.QunatityTypeCode != null) {
                var s = this.QunatityTypeCode.slice(1, this.QunatityTypeCode.length - 1);
                this.InvoiceQuantityType = s;
            }
        }
    };
    SupplierInvoiceItemLine.prototype.AdditemCodeDetail = function () {
        var originCountryCode = null;
        var originCountryName = null;
        var invoiceQuantityType = null;
        if ( /*this.Parent*/GITITEMCacheService_1.GITITEMCacheService.Instance.IsCountryPURForItems) {
            originCountryCode = this.OriginCountryCode;
            originCountryName = this.OriginCountryName;
        }
        if (GITITEMCacheService_1.GITITEMCacheService.Instance.IsUnitPURForItems) {
            invoiceQuantityType = this.InvoiceQuantityType;
        }
        //this.Parent.Parent.ItemCode_LocalCache.push(new ItemCodeComponent(this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, this.InvoiceQuantityType));
        GITITEMCacheService_1.GITITEMCacheService.Instance. /*ItemCode_LocalCache.push*/AddItemCodeComponent(new ItemCodeComponent(this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, invoiceQuantityType, this.Parent.declarationPM.CustomerCode));
    };
    SupplierInvoiceItemLine.prototype.OnOriginCountryCodeLostFocus = function (logCellTemplate, originCountryCodeLov) {
        if (!GITITEMCacheService_1.GITITEMCacheService.Instance.IsCountryPURForItems) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode)) {
            //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
            var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); //.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
            if (itemCodeDetails != null) {
                itemCodeDetails.OriginCountryCode = this.OriginCountryCode != null ? this.OriginCountryCode : this.customsCountry != null ? this.customsCountry.Code : null;
                itemCodeDetails.OriginCountryName = this.OriginCountryName != null ? this.OriginCountryName : this.customsCountry != null ? this.customsCountry.LocalName : null;
                itemCodeDetails.IsNew = true;
            }
        }
    };
    SupplierInvoiceItemLine.prototype.ItemCodeLostFocus = function (logCellTemplate) {
        var _this = this;
        if (!this.entityPM.ClassificationCode)
            this.entityPM.ClassificationCode = "";
        if (!this.entityPM.ItemDescription)
            this.entityPM.ItemDescription = "";
        if (Tools_1.AppTool.IsNullOrEmpty(this.ItemCode)) {
            return;
        }
        //if (this.Parent.Parent.ItemCode_LocalCache != null && this.Parent.Parent.ItemCode_LocalCache.length > 0) {
        //if (GITITEMCacheService.Instance.ItemCode_LocalCache != null && GITITEMCacheService.Instance.ItemCode_LocalCache.length > 0)
        {
            //var itemCodeDetails = this.Parent.Parent.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
            var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); //.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
            if (itemCodeDetails != null) {
                var b = true;
                if (b) {
                    GITITEMCacheService_1.GITITEMCacheService.Instance.OnItemCodeAdd(this.entityPM, itemCodeDetails)
                        .then(function (myOnItemCodeAddResult) {
                        switch (myOnItemCodeAddResult) {
                            case 2 /*OnItemCodeAddResult.AddTaskToUpdateDB*/:
                                {
                                    GITITEMCacheService_1.GITITEMCacheService.Instance. /*ItemCode_LocalCache.push*/AddItemCodeComponent(new ItemCodeComponent(_this.ItemCode, _this.ClassificationCode, _this.ItemDescription, _this.Parent.vendorNumber, _this.OriginCountryCode, _this.OriginCountryName, true, _this.InvoiceQuantityType, _this.Parent.declarationPM.CustomerCode));
                                }
                                break;
                            //case OnItemCodeAddResult.OverwriteRowFromDB:
                            //case OnItemCodeAddResult.voidDoNothing:
                            default:
                                {
                                    itemCodeDetails.IsNew = true;
                                    _this.ClassificationCode = itemCodeDetails.ClassificationCode;
                                    _this.ItemDescription = itemCodeDetails.ItemDescription;
                                    if (GITITEMCacheService_1.GITITEMCacheService.Instance.IsUnitPURForItems) {
                                        _this.InvoiceQuantityType = itemCodeDetails.InvoiceQuantityType;
                                    }
                                    if ( /*this.Parent*/GITITEMCacheService_1.GITITEMCacheService.Instance.IsCountryPURForItems) {
                                        _this.OriginCountryCode = itemCodeDetails.OriginCountryCode;
                                        _this.OriginCountryName = itemCodeDetails.OriginCountryName;
                                    }
                                    _this.GetQuantityType();
                                }
                                break;
                        }
                        return;
                    });
                }
                return;
            }
        }
        this.declarationWebService.GetGITITEMPartnersItemListByItemCode(this.Parent.vendorNumber, this.Parent.declarationPM.CustomerCode, this.ItemCode, 30, false)
            .subscribe(function (response) {
            var res = response.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(res) && res.length == 1) {
                _this.PartnerItemsSelectionCompleted(_this.entityPM, res[0]);
                return;
            }
            else {
                _this.declarationWebService.GetGITITEMPartnersItemListByItemCode(_this.Parent.vendorNumber, _this.Parent.declarationPM.CustomerCode, _this.ItemCode, 30, true)
                    .subscribe(function (response) {
                    var res = response.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(res) && res.length == 1) {
                        _this.PartnerItemsSelectionCompleted(_this.entityPM, res[0]);
                        return;
                    }
                    else {
                        _this.declarationWebService.GetGITITEMPartnersItemListByName(_this.Parent.vendorNumber, _this.Parent.declarationPM.CustomerCode, _this.ItemCode, 30)
                            .subscribe(function (response) {
                            var res = response.Result;
                            if (!Tools_1.AppTool.IsNullOrEmpty(res) && res.length == 1) {
                                _this.PartnerItemsSelectionCompleted(_this.entityPM, res[0]);
                                return;
                            }
                            else {
                                //Eitancommented 15 minutes ago
                                //@odelia devashi @itzik M סיכום:
                                //גם כאשר מזינים קודם פרט מכס ואח"כ קוד פריט (מקט), עדיין צריך ליצור TASK של לימוד עצמי + שימוש ב-CACHE ברמת SESSION
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.ClassificationCode)) {
                                    _this.AdditemCodeDetail(); //Task 43218: שיפור במנגנון לימוד עצמי
                                }
                            }
                        });
                    }
                });
            }
        });
    };
    SupplierInvoiceItemLine.prototype.ItemCodeDblClick = function (logCellTemplate) {
        var _this = this;
        if (!this.Parent.IsReadOnly) {
            console.log("[Double Click] ", this.entityPM);
            if (this.Parent.IsDisplayOnly)
                return;
            //close the cell before showing window; to avoid [true] to [false] problem
            logCellTemplate.IsDisplayMode = true;
            logCellTemplate.IsEditMode = false;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 650;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsPartnersItem.Q.ItemQuery");
            logWindow.ShowCloseButton = true;
            logWindow.WindowArgs = {
                invoicePM: this.Parent.EntityPM,
                customerCode: this.Parent.declarationPM.CustomerCode,
                searchText: this.ItemCode,
            };
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.PartnerItemsSelectionCompleted(_this.entityPM, $event);
            });
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/PartnersItemsSelectionComponent');
        }
    };
    SupplierInvoiceItemLine.prototype.PartnerItemsSelectionCompleted = function (item, partnersItem) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(partnersItem)) {
            console.log("Response returned: ", partnersItem);
            if (!Tools_1.AppTool.IsNullOrEmpty(partnersItem.ItemCode) || !Tools_1.AppTool.IsNullOrEmpty(partnersItem.ClassificationCode)) {
                var b = true;
                if (b) {
                    GITITEMCacheService_1.GITITEMCacheService.Instance.OnItemCodeAdd(this.entityPM, partnersItem)
                        .then(function (myOnItemCodeAddResult) {
                        switch (myOnItemCodeAddResult) {
                            case 2: //OnItemCodeAddResult.AddTaskToUpdateDB:
                                {
                                    GITITEMCacheService_1.GITITEMCacheService.Instance. /*ItemCode_LocalCache.push*/AddItemCodeComponent(new ItemCodeComponent(item.ItemCode, item.ClassificationCode, item.ItemDescription, _this.Parent.vendorNumber, item.OriginCountryCode, item.OriginCountryName, true, item.InvoiceQuantityType, _this.Parent.declarationPM.CustomerCode));
                                    _this.GetQuantityType();
                                }
                                break;
                            //case OnItemCodeAddResult.OverwriteRowFromDB:
                            //case OnItemCodeAddResult.voidDoNothing:
                            default:
                                {
                                    item.ItemCode = partnersItem.ItemCode;
                                    item.ClassificationCode = partnersItem.ClassificationCode;
                                    item.ItemDescription = partnersItem.Name;
                                    if ( /*this.Parent*/GITITEMCacheService_1.GITITEMCacheService.Instance.IsCountryPURForItems) {
                                        item.OriginCountryCode = partnersItem.OriginCountryCode;
                                        item.OriginCountryName = partnersItem.OriginCountryName;
                                    }
                                    if (GITITEMCacheService_1.GITITEMCacheService.Instance.IsUnitPURForItems) {
                                        item.InvoiceQuantityType = partnersItem.InvoiceQuantityType;
                                    }
                                    //this.Parent.Parent.ItemCode_LocalCache.push(new ItemCodeComponent(partnersItem.ItemCode, partnersItem.ClassificationCode, partnersItem.Name, this.Parent.vendorNumber, item.OriginCountryCode, item.OriginCountryName, false, item.InvoiceQuantityType));
                                    GITITEMCacheService_1.GITITEMCacheService.Instance. /*ItemCode_LocalCache.push*/AddItemCodeComponent(new ItemCodeComponent(partnersItem.ItemCode, partnersItem.ClassificationCode, partnersItem.Name, _this.Parent.vendorNumber, item.OriginCountryCode, item.OriginCountryName, false, item.InvoiceQuantityType, _this.Parent.declarationPM.CustomerCode));
                                    _this.GetQuantityType();
                                }
                                break;
                        }
                        return;
                    });
                }
            }
        }
    };
    SupplierInvoiceItemLine.prototype.VehicleButtonClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            var windowArgs = {};
            windowArgs.SupplierInvoiceItemPM = item.entityPM;
            windowArgs.IsDisplayOnly = this.Parent.IsReadOnly;
            windowArgs.declarationPM = this.Parent.declarationPM;
            windowArgs.parent = this.Parent;
            windowArgs.SupplierInvoicePM = this.Parent.EntityPM;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 1000;
            logWindow.Height = 500;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Vehicles");
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.SetVehicleStatusVisibility(); });
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemVehicleComponent');
        }
    };
    SupplierInvoiceItemLine.prototype.SetVehicleStatusVisibility = function () {
        if (this.entityPM.VehicleStatus) {
            this.VehicleOkVisiblity = true;
        }
        else {
            this.VehicleOkVisiblity = false;
        }
    };
    SupplierInvoiceItemLine.prototype.CopyLineClicked = function (copieditem, ItemsSource) {
        var _this = this;
        var entityPM = this.Parent.EntityPM;
        if (this.Parent.IsDisplayOnly)
            return; // go back -_-
        var line = 0;
        var sequence = 0;
        var orderByLineNo;
        if (entityPM.SupplierInvoiceItems.length > 0) {
            if (isNaN(entityPM.InvoiceItemLastLineNumber))
                entityPM.InvoiceItemLastLineNumber = 0;
            line = entityPM.InvoiceItemLastLineNumber;
            entityPM.InvoiceItemLastLineNumber = entityPM.InvoiceItemLastLineNumber + 1;
        }
        var copiedFromOrderByLineNo = copieditem.entityPM.OrderByLineNo;
        var lastCopyOrderByLineNo = copieditem.entityPM.LastCopyFromOrderNo;
        var lastCopyItem = this.Parent.ItemsSource.Collection.filter(function (d) { return d.OrderByLineNo == lastCopyOrderByLineNo; })[0];
        orderByLineNo = this.GetNextOrderByLineNumber(copiedFromOrderByLineNo, lastCopyOrderByLineNo);
        line += 1;
        sequence = copieditem.SequenceNumeric; // + 1;
        var item = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(entityPM);
        item.DeclarationId = entityPM.DeclarationId;
        item.CounterKey = entityPM.InvoiceCounterKey;
        item.Tenant = entityPM.Tenant;
        item.LineNumber = line; //entityPM.InvoiceItemLastLineNumber;
        item.SequenceNumeric = sequence;
        item.OrderByLineNo = orderByLineNo;
        item.ClassificationCode = copieditem.ClassificationCode;
        item.InvoiceQuantityType = copieditem.InvoiceQuantityType;
        item.OriginCountryCode = copieditem.OriginCountryCode;
        item.OriginCountryName = copieditem.OriginCountryName;
        item.TradeAgreementCode = copieditem.TradeAgreementCode;
        item.TradeAgreementName = copieditem.TradeAgreementName;
        item.IsCopy = true;
        entityPM.FullChildrenCount++;
        copieditem.entityPM.LastCopyFromOrderNo = orderByLineNo;
        this.Parent.ChildrenCount = "(" + entityPM.FullChildrenCount + ")";
        if (!entityPM.SupplierInvoiceItems.includes(item)) {
            var copiedItemIndex;
            copiedItemIndex = this.Parent.ItemsSource.GetIndex(copieditem);
            //if (lastCopyItem) {
            //    copiedItemIndex = this.Parent.ItemsSource.GetIndex(lastCopyItem);
            //}
            //else {
            //    copiedItemIndex = this.Parent.ItemsSource.GetIndex(copieditem);
            //}
            entityPM.AddSupplierInvoiceItem(item);
            this.Parent.ItemsSource.InsertAtIndex(copiedItemIndex + 1, new SupplierInvoiceItemLine(item, this.Parent));
            //this.Parent.ItemsSource.Collection.push(new SupplierInvoiceItemLine(item, this.Parent));//Insert(new SupplierInvoiceItemLine(item, this.Parent), false);
            if (isNaN(entityPM.FullItemsCount))
                entityPM.FullItemsCount = 0;
            entityPM.FullItemsCount = entityPM.FullItemsCount + 1;
            entityPM.MaxSequence = entityPM.MaxSequence + 1;
        }
        var sequenceNo = this.Parent.ItemsSource.Collection[0].SequenceNumeric;
        var sortedCollection = this.Parent.ItemsSource.Collection.sort(function (a, b) { return a.SequenceNumeric - b.SequenceNumeric || a.LineNumber - b.LineNumber; });
        sortedCollection.forEach(function (item) {
            _this.Parent.ItemsSource.Collection.filter(function (d) { return d.LineNumber == item.LineNumber; })[0].SequenceNumeric = sequenceNo;
            sequenceNo++;
        });
        //this.Parent.ItemsSource.Collection.forEach((item: SupplierInvoiceItemLine) => {
        //    item.SequenceNumeric = sequence;
        //    sequence++;
        //});
        //this.Parent.ItemsSource.InsertCollection(sortedCollection);
        this.Parent.calculateTotals(false, null);
    };
    SupplierInvoiceItemLine.prototype.GetNextOrderByLineNumber = function (copiedOrderByLineNo, lastCopyOrderByLineNo) {
        var newCopyOrderbyNo = "";
        if (Tools_1.AppTool.IsNullOrEmpty(lastCopyOrderByLineNo)) {
            newCopyOrderbyNo = copiedOrderByLineNo + ".1";
        }
        else {
            var lastcopyLineNoparts = lastCopyOrderByLineNo.split('.');
            var incremetedPart;
            var compinedLastPart;
            var lastpartLastNo = lastcopyLineNoparts[lastcopyLineNoparts.length - 1];
            var lastPartLastNoLastNo = lastpartLastNo.substring(lastpartLastNo.length - 1);
            incremetedPart = Number(lastPartLastNoLastNo);
            compinedLastPart = Number(lastpartLastNo);
            var addedResult = "";
            if (incremetedPart != 9) {
                addedResult = (compinedLastPart + 1).toString();
            }
            else {
                addedResult = lastpartLastNo + "1";
            }
            for (var i = 0; i < lastcopyLineNoparts.length - 1; i++) {
                newCopyOrderbyNo = newCopyOrderbyNo + "." + lastcopyLineNoparts[i];
            }
            newCopyOrderbyNo = newCopyOrderbyNo + "." + addedResult;
            newCopyOrderbyNo = newCopyOrderbyNo.substr(1, newCopyOrderbyNo.length - 1);
        }
        return newCopyOrderbyNo;
    };
    SupplierInvoiceItemLine.prototype.Dispose = function () {
        this.Parent = null;
        this.DataContext = null;
        if (this.QuantityTypeCodeLoaded) {
            this.QuantityTypeCodeLoaded.unsubscribe();
        }
    };
    // timer?? because function OnSelectedItemChanged() hit before after these functions
    SupplierInvoiceItemLine.prototype.CloseButtonClicked = function () {
        var _this = this;
        var t = setTimeout(function () {
            _this.closedManullay = true;
            _this.ShowClassifierRemarkTooltip = false;
        }, 20);
    };
    SupplierInvoiceItemLine.prototype.ExcButtonClicked = function () {
        var _this = this;
        var t = setTimeout(function () {
            _this.closedManullay = false;
            _this.ShowClassifierRemarkTooltip = true;
        }, 20);
    };
    SupplierInvoiceItemLine.prototype.TariffErrorButtonClicked = function () {
        this.ShowTariffErrorTooltip = !this.ShowTariffErrorTooltip;
    };
    SupplierInvoiceItemLine.prototype.CheckTariff = function () {
        if (this.TradeAgreementCode && this.OriginCountryCode) {
            this.ShowTariffErrorInfo = (this.CustomsCountry.TarriffCode != this.TradeAgreementCode);
        }
        else {
            this.ShowTariffErrorInfo = false;
        }
    };
    return SupplierInvoiceItemLine;
}(BaseComponent_1.BaseComponent));
exports.SupplierInvoiceItemLine = SupplierInvoiceItemLine;
var SupplierInvoiceFreightAmountLine = /** @class */ (function (_super) {
    __extends(SupplierInvoiceFreightAmountLine, _super);
    function SupplierInvoiceFreightAmountLine(EntityPM, parent) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.SupplierInvoiceFreightAmount";
        _this.DataContext = _this;
        _this.entityPM = EntityPM;
        _this.Parent = parent;
        if (EntityPM.ChangeSetOp == "Insert") {
            _this.UIProperties.SetEnabled("CurrencyTypeCode", "Customs.SupplierInvoiceFreightAmount", true);
        }
        else {
            _this.UIProperties.SetEnabled("CurrencyTypeCode", "Customs.SupplierInvoiceFreightAmount", false);
        }
        return _this;
    }
    Object.defineProperty(SupplierInvoiceFreightAmountLine.prototype, "CurrencyType", {
        get: function () { return this.currencyType; },
        set: function (value) {
            if (this.currencyType != value) {
                this.currencyType = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.CurrencyTypeName = value.LocalName;
            }
            else {
                this.CurrencyTypeName = null;
                this.CurrencyTypeCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceFreightAmountLine.prototype, "CurrencyTypeCode", {
        get: function () { return this.entityPM.CurrencyTypeCode; },
        set: function (newValue) {
            var _this = this;
            if (this.Parent.AmountList.Length > 0) {
                if (newValue != null) {
                    var exist = this.Parent.EntityPM.SupplierInvoiceFreightAmounts.find(function (d) { return d.CurrencyTypeCode === newValue; });
                    this.entityPM.CurrencyTypeCode = newValue;
                    if (exist) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow.Width = 400;
                        confirmWindow.Height = 200;
                        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        confirmWindow.ShowNoButton = false;
                        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType"));
                        this.entityPM.CurrencyTypeCode = newValue;
                        this.entityPM.CurrencyTypeCode = null;
                        // this.entityPM.CurrencyTypeName = null;
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                //this.entityPM.CurrencyTypeCode = newValue;
                                _this.entityPM.CurrencyTypeCode = null;
                                _this.CurrencyTypeName = null;
                                confirmWindow.Close();
                            }
                        });
                    }
                    else {
                        this.entityPM.CurrencyTypeCode = newValue;
                        if (this.Parent.AmountList.Length == 1) {
                            this.Parent.EntityPM.AddSupplierInvoiceFreightAmount(this.entityPM);
                        }
                    }
                }
                else {
                    this.entityPM.CurrencyTypeCode = newValue;
                }
            }
            if (this.Parent.EntityPM.SupplierInvoiceFreightAmounts.length == 1) {
                this.Parent.FreightCurrencyTypeCode = newValue;
            }
            if (this.Amount != null) {
                this.Parent.LoadCurrenciesExchangeRates(true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceFreightAmountLine.prototype, "CurrencyTypeName", {
        get: function () { return this.entityPM.CurrencyTypeName; },
        set: function (newValue) { this.entityPM.CurrencyTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceFreightAmountLine.prototype, "Amount", {
        get: function () { return this.entityPM.Amount; },
        set: function (newValue) {
            this.entityPM.Amount = newValue;
        },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceFreightAmountLine.prototype.OnAmountLostFocus = function (logCellTemplate, amountItemTextBox) {
        this.Amount = amountItemTextBox.textValue;
        if (this.CurrencyTypeCode != null) {
            this.Parent.LoadCurrenciesExchangeRates(true);
        }
    };
    SupplierInvoiceFreightAmountLine.prototype.DeleteButtonClicked = function () {
        if (this.Parent.EntityPM.SupplierInvoiceFreightAmounts.includes(this.entityPM)) {
            this.Parent.EntityPM.RemoveSupplierInvoiceFreightAmount(this.entityPM);
            if (this.CurrencyTypeCode == this.Parent.FreightCurrencyTypeCode) {
                if (this.Parent.EntityPM.SupplierInvoiceFreightAmounts.length > 0) {
                    this.Parent.FreightCurrencyTypeCode = this.Parent.EntityPM.SupplierInvoiceFreightAmounts[0].CurrencyTypeCode;
                }
            }
        }
        this.Parent.LoadCurrenciesExchangeRates(true);
        this.Parent.BuildFreightAmountsList();
    };
    return SupplierInvoiceFreightAmountLine;
}(BaseComponent_1.BaseComponent));
exports.SupplierInvoiceFreightAmountLine = SupplierInvoiceFreightAmountLine;
var ItemCodeComponent = /** @class */ (function (_super) {
    __extends(ItemCodeComponent, _super);
    function ItemCodeComponent(itemCode, classificationCode, itemDescription, vendorNumber, originCountryCode, originCountryName, isNew, invoiceQuantityType, _CustomerCode) {
        var _this = _super.call(this) || this;
        _this._CustomerCode = _CustomerCode;
        _this.ItemCode = itemCode;
        _this.ClassificationCode = classificationCode;
        _this.VendorNumber = vendorNumber;
        _this.ItemDescription = itemDescription;
        _this.OriginCountryCode = originCountryCode;
        _this.OriginCountryName = originCountryName;
        _this.InvoiceQuantityType = invoiceQuantityType;
        _this.IsNew = isNew;
        return _this;
    }
    Object.defineProperty(ItemCodeComponent.prototype, "ItemCode", {
        get: function () { return this._ItemCode; },
        set: function (newValue) { this._ItemCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "ClassificationCode", {
        get: function () { return this._ClassificationCode; },
        set: function (newValue) { this._ClassificationCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "VendorNumber", {
        get: function () { return this._VendorNumber; },
        set: function (newValue) { this._VendorNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "ItemDescription", {
        get: function () { return this._ItemDescription; },
        set: function (newValue) { this._ItemDescription = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "OriginCountryCode", {
        get: function () { return this._OriginCountryCode; },
        set: function (newValue) { this._OriginCountryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "OriginCountryName", {
        get: function () { return this._OriginCountryName; },
        set: function (newValue) { this._OriginCountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "InvoiceQuantityType", {
        get: function () { return this._InvoiceQuantityType; },
        set: function (newValue) { this._InvoiceQuantityType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "CustomerCode", {
        get: function () { return this._CustomerCode; },
        set: function (newValue) { this._CustomerCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemCodeComponent.prototype, "IsNew", {
        get: function () { return this._IsNew; },
        set: function (newValue) { this._IsNew = newValue; },
        enumerable: true,
        configurable: true
    });
    return ItemCodeComponent;
}(BaseComponent_1.BaseComponent));
exports.ItemCodeComponent = ItemCodeComponent;
//# sourceMappingURL=SupplierInvoiceGeneralTabComponent.js.map