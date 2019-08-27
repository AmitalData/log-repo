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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var DeclarationDisplayOnlyChecks_1 = require("../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var CustomsVendorListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsVendorListService");
var CustomsCountryListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsCountryListService");
var DeclarationEventManager_1 = require("../../../../../Customs/Utilities/DeclarationEventManager");
var SupplierInvoiceGeneralTabComponent_1 = require("../../../../../Customsmodules/Customsdeclarationmodules/Declarationsupplierinvoice/Components/Supplierinvoices/SupplierInvoiceGeneralTabComponent");
var LuhnAlgorithm_1 = require("../../../../../Customs/Utilities/LuhnAlgorithm");
var CustomsVendorPMService_1 = require("../../../../../Customs/Services/StandardPMs/CustomsVendorPMService");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var CustomsSettingExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService");
var DeclarationClassificationComponent_1 = require("./DeclarationClassificationComponent");
var SupplierInvoiceService_1 = require("../../../../../Customs/Services/Others/SupplierInvoiceService");
var SupplierInvoiceExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var QuantityTypeMessageService_1 = require("../../../../../Customs/Services/WebServices/QuantityTypeMessageService");
var GITITEMCacheService_1 = require("../../../../../Customs/Services/Others/GITITEMCacheService");
var SInvoiceClassificationTabComponent = /** @class */ (function (_super) {
    __extends(SInvoiceClassificationTabComponent, _super);
    function SInvoiceClassificationTabComponent(entityArgs, cd) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.ObjectTableName = "Customs.SupplierInvoice";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.ParentIsDisplayOnly = false;
        _this.ShowExcludeConsignmentBoolean = false;
        _this.IsCourierDeclaration = false;
        _this.customsVendorPMService = new CustomsVendorPMService_1.CustomsVendorPMService();
        _this.vendorNumber = "";
        _this.IsCountryPURForItems = false;
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        _this.ClasificationQtyTypes = {};
        _this.quantityTypeMessageService = new QuantityTypeMessageService_1.QuantityTypeMessageService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isChecked = true; /// יש לשים לב ללוגיקות שקיימות במסך העבודה הרגיל. למשל:     בשינוי פרט מכס יש להקפיץ יחידת מידה (במסך הרגיל זה תלוי בסימון V, פה זה ההתנהגות הרגילה).
        _this.DisplayOnlyMessage = "";
        _this.supplierInvoiceService = new SupplierInvoiceService_1.SupplierInvoiceService();
        _this.NumberOfLoadedItems = 50; // COURIER HAVE TO BE NO MORE THEN 10 
        //this.ConsimentPackages = new ObservableCollection([]);
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.AddEditSupplierInvoiceDUMMYManager = new DeclarationClassificationComponent_1.AddEditSupplierInvoiceDUMMY("");
        _this.Listen();
        return _this;
    }
    SInvoiceClassificationTabComponent.prototype.ngOnDestroy = function () {
        console.log("SInvoiceClassificationTabComponent:ngOnDestroy");
        //if (this.Tab.ComponentReference && this.Tab.ComponentReference.ngOnDestroy) {
        //    this.Tab.ComponentReference.ngOnDestroy();
        //}
        this.Tab.ComponentReference = null;
        this.Tab = null;
        if (this._SubDisplayModeChanged) {
            this._SubDisplayModeChanged.unsubscribe();
            this._SubDisplayModeChanged = null;
        }
        if (this._SubConsignmentsChanged) {
            this._SubConsignmentsChanged.unsubscribe();
            this._SubConsignmentsChanged = null;
        }
    };
    SInvoiceClassificationTabComponent.prototype.Listen = function () {
        var _this = this;
        this._SubDisplayModeChanged =
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.subscribe(function (IsDisplayOnly) {
                _this.ParentIsDisplayOnly = IsDisplayOnly;
                _this.SetScreenFieldsEditability();
            });
        this._SubConsignmentsChanged =
            DeclarationEventManager_1.DeclarationEventManager.ConsignmentsChanged.subscribe(function (e) {
                console.log("ConsignmentsChanged", _this.declarationPM, e);
            });
    };
    Object.defineProperty(SInvoiceClassificationTabComponent.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (newValue) { this.isChecked = newValue; },
        enumerable: true,
        configurable: true
    });
    SInvoiceClassificationTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    SInvoiceClassificationTabComponent.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            return;
        }
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.CurrentSession.CurrentEditComponent.EntityPM).subscribe(function (response) {
            var displayOnlyCheckResult = response.Result;
            _this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (_this.IsDisplayOnly) {
                _this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            _this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(_this.IsDisplayOnly);
        });
    };
    SInvoiceClassificationTabComponent.prototype.SetTabArgs = function (args) {
        var _this = this;
        this.EntityPM = args.EntityPM;
        this.Tab = args.Tab;
        this.IsDisplayOnly = args.Disabled;
        this.ParentIsDisplayOnly = args.Disabled;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VendorId)) {
            this.customsVendorPMService.get(this.EntityPM.VendorId).subscribe(function (myResponse) {
                _this.vendor = myResponse.Result;
                if (_this.vendor != null) {
                    _this.vendorNumber = _this.vendor.VendorNumber;
                }
            });
        }
        this.AddEditSupplierInvoiceDUMMYManager = args.Parent.AddEditSupplierInvoiceDUMMYManager;
        this.GetFreightTotals();
        if (!this.declarationPM)
            this.declarationPM = args.Parent.DeclarationPM;
        this.IsCourierDeclaration = this.declarationPM.IsCourierDeclaration;
        this.ShowExcludeConsignmentBoolean = this.declarationPM.Consignments.length == 1;
        //this.EntityPM.PropertyChanged.subscribe((event) => { console.log("PropertyChanged: ", event); });
        if (this.IsDisplayOnly) {
            this.SetScreenFieldsEditability();
        }
        // show xml errors
        if (!Tools_1.AppTool.IsNullOrEmpty(args.DecErrors)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(args.DecErrors.Field)) {
                this.UIProperties.SetValidity(args.DecErrors.Field, "Customs.Consignment", false, args.DecErrors.Description);
            }
        }
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        this.customsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString())
            .subscribe(function (customsSettingList) {
            if (customsSettingList) {
                _this.CurrentSession.StopBusyIndicator();
                if (_this.AddEditSupplierInvoiceDUMMYManager.IsNewEntity) {
                    var autoFillAccountType = customsSettingList.Result ? customsSettingList.Result.AutoFillAccountType : false;
                    if (autoFillAccountType) {
                        //this.AccountTypeCode = "380";
                    }
                }
                var autoUnitMeasurement = customsSettingList.Result ? customsSettingList.Result.AutoUnitMeasurement : false;
                if (autoUnitMeasurement) {
                    _this.IsChecked = true;
                }
            }
            _this.GetCountryPURForItems();
        });
        //}
        var TempItemSource = [];
        for (var i = 0; i < this.EntityPM.SupplierInvoiceItems.length; i++) {
            TempItemSource.push(new SInvoiceItemClassificationLine(this.EntityPM.SupplierInvoiceItems[i], this));
        }
        this.ItemsSource.InsertCollection(TempItemSource);
        this.CurrentSession.StopBusyIndicator();
        console.log("Tabs Args: ", args);
    };
    SInvoiceClassificationTabComponent.prototype.GetFreightTotals = function () {
        var _this = this;
        this.supplierInvoiceService.GetTotalForeignCurrencyForInvoice(this.EntityPM.DeclarationId, this.EntityPM.InvoiceCounterKey).subscribe(function (response) {
            _this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = response.Result;
            //for (let item of items)// this.entitypm(d=> d. SupplierInvoiceItemViewModel item in InvoiceItemsObslist.Where(d => d.entityPM.CounterKey == 0))
            //{
            //    if (item.ItemPrice != null)
            //        this.TotalForeignCurrency = (TotalForeignCurrency != null ? TotalForeignCurrency : 0) + item.ItemPrice;
            //}
            if (isNaN(_this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency))
                _this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;
            var amount = _this.InvoiceAmount;
            if (isNaN(_this.InvoiceAmount))
                amount = 0;
            _this.AddEditSupplierInvoiceDUMMYManager.Difference = _this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - amount;
            if (_this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency != 0) {
                if (_this.AddEditSupplierInvoiceDUMMYManager.Difference != null) {
                    if (_this.AddEditSupplierInvoiceDUMMYManager.Difference != 0) {
                        _this.AddEditSupplierInvoiceDUMMYManager.DifferenceColor = Tools_1.FontTool.Red; //red
                    }
                    else {
                        _this.AddEditSupplierInvoiceDUMMYManager.DifferenceColor = Tools_1.FontTool.Green; //green
                    }
                }
            }
            else {
                _this.AddEditSupplierInvoiceDUMMYManager.DifferenceColor = Tools_1.FontTool.Black;
            }
        });
    };
    SInvoiceClassificationTabComponent.prototype.GetCountryPURForItems = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_I_PUR_CTRY", "NON", "NON", this.declarationPM.Tenant)
            .subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                _this.IsCountryPURForItems = true;
            }
        });
    };
    SInvoiceClassificationTabComponent.prototype.BuildItemsList = function () {
        this.CurrentSession.StartBusyIndicator("Customs.General.O.Loading");
        this.ItemsSource.Clear();
        //this.ParentItems = [];
        //this.ChildrenItems = [];
        //var TempItemSource: SInvoiceItemClassificationLine[] = [];
        //this.AccumulatedMessageVisibility = false;
        for (var i = 0; i < this.EntityPM.SupplierInvoiceItems.length; i++) {
            //TempItemSource.push(new SInvoiceItemClassificationLine(this.EntityPM.SupplierInvoiceItems[i], this));
            this.ItemsSource.Insert(new SInvoiceItemClassificationLine(this.EntityPM.SupplierInvoiceItems[i], this));
        }
        //this.ItemsSource.InsertCollection(TempItemSource);
        this.CurrentSession.StopBusyIndicator();
        //  this.originalItemSource.InsertCollection(TempItemSource);
    };
    SInvoiceClassificationTabComponent.prototype.SetScreenFieldsEditability = function () {
        console.log("SetScreenFieldsEditability: " + this.EntityPM);
        this.UIProperties.SetEnabled("CargoDescription", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("OriginCountryCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SecondCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ReceiverWarehouseCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("UnloadPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsLastReleaseFromWarehous", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StorageSiteCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("LoadingPortCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ThirdCargoID", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CargoDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManifestDate", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeliveryPlaceName", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WeightValue", this.ObjectTableName, !this.IsDisplayOnly);
    };
    SInvoiceClassificationTabComponent.prototype.calculateTotals = function (deleteItem, deletedItemPrice) {
        if (deleteItem) {
            if (deletedItemPrice == null)
                deletedItemPrice = 0;
            if (this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency == null)
                this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;
            this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - deletedItemPrice;
            this.AddEditSupplierInvoiceDUMMYManager.Difference = this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - (this.InvoiceAmount);
        }
    };
    Object.defineProperty(SInvoiceClassificationTabComponent.prototype, "InvoiceAmount", {
        get: function () { return this.EntityPM.InvoiceAmount; },
        set: function (newValue) {
            if (this.EntityPM.InvoiceAmount != newValue) {
                //this.Parent.calculateCommission = true; //old
            }
            this.EntityPM.InvoiceAmount = newValue;
            if (this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency == null)
                this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;
            this.AddEditSupplierInvoiceDUMMYManager.Difference = this.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - (newValue);
        },
        enumerable: true,
        configurable: true
    });
    SInvoiceClassificationTabComponent.prototype.OnSelectedItemChanged = function (selectedRow) {
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
    SInvoiceClassificationTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);
        for (var _i = 0, _a = this.CurrentSession.CurrentEditComponent.EntityPM.Consignments; _i < _a.length; _i++) {
            var item_1 = _a[_i];
            for (var _b = 0, _c = item_1.ConsignmentPackages; _b < _c.length; _b++) {
                var line = _c[_b];
                if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                    var errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                    if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                        errors.push(errorMessage);
                    }
                }
            }
        }
        if (errors.length > 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }
        else {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            if (this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicator("");
                this.declarationPMService.update(this.CurrentSession.CurrentEditComponent.EntityPM).subscribe(function (response) {
                    var declaration = response.Result;
                    _this.CurrentSession.StopBusyIndicator();
                    if (!Tools_1.AppTool.IsNullOrEmpty(declaration)) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
                            _this.EditInvoice(item);
                        }
                    }
                });
            }
            else {
                this.EditInvoice(item);
            }
        }
    };
    SInvoiceClassificationTabComponent.prototype.EditInvoice = function (mySInvoiceItemClassificationLine) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        var supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        var decPM = this.CurrentSession.CurrentEditComponent.EntityPM; // this component 
        supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(decPM /*this.EntityPM*/.Id, this.EntityPM.InvoiceCounterKey, 0, this.NumberOfLoadedItems, "parent").subscribe(function (response) {
            var windowArgs = {};
            windowArgs.EntityPM = response.Result;
            windowArgs.declarationPM = decPM /*this.EntityPM*/;
            windowArgs.NumberOfLoadedItems = _this.NumberOfLoadedItems;
            var windowTitle = "Supplier Invoice";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 995; // don't change this width!
            logWindow.Height = 600;
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(decPM /*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.InvoiceNumber + "-" + decPM /*this.EntityPM*/.DeclarationNumber;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(decPM /*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + decPM /*this.EntityPM*/.DeclarationNumber;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(decPM /*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.InvoiceNumber;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(decPM /*this.EntityPM*/.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
            }
            windowArgs.IsDisplayOnly = false; // //this.IsDisplayOnly;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            windowArgs.FromClassificationJumpToSII = mySInvoiceItemClassificationLine.entityPM.SequenceNumeric;
            ///this.CD.detach();
            logWindow.WindowClosed.subscribe(function (event) {
                if (event != 'cancel') {
                    _this.RefreshEntity();
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                else {
                    _this.ReloadMyScreen();
                }
                ///this.CD.reattach();
            });
            logWindow.IsHideHeader = true;
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    SInvoiceClassificationTabComponent.prototype.ReloadMyScreen = function () {
        ///this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        //this.getSupplierInvoices();
        //this.DisplayOnlyCheck();
    };
    SInvoiceClassificationTabComponent = __decorate([
        core_1.Component({
            selector: 'SInvoiceClassificationTabContent',
            moduleId: module.id,
            templateUrl: './SInvoiceClassificationTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], SInvoiceClassificationTabComponent);
    return SInvoiceClassificationTabComponent;
}(BaseComponent_1.BaseComponent));
exports.SInvoiceClassificationTabComponent = SInvoiceClassificationTabComponent;
var SInvoiceItemClassificationLine = /** @class */ (function (_super) {
    __extends(SInvoiceItemClassificationLine, _super);
    function SInvoiceItemClassificationLine(EntityPM, parent) {
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
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.itemAdditionalStatusVisibility = false;
        _this.valid = true;
        _this.digit = null;
        _this.checkDigit = 0;
        _this.oldvalue = 0;
        _this.doCalculate = false;
        _this.entityPM = EntityPM;
        //calculate ids
        if (_this.entityPM.ClasifiedRemarks) {
            _this.ShowClassefierRemarkInfo = true;
        }
        _this.Parent = parent;
        _this.oldvalue = _this.entityPM.ItemPrice;
        return _this;
    }
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "EditButtonName", {
        get: function () { return this.editButtonName; },
        set: function (value) { this.editButtonName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "ItemAdditionalStatusVisibility", {
        get: function () { return this.itemAdditionalStatusVisibility; },
        set: function (value) { this.itemAdditionalStatusVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "MeasurmentUnit", {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "TradeAgreement", {
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
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "CustomsCountry", {
        get: function () { return this.customsCountry; },
        set: function (value) {
            if (this.customsCountry != value) {
                this.customsCountry = value;
                //this.CheckTariff();
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.OriginCountryName = value.LocalName;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode) && this.Parent.IsCountryPURForItems) {
                    //var itemCodeDetails = this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                    var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); /*ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0]*/
                    ;
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
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode) && this.Parent.IsCountryPURForItems) {
                    //var itemCodeDetails = this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                    var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); //ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
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
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "OrderByLineNo", {
        get: function () { return this.entityPM.OrderByLineNo; },
        set: function (newValue) { this.entityPM.OrderByLineNo = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "SequenceNumeric", {
        get: function () { return this.entityPM.SequenceNumeric; },
        set: function (newValue) { this.entityPM.SequenceNumeric = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "ItemCode", {
        get: function () { return this.entityPM.ItemCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "ItemDescription", {
        //public set ItemCode(newValue: string) { this.entityPM.ItemCode = newValue; }
        get: function () { return this.entityPM.ItemDescription; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "NotForAccumaltion", {
        //public set ItemDescription(newValue: string) { this.entityPM.ItemDescription = newValue; }
        get: function () { return this.entityPM.NotForAccumaltion; },
        set: function (value) {
            this.entityPM.NotForAccumaltion = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "SearchFields", {
        get: function () { return this.entityPM.SearchFields; },
        set: function (value) {
            this.entityPM.SearchFields = value;
        },
        enumerable: true,
        configurable: true
    });
    SInvoiceItemClassificationLine.prototype.SetDirty = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent == null)
            return;
        var declarationPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        declarationPM.IsDirty = true;
        this.entityPM.ChangeSetOp = "Update";
        var si = declarationPM.SupplierInvoices.filter(function (si) {
            //si.DeclarationId == this.entityPM.DeclarationId &&
            return si.InvoiceCounterKey == _this.entityPM.CounterKey;
        })[0];
        si.IsDirty = true;
        si.ChangeSetOp = "Update"; //ca sera sera
    };
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "ClassificationCode", {
        get: function () { return this.entityPM.ClassificationCode; },
        set: function (newValue) {
            this.entityPM.ClassificationCode = newValue;
            this.SetDirty();
            if (newValue == null) {
                this.UIProperties.SetValidity("ClassificationCode", "Customs.SupplierInvoiceItem", true, "");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "TradeAgreementCode", {
        get: function () { return this.entityPM.TradeAgreementCode; },
        set: function (newValue) {
            this.SetDirty();
            this.entityPM.TradeAgreementCode = newValue;
            //this.CheckTariff();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "TradeAgreementName", {
        get: function () { return this.entityPM.TradeAgreementName; },
        set: function (newValue) {
            this.entityPM.TradeAgreementName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "InvoiceQuantity", {
        get: function () { return this.entityPM.InvoiceQuantity; },
        set: function (newValue) {
            this.SetDirty();
            this.entityPM.InvoiceQuantity = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "InvoiceQuantityType", {
        get: function () { return this.entityPM.InvoiceQuantityType; },
        set: function (newValue) {
            this.SetDirty();
            this.entityPM.InvoiceQuantityType = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "InvoiceQuantityTypeName", {
        get: function () { return this.entityPM.InvoiceQuantityTypeName; },
        set: function (newValue) { this.entityPM.InvoiceQuantityTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "ItemPrice", {
        get: function () { return this.entityPM.ItemPrice; },
        set: function (newValue) {
            this.SetDirty();
            if (newValue != this.entityPM.ItemPrice) {
                this.doCalculate = true;
                this.entityPM.ItemPrice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "OriginCountryCode", {
        get: function () { return this.entityPM.OriginCountryCode; },
        set: function (newValue) {
            this.SetDirty();
            this.entityPM.OriginCountryCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "OriginCountryName", {
        get: function () { return this.entityPM.OriginCountryName; },
        set: function (newValue) { this.entityPM.OriginCountryName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "QunatityTypeCode", {
        get: function () { return this.qunatityTypeCode; },
        set: function (newValue) { this.qunatityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "IsParent", {
        get: function () { return this.entityPM.IsParent; },
        set: function (newValue) { this.entityPM.IsParent = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "ClasifiedRemarks", {
        get: function () { return this.entityPM.ClasifiedRemarks; },
        set: function (newValue) { this.entityPM.ClasifiedRemarks = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SInvoiceItemClassificationLine.prototype, "TariffErrorText", {
        get: function () { return this._TariffErrorText; },
        set: function (newValue) { this._TariffErrorText = newValue; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    SInvoiceItemClassificationLine.prototype.OnItemPriceLostFocus = function (value) {
        if (this.doCalculate) {
            if (isNaN(this.ItemPrice))
                this.ItemPrice = 0;
            if (this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency == null)
                this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = 0;
            if (isNaN(this.oldvalue))
                this.oldvalue = 0;
            var totalFCurr = this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency;
            this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency = totalFCurr - this.oldvalue + this.ItemPrice;
            this.Parent.AddEditSupplierInvoiceDUMMYManager.Difference = this.Parent.AddEditSupplierInvoiceDUMMYManager.TotalForeignCurrency - (this.Parent.InvoiceAmount);
        }
        this.oldvalue = this.entityPM.ItemPrice;
        this.doCalculate = false;
    };
    SInvoiceItemClassificationLine.prototype.ClassificationKeyUp = function (event, logCellTemplate, classificationTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnClassificationLostFocus(logCellTemplate, classificationTextBox);
        }
    };
    SInvoiceItemClassificationLine.prototype.OnClassificationLostFocus = function (logCellTemplate, classificationTextBox) {
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
                //var itemCodeDetails = this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                var itemCodeDetails = GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); //.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
                if (itemCodeDetails == null) {
                    var originCountryCode = null;
                    var originCountryName = null;
                    if (this.Parent.IsCountryPURForItems) {
                        originCountryCode = this.OriginCountryCode;
                        originCountryName = this.OriginCountryName;
                    }
                    //this.Parent.AddEditSupplierInvoiceDUMMYManager.ItemCode_LocalCache.push(new ItemCodeComponent(this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, null));
                    GITITEMCacheService_1.GITITEMCacheService.Instance.AddItemCodeComponent(/*ItemCode_LocalCache.push(*/ new SupplierInvoiceGeneralTabComponent_1.ItemCodeComponent(this.ItemCode, this.ClassificationCode, this.ItemDescription, this.Parent.vendorNumber, originCountryCode, originCountryName, true, null, this.Parent.declarationPM.CustomerCode));
                }
                else {
                    if (itemCodeDetails.ClassificationCode != this.ClassificationCode || itemCodeDetails.ItemDescription != this.ItemDescription) {
                        itemCodeDetails.ClassificationCode = this.ClassificationCode;
                        itemCodeDetails.ItemDescription = this.ItemDescription;
                        itemCodeDetails.VendorNumber = this.Parent.vendorNumber;
                        if (this.Parent.IsCountryPURForItems) {
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
        if (this.Parent.IsChecked) { //private isChecked: boolean = true;/// יש לשים לב ללוגיקות שקיימות במסך העבודה הרגיל. למשל:     בשינוי פרט מכס יש להקפיץ יחידת מידה (במסך הרגיל זה תלוי בסימון V, פה זה ההתנהגות הרגילה).
            if (this.InvoiceQuantityType == null && this.QunatityTypeCode != null) {
                var s = this.QunatityTypeCode.slice(1, this.QunatityTypeCode.length - 1);
                this.InvoiceQuantityType = s;
            }
        }
    };
    SInvoiceItemClassificationLine.prototype.GetQuantityType = function (isChangeInvoiceQuantityType) {
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
    SInvoiceItemClassificationLine.prototype.OnOriginCountryCodeLostFocus = function (logCellTemplate, originCountryCodeLov) {
        if (!this.Parent.IsCountryPURForItems) {
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ItemCode)) {
            var itemCodeDetails = /*this.Parent.AddEditSupplierInvoiceDUMMYManager*/ GITITEMCacheService_1.GITITEMCacheService.Instance.FirstItemCodeComponent(this.ItemCode); //.ItemCode_LocalCache.filter(vm => vm.ItemCode == this.ItemCode)[0];
            if (itemCodeDetails != null) {
                itemCodeDetails.OriginCountryCode = this.OriginCountryCode != null ? this.OriginCountryCode : this.customsCountry != null ? this.customsCountry.Code : null;
                itemCodeDetails.OriginCountryName = this.OriginCountryName != null ? this.OriginCountryName : this.customsCountry != null ? this.customsCountry.LocalName : null;
                itemCodeDetails.IsNew = true;
            }
        }
    };
    SInvoiceItemClassificationLine.prototype.Dispose = function () {
        this.Parent = null;
        this.DataContext = null;
        if (this.QuantityTypeCodeLoaded) {
            this.QuantityTypeCodeLoaded.unsubscribe();
        }
    };
    return SInvoiceItemClassificationLine;
}(BaseComponent_1.BaseComponent));
exports.SInvoiceItemClassificationLine = SInvoiceItemClassificationLine;
//# sourceMappingURL=SInvoiceClassificationTabComponent.js.map