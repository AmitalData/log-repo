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
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ShipmentPayablePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentPayablePM");
var ShipmentReceivablePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentReceivablePM");
var ShipmentAWBPrintOnlyPM_1 = require("../../../../../Shipment/EntityPMs/ShipmentAWBPrintOnlyPM");
var IATACodeListService_1 = require("../../../../../Infrastructure/Services/StandardLists/IATACodeListService");
var CurrencyListService_1 = require("../../../../../Common/Services/StandardLists/CurrencyListService");
var DueTypeListService_1 = require("../../../../../Common/Services/StandardLists/DueTypeListService");
var ChargesTypeListService_1 = require("../../../../../Common/Services/StandardLists/ChargesTypeListService");
var MeasurementListService_1 = require("../../../../../Common/Services/StandardLists/MeasurementListService");
var Tools_2 = require("../../../../../Shipment/Tools");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var OtherChargesTabComponent = /** @class */ (function (_super) {
    __extends(OtherChargesTabComponent, _super);
    function OtherChargesTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.SelectedItem = null;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.AllDueTypes = [];
        _this.AllIATACodes = [];
        _this.AllCurrencies = [];
        _this.AllChargesTypes = [];
        _this.AllMeasurements = [];
        _this.IsEditingEnabled = false;
        _this.IsFillDataWarningVisible = false;
        // Summary Totals
        _this.Total_P = 0;
        _this.Total_C = 0;
        _this.TaxAmount_P = 0;
        _this.TaxAmount_C = 0;
        _this.AgentAmount_P = 0;
        _this.AgentAmount_C = 0;
        _this.CarrierAmount_P = 0;
        _this.CarrierAmount_C = 0;
        _this.ValuationAmount_P = 0;
        _this.ValuationAmount_C = 0;
        _this.ItemsSource = [];
        _this.InitServices();
        return _this;
    }
    OtherChargesTabComponent.prototype.InitServices = function () {
        if (this.myDueTypeListService == null) {
            this.myDueTypeListService = new DueTypeListService_1.DueTypeListService();
        }
        if (this.myIATACodeListService == null) {
            this.myIATACodeListService = new IATACodeListService_1.IATACodeListService();
        }
        if (this.myCurrencyListService == null) {
            this.myCurrencyListService = new CurrencyListService_1.CurrencyListService();
        }
        if (this.myChargesTypeListService == null) {
            this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        }
        if (this.myMeasurementListService == null) {
            this.myMeasurementListService = new MeasurementListService_1.MeasurementListService();
        }
    };
    OtherChargesTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.SetWarningInfo();
        this.BuildItemsSource();
        this.GenerateDefaultList();
        this.SetGenerateButton();
        this.Listen();
        this.SetUIProperties();
    };
    OtherChargesTabComponent.prototype.RefreshTab = function () {
        this.SetGenerateButton();
        this.ComputeTotals();
    };
    OtherChargesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.BuildItemsSource();
                    _this.SetUIProperties();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.BuildItemsSource();
                    _this.SetUIProperties();
                }
            });
        }
    };
    OtherChargesTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.ItemsSource.forEach(function (item) {
            item.SetUIProperties();
        });
    };
    OtherChargesTabComponent.prototype.SetWarningInfo = function () {
        if (!this.Wizard.IsImportWizard) {
            var myResult = false;
            var myCodes = [];
            myCodes.push("EAWB");
            myCodes.push("BUBK");
            if (FeatureLocator_1.FeatureLocator.IsPackageOneOf(myCodes)) {
                myResult = false;
            }
            else {
                if (this.ItemsSource.length == 0) {
                    myResult = true;
                }
            }
            this.IsFillDataWarningVisible = myResult;
        }
    };
    OtherChargesTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_OTC();
    };
    Object.defineProperty(OtherChargesTabComponent.prototype, "TenantZeroAirlineId", {
        get: function () { return this.EntityPM.TenantZeroAirlineId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherChargesTabComponent.prototype, "AsAgreedOtherCharges", {
        get: function () { return this.EntityPM.AsAgreedOtherCharges; },
        set: function (newValue) {
            if (this.EntityPM.AsAgreedOtherCharges != newValue) {
                this.EntityPM.AsAgreedOtherCharges = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherChargesTabComponent.prototype, "OtherPrepaidCollectId", {
        get: function () { return this.EntityPM.OtherPrepaidCollectId; },
        set: function (newValue) {
            if (this.EntityPM.OtherPrepaidCollectId != newValue) {
                this.EntityPM.OtherPrepaidCollectId = newValue;
                Tools_2.ShipmentTool.BuildAWBChargesCodeCode(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherChargesTabComponent.prototype, "IsPrepaidSelected", {
        get: function () {
            var myResult = false;
            if (this.OtherPrepaidCollectId == "P") {
                myResult = true;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    OtherChargesTabComponent.prototype.SetIsPrepaidSelected = function (newValue) {
        if (newValue) {
            this.OtherPrepaidCollectId = "P";
        }
        else {
            this.OtherPrepaidCollectId = "C";
        }
    };
    OtherChargesTabComponent.prototype.ApplyToAllClicked = function () {
        var _this = this;
        this.ItemsSource.forEach(function (item) {
            item.PrepaidCollectId = _this.OtherPrepaidCollectId;
        });
    };
    // Manage Defaults
    OtherChargesTabComponent.prototype.ManageDefaultsClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Manage defaults";
        logWindow.WindowArgs = this.EntityPM.ShipmentLevelCode;
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherCharges/ManageDefaultsComponent");
    };
    OtherChargesTabComponent.prototype.SetGenerateButton = function () {
        var myResult = false;
        if (!FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
            if (this.ItemsSource.length == 0) {
                myResult = true;
            }
        }
        this.IsGeneratingVisible = myResult;
        this.SetWarningInfo();
    };
    OtherChargesTabComponent.prototype.GenerateClicked = function () {
        var _this = this;
        this.myChargesTypeListService.getAll().subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.AllChargesTypes.concat(myResponse.Result);
                    var ChargesTypes = myResponse.Result;
                    if (ChargesTypes != null) {
                        ChargesTypes = ChargesTypes.filter(function (d) { return d.IsAir == true && d.InActive == false && d.ChargesGroupCode != "FRT"; });
                        if (_this.EntityPM.ShipmentLevelCode == "C") {
                            ChargesTypes = ChargesTypes.filter(function (d) { return d.IsAutoDisplayInConsolidation; });
                        }
                        else {
                            ChargesTypes = ChargesTypes.filter(function (d) { return d.IsAutoDisplayInShipment == true; });
                        }
                        ChargesTypes.sort(function (a, b) { return a.ViewOrder - b.ViewOrder; }).forEach(function (item) {
                            var newAWBPrintOnly = new ShipmentAWBPrintOnlyPM_1.ShipmentAWBPrintOnlyPM(_this.EntityPM);
                            newAWBPrintOnly.Tenant = _this.EntityPM.Tenant;
                            newAWBPrintOnly.ShipmentId = _this.EntityPM.Id;
                            newAWBPrintOnly.MeasurementId = item.MeasurementId;
                            newAWBPrintOnly.DueTypeCode = item.DueTypeCode;
                            newAWBPrintOnly.DueTypeName = item.DueTypeName;
                            newAWBPrintOnly.IATACodeId = item.IATACodeId;
                            newAWBPrintOnly.CurrencyId = _this.EntityPM.AWBCurrencyId;
                            newAWBPrintOnly.CurrencyCode = _this.EntityPM.AWBCurrencyCode;
                            newAWBPrintOnly.PrepaidCollectId = _this.EntityPM.OtherPrepaidCollectId;
                            if (newAWBPrintOnly.CurrencyId != null) {
                                var myCurrencyList = _this.AllCurrencies.filter(function (d) { return d.Id == newAWBPrintOnly.CurrencyId; })[0];
                                if (myCurrencyList != null) {
                                    newAWBPrintOnly.CurrencyCode = myCurrencyList.Code;
                                }
                                else {
                                    _this.myCurrencyListService.getSingle(newAWBPrintOnly.CurrencyId).subscribe(function (myResponse) {
                                        if (myResponse != null) {
                                            if (!myResponse.HasError) {
                                                _this.AllCurrencies.push(myResponse.Result);
                                                newAWBPrintOnly.CurrencyCode = myResponse.Result.Code;
                                            }
                                        }
                                    });
                                }
                            }
                            if (newAWBPrintOnly.IATACodeId != null) {
                                var myIATACodeList = _this.AllIATACodes.filter(function (d) { return d.Id == newAWBPrintOnly.IATACodeId; })[0];
                                if (myIATACodeList != null) {
                                    newAWBPrintOnly.IATACodeName = myIATACodeList.Name;
                                }
                                else {
                                    _this.myIATACodeListService.getSingle(newAWBPrintOnly.IATACodeId).subscribe(function (myResponse) {
                                        if (myResponse != null) {
                                            if (!myResponse.HasError) {
                                                _this.AllIATACodes.push(myResponse.Result);
                                                newAWBPrintOnly.IATACodeName = myResponse.Result.Name;
                                            }
                                        }
                                    });
                                }
                            }
                            if (newAWBPrintOnly.MeasurementId != null) {
                                var myMeasurementList = _this.AllMeasurements.filter(function (d) { return d.Id == newAWBPrintOnly.MeasurementId; })[0];
                                if (myMeasurementList != null) {
                                    newAWBPrintOnly.MeasurementCode = myMeasurementList.Code;
                                    switch (newAWBPrintOnly.MeasurementCode) {
                                        case "GRWT": {
                                            newAWBPrintOnly.Quantity = _this.EntityPM.GrossWeight;
                                            break;
                                        }
                                        case "CHWT": {
                                            newAWBPrintOnly.Quantity = _this.EntityPM.ChargeableWeight;
                                            break;
                                        }
                                        case "VOLU": {
                                            newAWBPrintOnly.Quantity = _this.EntityPM.Volume;
                                            break;
                                        }
                                        case "BTEU": {
                                            newAWBPrintOnly.Quantity = _this.EntityPM.TEU;
                                            break;
                                        }
                                        case "FIXD": {
                                            newAWBPrintOnly.Quantity = 1;
                                            break;
                                        }
                                        case "PRVL": {
                                            newAWBPrintOnly.Quantity = _this.EntityPM.ValueOfGoods;
                                            break;
                                        }
                                        case "GWTN": {
                                            newAWBPrintOnly.Quantity = _this.EntityPM.GrossWeightPerTon;
                                            break;
                                        }
                                        case "QTY": {
                                            newAWBPrintOnly.Quantity = _this.EntityPM.NumberOfPackages;
                                            break;
                                        }
                                        default: {
                                            break;
                                        }
                                    }
                                }
                                else {
                                    _this.myMeasurementListService.getSingle(newAWBPrintOnly.MeasurementId).subscribe(function (myResponse) {
                                        if (myResponse != null) {
                                            if (!myResponse.HasError) {
                                                _this.AllMeasurements.push(myResponse.Result);
                                                newAWBPrintOnly.MeasurementCode = myResponse.Result.Code;
                                                switch (newAWBPrintOnly.MeasurementCode) {
                                                    case "GRWT": {
                                                        newAWBPrintOnly.Quantity = _this.EntityPM.GrossWeight;
                                                        break;
                                                    }
                                                    case "CHWT": {
                                                        newAWBPrintOnly.Quantity = _this.EntityPM.ChargeableWeight;
                                                        break;
                                                    }
                                                    case "VOLU": {
                                                        newAWBPrintOnly.Quantity = _this.EntityPM.Volume;
                                                        break;
                                                    }
                                                    case "BTEU": {
                                                        newAWBPrintOnly.Quantity = _this.EntityPM.TEU;
                                                        break;
                                                    }
                                                    case "FIXD": {
                                                        newAWBPrintOnly.Quantity = 1;
                                                        break;
                                                    }
                                                    case "PRVL": {
                                                        newAWBPrintOnly.Quantity = _this.EntityPM.ValueOfGoods;
                                                        break;
                                                    }
                                                    case "GWTN": {
                                                        newAWBPrintOnly.Quantity = _this.EntityPM.GrossWeightPerTon;
                                                        break;
                                                    }
                                                    case "QTY": {
                                                        newAWBPrintOnly.Quantity = _this.EntityPM.NumberOfPackages;
                                                        break;
                                                    }
                                                    default: {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                            _this.EntityPM.ShipmentAWBPrintOnlies.push(newAWBPrintOnly);
                        });
                        _this.BuildItemsSource();
                    }
                }
            }
        });
    };
    OtherChargesTabComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.ItemsSource = [];
        this.EntityPM.ShipmentPayables.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).forEach(function (item) {
            _this.ItemsSource.push(new AWBWizardOtherChargeItem(item, _this, false));
        });
        this.EntityPM.ShipmentReceivables.filter(function (f) { return f.ChargesGroupCode != "FRT"; }).forEach(function (item) {
            _this.ItemsSource.push(new AWBWizardOtherChargeItem(item, _this, false));
        });
        this.EntityPM.ShipmentAWBPrintOnlies.forEach(function (item) {
            _this.ItemsSource.push(new AWBWizardOtherChargeItem(item, _this, false));
        });
        this.ComputeTotals();
        this.SetGenerateButton();
    };
    OtherChargesTabComponent.prototype.GenerateDefaultList = function () {
        var _this = this;
        if (FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
            if (this.ItemsSource.length < 5) {
                var list = [];
                for (var i = this.ItemsSource.length; i < 5; i++) {
                    var item = new ShipmentAWBPrintOnlyPM_1.ShipmentAWBPrintOnlyPM(null);
                    item.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    item.ShipmentId = this.EntityPM.Id;
                    item.CurrencyId = this.EntityPM.AWBCurrencyId;
                    item.CurrencyCode = this.EntityPM.AWBCurrencyCode;
                    item.PrepaidCollectId = this.EntityPM.OtherPrepaidCollectId;
                    item.ExchangeRate = this.Wizard.GetCurrencyRate(this.EntityPM.AWBCurrencyId);
                    list.push(item);
                }
                list.forEach(function (item) {
                    _this.ItemsSource.push(new AWBWizardOtherChargeItem(item, _this, false));
                });
                this.SetGenerateButton();
            }
        }
    };
    Object.defineProperty(OtherChargesTabComponent.prototype, "AWBFreightAmountPrepaid", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBFreightAmountPrepaid) ? 0 : this.EntityPM.AWBFreightAmountPrepaid; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherChargesTabComponent.prototype, "AWBFreightAmountCollect", {
        get: function () { return Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.AWBFreightAmountCollect) ? 0 : this.EntityPM.AWBFreightAmountCollect; },
        enumerable: true,
        configurable: true
    });
    OtherChargesTabComponent.prototype.ComputeTotals = function () {
        var _this = this;
        this.Total_P = 0;
        this.Total_C = 0;
        this.TaxAmount_P = 0;
        this.TaxAmount_C = 0;
        this.AgentAmount_P = 0;
        this.AgentAmount_C = 0;
        this.CarrierAmount_P = 0;
        this.CarrierAmount_C = 0;
        this.ValuationAmount_P = 0;
        this.ValuationAmount_C = 0;
        this.ItemsSource.filter(function (d) { return d.AWBPrint && d.CurrencyId == _this.EntityPM.AWBCurrencyId; }).forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.Amount)) {
                if (item.PrepaidCollectId == "P") {
                    _this.Total_P += item.Amount;
                    switch (item.DueTypeCode) {
                        case "TX": {
                            _this.TaxAmount_P += item.Amount;
                            break;
                        }
                        case "AG": {
                            _this.AgentAmount_P += item.Amount;
                            break;
                        }
                        case "CA": {
                            _this.CarrierAmount_P += item.Amount;
                            break;
                        }
                        case "VL": {
                            _this.ValuationAmount_P += item.Amount;
                            break;
                        }
                    }
                }
                else if (item.PrepaidCollectId == "C") {
                    _this.Total_C += item.Amount;
                    switch (item.DueTypeCode) {
                        case "TX": {
                            _this.TaxAmount_C += item.Amount;
                            break;
                        }
                        case "AG": {
                            _this.AgentAmount_C += item.Amount;
                            break;
                        }
                        case "CA": {
                            _this.CarrierAmount_C += item.Amount;
                            break;
                        }
                        case "VL": {
                            _this.ValuationAmount_C += item.Amount;
                            break;
                        }
                    }
                }
            }
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AWBFreightAmountPrepaid)) {
            this.Total_P += this.AWBFreightAmountPrepaid;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AWBFreightAmountCollect)) {
            this.Total_C += this.AWBFreightAmountCollect;
        }
        this.FireWizardEvent();
        this.SetWarningInfo();
        this.SetGenerateButton();
    };
    // Commands
    OtherChargesTabComponent.prototype.Add = function () {
        var itemPM = new ShipmentAWBPrintOnlyPM_1.ShipmentAWBPrintOnlyPM(null);
        itemPM.ShipmentId = this.EntityPM.Id;
        itemPM.Tenant = this.EntityPM.Tenant;
        itemPM.CurrencyId = this.EntityPM.AWBCurrencyId;
        itemPM.CurrencyCode = this.EntityPM.AWBCurrencyCode;
        itemPM.PrepaidCollectId = this.EntityPM.OtherPrepaidCollectId;
        itemPM.ExchangeRate = this.Wizard.GetCurrencyRate(this.EntityPM.AWBCurrencyId);
        var itemViewModel = new AWBWizardOtherChargeItem(itemPM, this, true);
        itemViewModel.OnCurrencyIdChanged();
        var title = "Add " + itemViewModel.TypeName.replace("AWB Print Only", "Charge");
        this.RunWindow(itemViewModel, title);
    };
    OtherChargesTabComponent.prototype.Edit = function (itemViewModel) {
        var title = "Edit " + itemViewModel.TypeName.replace("AWB Print Only", "Charge");
        this.RunWindow(itemViewModel, title);
    };
    OtherChargesTabComponent.prototype.Delete = function (item) {
        // Dont Show Confirm: Task 21883
        if (item != null) {
            if (item.PayablePM != null) {
                this.EntityPM.RemovePayable(item.PayablePM);
            }
            else if (item.ReceivablePM != null) {
                this.EntityPM.RemoveReceivable(item.ReceivablePM);
            }
            else if (item.AWBPrintOnlyPM != null) {
                this.EntityPM.RemoveAWBPrintOnly(item.AWBPrintOnlyPM);
            }
            var index = this.ItemsSource.indexOf(item);
            if (index > -1) {
                this.ItemsSource.splice(index, 1);
            }
            this.ComputeTotals();
            this.SetGenerateButton();
        }
    };
    OtherChargesTabComponent.prototype.RunWindow = function (item, windowTitle) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.DataContext = item;
        logWindow.Show("./ShipmentModules/ShipmentAWB/Components/AWBWizard/OtherCharges/AddEditOtherChargeComponent");
    };
    OtherChargesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'OtherChargesTabComponent',
            templateUrl: './OtherChargesTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], OtherChargesTabComponent);
    return OtherChargesTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OtherChargesTabComponent = OtherChargesTabComponent;
var AWBWizardOtherChargeItem = /** @class */ (function (_super) {
    __extends(AWBWizardOtherChargeItem, _super);
    function AWBWizardOtherChargeItem(item, fatherComponent, isnew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.DataContext = _this;
        _this.IsEditingItemEnabled = false;
        _this.IsChargesTypeVisible = false;
        _this.IsAWBPrintIconVisible = false;
        _this.IsAWBPrintCheckBoxVisible = false;
        _this.IsAWBPrintCheckBoxEnabled = false;
        if (item != null) {
            _this.IsNewEntity = isnew;
            _this.ShipmentPM = fatherComponent.EntityPM;
            if (item instanceof ShipmentPayablePM_1.ShipmentPayablePM) {
                _this.PayablePM = item;
                _this.TypeName = "Payable";
                _this.TypeImageSource = "./Images/Letter_P.png";
                _this.AmountFieldName = "TotalAmount";
                _this.RateFieldName = "Rate";
                _this.ObjectTableName = "ShipmentPayable";
            }
            else if (item instanceof ShipmentReceivablePM_1.ShipmentReceivablePM) {
                _this.ReceivablePM = item;
                _this.TypeName = "Receivable";
                _this.TypeImageSource = "./Images/Letter_R.png";
                _this.AmountFieldName = "TotalAmount";
                _this.RateFieldName = "Rate";
                _this.ObjectTableName = "ShipmentReceivable";
            }
            else if (item instanceof ShipmentAWBPrintOnlyPM_1.ShipmentAWBPrintOnlyPM) {
                _this.AWBPrintOnlyPM = item;
                _this.TypeName = "AWB Print Only";
                _this.TypeImageSource = "./Images/Letter_A.png";
                _this.AmountFieldName = "Amount";
                _this.RateFieldName = "ExchangeRate";
                _this.ObjectTableName = "ShipmentAWBPrintOnly";
            }
            _this.fatherComponent._entityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (response) {
                _this.SetUIProperties();
                _this.BuildQueryFilters();
            });
        }
        return _this;
    }
    AWBWizardOtherChargeItem.prototype.SetUIProperties = function () {
        this.SetCheckBox();
        this.SetEditingItemEnabled();
        this.ValidateAWBPrintRequiredFields();
        this.UIProperties.SetEnabled("ChargesTypeId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled(this.AmountFieldName, this.ObjectTableName, false);
        this.UIProperties.SetEnabled("IATACodeId", this.ObjectTableName, this.IsEditingItemEnabled);
        var isEditingFieldEnabled = this.IsEditingItemEnabled;
        if (isEditingFieldEnabled) {
            if (this.AWBPrintOnlyPM != null) {
                if (FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.IATACodeId)) {
                        isEditingFieldEnabled = false;
                    }
                }
            }
        }
        this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("MeasurementId", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("Quantity", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("UnitPrice", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("PrepaidCollectId", this.ObjectTableName, isEditingFieldEnabled);
        this.UIProperties.SetEnabled("DueTypeCode", this.ObjectTableName, isEditingFieldEnabled);
    };
    AWBWizardOtherChargeItem.prototype.SetEditingItemEnabled = function () {
        var isEditingItemEnabled = this.fatherComponent.IsEditingEnabled;
        if (isEditingItemEnabled) {
            if (this.ReceivablePM != null) {
                if (this.ReceivablePM.ShipmentReceivableLineStatusCode == "ACCT" || this.ReceivablePM.ShipmentReceivableLineStatusCode == "DRFT") {
                    isEditingItemEnabled = false;
                }
                if (this.ReceivablePM.IsChargeBySteps) {
                    isEditingItemEnabled = false;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ReceivablePM.ShipmentReceivableParentId)) {
                    isEditingItemEnabled = false;
                }
            }
            else if (this.PayablePM != null) {
                if (this.PayablePM.ShipmentPayableLineStatusCode == "ACCT" || this.PayablePM.ShipmentPayableLineStatusCode == "PACC") {
                    isEditingItemEnabled = false;
                }
                if (this.PayablePM.IsChargeBySteps) {
                    isEditingItemEnabled = false;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.PayablePM.ShipmentPayableParentId)) {
                    isEditingItemEnabled = false;
                }
            }
        }
        this.IsEditingItemEnabled = isEditingItemEnabled;
    };
    AWBWizardOtherChargeItem.prototype.ValidateAWBPrintRequiredFields = function () {
        var isChargesTypeVisible = true;
        if (this.AWBPrintOnlyPM != null) {
            isChargesTypeVisible = false;
            this.UIProperties.SetVisibility("ChargesTypeId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("IATACodeId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.IATACodeId));
            this.UIProperties.SetRequired("PrepaidCollectId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.PrepaidCollectId));
        }
        this.IsChargesTypeVisible = isChargesTypeVisible;
    };
    AWBWizardOtherChargeItem.prototype.BuildQueryFilters = function () {
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "AWBPrint", {
        get: function () {
            var myResult = false;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.AWBPrint;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.AWBPrint;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = true;
                if (Tools_1.AppTool.IsNullOrEmpty(this.IATACodeId) || Tools_1.AppTool.IsNullOrEmpty(this.PrepaidCollectId) || Tools_1.AppTool.IsNullOrEmpty(this.DueTypeCode) || this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
                    myResult = false;
                }
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.AWBPrint != newValue) {
                    this.PayablePM.AWBPrint = newValue;
                }
                this.fatherComponent.ComputeTotals();
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.AWBPrint != newValue) {
                    this.ReceivablePM.AWBPrint = newValue;
                }
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.SetAWBPrintField = function () {
        var myResult = true;
        if (Tools_1.AppTool.IsNullOrEmpty(this.IATACodeId) || Tools_1.AppTool.IsNullOrEmpty(this.PrepaidCollectId) || Tools_1.AppTool.IsNullOrEmpty(this.DueTypeCode) || this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
            myResult = false;
        }
        this.AWBPrint = myResult;
        this.SetCheckBox();
    };
    AWBWizardOtherChargeItem.prototype.SetCheckBox = function () {
        var isMatched = true;
        var error = "";
        if (this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
            isMatched = false;
            error = "Currency is not equal to the AWB currency";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.IATACodeId)) {
            isMatched = false;
            if (Tools_1.AppTool.IsNullOrEmpty(error)) {
                error = "IATA code field is required";
            }
            else {
                error += "\nIATA code field is required";
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DueTypeCode)) {
            isMatched = false;
            if (Tools_1.AppTool.IsNullOrEmpty(error)) {
                error = "Due type field is required";
            }
            else {
                error += "\nDue type field is required";
            }
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.PrepaidCollectId)) {
            isMatched = false;
            if (Tools_1.AppTool.IsNullOrEmpty(error)) {
                error = "P / C field is required";
            }
            else {
                error += "\nP / C field is required";
            }
        }
        this.NotMatchedToolTip = error;
        this.IsAWBPrintIconVisible = !isMatched;
        this.IsAWBPrintCheckBoxVisible = isMatched;
        this.IsAWBPrintCheckBoxEnabled = this.AWBPrintOnlyPM == null ? true : false;
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "IATACodeId", {
        // IATACodeId
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.IATACodeId;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.IATACodeId;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.IATACodeId;
            }
            return myResult;
        },
        set: function (newValue) {
            var _this = this;
            var isExecuting = false;
            if (this.PayablePM != null) {
                if (this.PayablePM.IATACodeId != newValue) {
                    this.PayablePM.IATACodeId = newValue;
                    isExecuting = true;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.IATACodeId != newValue) {
                    this.ReceivablePM.IATACodeId = newValue;
                    isExecuting = true;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.IATACodeId != newValue) {
                    this.AWBPrintOnlyPM.IATACodeId = newValue;
                    isExecuting = true;
                    if (FeatureLocator_1.FeatureLocator.IsPackage_EAWB()) {
                        var itemIndex = this.ShipmentPM.ShipmentAWBPrintOnlies.indexOf(this.AWBPrintOnlyPM);
                        if (newValue == null) {
                            this.MeasurementId = null;
                            this.DueTypeCode = null;
                            this.UnitPrice = null;
                            this.Quantity = null;
                            if (!this.IsWindowMode) {
                                if (itemIndex > -1) {
                                    this.ShipmentPM.RemoveAWBPrintOnly(this.AWBPrintOnlyPM);
                                }
                            }
                        }
                        else {
                            var list = this.fatherComponent.AllIATACodes.filter(function (d) { return d.Id == _this.AWBPrintOnlyPM.IATACodeId; })[0];
                            if (list != null) {
                                this.AWBPrintOnlyPM.IATACodeName = list.Name;
                            }
                            else {
                                this.fatherComponent.myIATACodeListService.getSingle(this.AWBPrintOnlyPM.IATACodeId).subscribe(function (myResponse) {
                                    if (myResponse != null) {
                                        if (!myResponse.HasError) {
                                            _this.fatherComponent.AllIATACodes.push(myResponse.Result);
                                            _this.AWBPrintOnlyPM.IATACodeName = myResponse.Result.Name;
                                        }
                                    }
                                });
                            }
                            if (!this.IsWindowMode) {
                                if (itemIndex == -1) {
                                    this.ShipmentPM.AddAWBPrintOnly(this.AWBPrintOnlyPM);
                                }
                            }
                        }
                    }
                    this.ValidateAWBPrintRequiredFields();
                }
            }
            if (isExecuting) {
                this.OnIATACodeIdChanged();
                this.SetAWBPrintField();
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "IATACodeName", {
        get: function () {
            var myResult = null;
            if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.IATACodeName;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.AWBPrintOnlyPM != null) {
                this.AWBPrintOnlyPM.IATACodeName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.OnIATACodeIdChanged = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.IATACodeId)) {
            this.IATACodeName = null;
        }
        else {
            var list = this.fatherComponent.AllIATACodes.filter(function (d) { return d.Id == _this.IATACodeId; })[0];
            if (list != null) {
                this.IATACodeName = list.Name;
            }
            else {
                this.fatherComponent.myIATACodeListService.getSingle(this.IATACodeId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.fatherComponent.AllIATACodes.push(myResponse.Result);
                            _this.IATACodeName = myResponse.Result.Name;
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "ChargesTypeId", {
        // ChargesTypeId
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.ChargesTypeId;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.ChargesTypeId;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.ChargesTypeId != newValue) {
                    this.PayablePM.ChargesTypeId = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.ChargesTypeId != newValue) {
                    this.ReceivablePM.ChargesTypeId = newValue;
                }
            }
            this.OnChargesTypeIdChanged();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "ChargesTypeCode", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.ChargesTypeCode;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.ChargesTypeCode;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.ChargesTypeCode != newValue) {
                    this.PayablePM.ChargesTypeCode = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.ChargesTypeCode != newValue) {
                    this.ReceivablePM.ChargesTypeCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "ChargesTypeName", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.ChargesTypeName;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.ChargesTypeName;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.ChargesTypeName != newValue) {
                    this.PayablePM.ChargesTypeName = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.ChargesTypeName != newValue) {
                    this.ReceivablePM.ChargesTypeName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "ChargesGroupCode", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.ChargesGroupCode;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.ChargesGroupCode;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.ChargesGroupCode != newValue) {
                    this.PayablePM.ChargesGroupCode = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.ChargesGroupCode != newValue) {
                    this.ReceivablePM.ChargesGroupCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.OnChargesTypeIdChanged = function () {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ChargesTypeId)) {
            this.ChargesTypeCode = null;
            this.ChargesTypeName = null;
            this.DueTypeCode = null;
            this.DueTypeName = null;
            this.PrepaidCollectId = null;
            this.CurrencyId = null;
            this.ChargesGroupCode = null;
            this.IATACodeId = null;
            this.MeasurementId = null;
        }
        else {
            var list = this.fatherComponent.AllChargesTypes.filter(function (d) { return d.Id == _this.ChargesTypeId; })[0];
            if (list != null) {
                this.ChargesTypeCode = list.Code;
                this.ChargesTypeName = list.EnglishName;
                this.DueTypeCode = list.DueTypeCode;
                this.DueTypeName = list.DueTypeName;
                this.ChargesGroupCode = list.ChargesGroupCode;
                this.IATACodeId = list.IATACodeId;
                this.MeasurementId = list.MeasurementId;
                this.SetPrepaidCollectId();
                if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                    this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                }
                else {
                    this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                }
            }
            else {
                this.fatherComponent.myChargesTypeListService.getSingle(this.ChargesTypeId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            _this.fatherComponent.AllChargesTypes.push(list);
                            _this.ChargesTypeCode = list.Code;
                            _this.ChargesTypeName = list.EnglishName;
                            _this.DueTypeCode = list.DueTypeCode;
                            _this.DueTypeName = list.DueTypeName;
                            _this.ChargesGroupCode = list.ChargesGroupCode;
                            _this.IATACodeId = list.IATACodeId;
                            _this.MeasurementId = list.MeasurementId;
                            _this.SetPrepaidCollectId();
                            if (list.ChargesGroupCode == "FRT" || list.ChargesGroupCode == "SCH") {
                                _this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
                            }
                            else {
                                _this.CurrencyId = SessionLocator_1.SessionLocator.TenantPM.OtherChargesCurrencyId;
                            }
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "PrepaidCollectId", {
        // PrepaidCollectId
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.PrepaidCollectId;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.PrepaidCollectId;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.PrepaidCollectId;
            }
            return myResult;
        },
        set: function (newValue) {
            var isExecuting = false;
            if (this.PayablePM != null) {
                if (this.PayablePM.PrepaidCollectId != newValue) {
                    this.PayablePM.PrepaidCollectId = newValue;
                    isExecuting = true;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.PrepaidCollectId != newValue) {
                    this.ReceivablePM.PrepaidCollectId = newValue;
                    isExecuting = true;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.PrepaidCollectId != newValue) {
                    this.AWBPrintOnlyPM.PrepaidCollectId = newValue;
                    isExecuting = true;
                    this.ValidateAWBPrintRequiredFields();
                }
            }
            if (isExecuting) {
                this.SetAWBPrintField();
                this.SetUIProperties();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.SetPrepaidCollectId = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.PrepaidCollectId)) {
            if (this.ChargesGroupCode == "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.FreightPrepaidCollectId;
            }
            else if (this.ChargesGroupCode != "FRT") {
                this.PrepaidCollectId = this.ShipmentPM.OtherPrepaidCollectId;
            }
        }
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "DueTypeCode", {
        // DueTypeCode
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.DueTypeCode;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.DueTypeCode;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.DueTypeCode;
            }
            return myResult;
        },
        set: function (newValue) {
            var isExecuting = false;
            if (this.PayablePM != null) {
                if (this.PayablePM.DueTypeCode != newValue) {
                    this.PayablePM.DueTypeCode = newValue;
                    isExecuting = true;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.DueTypeCode != newValue) {
                    this.ReceivablePM.DueTypeCode = newValue;
                    isExecuting = true;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.DueTypeCode != newValue) {
                    this.AWBPrintOnlyPM.DueTypeCode = newValue;
                    isExecuting = true;
                }
            }
            if (isExecuting) {
                this.SetAWBPrintField();
                this.SetUIProperties();
                this.OnDueTypeCodeChanged();
                this.fatherComponent.ComputeTotals();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "DueTypeName", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.DueTypeName;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.DueTypeName;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.DueTypeName;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.DueTypeName != newValue) {
                    this.PayablePM.DueTypeName = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.DueTypeName != newValue) {
                    this.ReceivablePM.DueTypeName = newValue;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.DueTypeName != newValue) {
                    this.AWBPrintOnlyPM.DueTypeName = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.OnDueTypeCodeChanged = function () {
        var _this = this;
        if (this.DueTypeCode == null) {
            this.DueTypeName = null;
        }
        else {
            var list = this.fatherComponent.AllDueTypes.filter(function (d) { return d.Code == _this.DueTypeCode; })[0];
            if (list != null) {
                this.DueTypeName = list.Name;
            }
            else {
                this.fatherComponent.myDueTypeListService.getSingle(this.DueTypeCode).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.fatherComponent.AllDueTypes.push(myResponse.Result);
                            _this.DueTypeName = myResponse.Result.Name;
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "CurrencyId", {
        // CurrencyId
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.CurrencyId;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.CurrencyId;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.CurrencyId;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.CurrencyId != newValue) {
                    this.PayablePM.CurrencyId = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.CurrencyId != newValue) {
                    this.ReceivablePM.CurrencyId = newValue;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.CurrencyId != newValue) {
                    this.AWBPrintOnlyPM.CurrencyId = newValue;
                }
            }
            this.SetAWBPrintField();
            this.OnCurrencyIdChanged();
            this.fatherComponent.ComputeTotals();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "CurrencyCode", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.CurrencyCode;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.CurrencyCode;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.CurrencyCode;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.CurrencyCode != newValue) {
                    this.PayablePM.CurrencyCode = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.CurrencyCode != newValue) {
                    this.ReceivablePM.CurrencyCode = newValue;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.CurrencyCode != newValue) {
                    this.AWBPrintOnlyPM.CurrencyCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "ExchangeRate", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.Rate;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.Rate;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.ExchangeRate;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.Rate != newValue) {
                    this.PayablePM.Rate = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.Rate != newValue) {
                    this.ReceivablePM.Rate = newValue;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.ExchangeRate != newValue) {
                    this.AWBPrintOnlyPM.ExchangeRate = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.OnCurrencyIdChanged = function () {
        var _this = this;
        this.ExchangeRate = this.fatherComponent.Wizard.GetCurrencyRate(this.CurrencyId);
        if (this.CurrencyId == null) {
            this.CurrencyCode = null;
        }
        else {
            var list = this.fatherComponent.AllCurrencies.filter(function (d) { return d.Id == _this.CurrencyId; })[0];
            if (list != null) {
                this.CurrencyCode = list.Code;
            }
            else {
                this.fatherComponent.myCurrencyListService.getSingle(this.CurrencyId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.fatherComponent.AllCurrencies.push(myResponse.Result);
                            _this.CurrencyCode = myResponse.Result.Code;
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "MeasurementId", {
        // MeasurementId
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.MeasurementId;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.MeasurementId;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.MeasurementId;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.MeasurementId != newValue) {
                    this.PayablePM.MeasurementId = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.MeasurementId != newValue) {
                    this.ReceivablePM.MeasurementId = newValue;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.MeasurementId != newValue) {
                    this.AWBPrintOnlyPM.MeasurementId = newValue;
                }
            }
            this.OnMeasurementIdChanged();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "MeasurementCode", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.MeasurementCode;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.MeasurementCode;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.MeasurementCode;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.MeasurementCode != newValue) {
                    this.PayablePM.MeasurementCode = newValue;
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.MeasurementCode != newValue) {
                    this.ReceivablePM.MeasurementCode = newValue;
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.MeasurementCode != newValue) {
                    this.AWBPrintOnlyPM.MeasurementCode = newValue;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.OnMeasurementIdChanged = function () {
        var _this = this;
        if (this.MeasurementId == null) {
            this.MeasurementCode = null;
        }
        else {
            var list = this.fatherComponent.AllMeasurements.filter(function (d) { return d.Id == _this.MeasurementId; })[0];
            if (list != null) {
                this.MeasurementCode = list.Code;
                switch (this.MeasurementCode) {
                    case "GRWT": {
                        this.Quantity = this.ShipmentPM.GrossWeight;
                        break;
                    }
                    case "CHWT": {
                        this.Quantity = this.ShipmentPM.ChargeableWeight;
                        break;
                    }
                    case "VOLU": {
                        this.Quantity = this.ShipmentPM.Volume;
                        break;
                    }
                    case "BTEU": {
                        this.Quantity = this.ShipmentPM.TEU;
                        break;
                    }
                    case "FIXD": {
                        this.Quantity = 1;
                        break;
                    }
                    case "PRVL": {
                        this.Quantity = this.ShipmentPM.ValueOfGoods;
                        break;
                    }
                    case "GWTN": {
                        this.Quantity = this.ShipmentPM.GrossWeightPerTon;
                        break;
                    }
                    case "QTY": {
                        this.Quantity = this.ShipmentPM.NumberOfPackages;
                        break;
                    }
                    default: {
                        break;
                    }
                }
            }
            else {
                this.fatherComponent.myMeasurementListService.getSingle(this.MeasurementId).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (!myResponse.HasError) {
                            _this.fatherComponent.AllMeasurements.push(myResponse.Result);
                            _this.MeasurementCode = myResponse.Result.Code;
                            switch (_this.MeasurementCode) {
                                case "GRWT": {
                                    _this.Quantity = _this.ShipmentPM.GrossWeight;
                                    break;
                                }
                                case "CHWT": {
                                    _this.Quantity = _this.ShipmentPM.ChargeableWeight;
                                    break;
                                }
                                case "VOLU": {
                                    _this.Quantity = _this.ShipmentPM.Volume;
                                    break;
                                }
                                case "BTEU": {
                                    _this.Quantity = _this.ShipmentPM.TEU;
                                    break;
                                }
                                case "FIXD": {
                                    _this.Quantity = 1;
                                    break;
                                }
                                case "PRVL": {
                                    _this.Quantity = _this.ShipmentPM.ValueOfGoods;
                                    break;
                                }
                                case "GWTN": {
                                    _this.Quantity = _this.ShipmentPM.GrossWeightPerTon;
                                    break;
                                }
                                case "QTY": {
                                    _this.Quantity = _this.ShipmentPM.NumberOfPackages;
                                    break;
                                }
                                default: {
                                    break;
                                }
                            }
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "Name", {
        get: function () {
            var myResult = "";
            if (this.AWBPrintOnlyPM != null) {
                myResult = this.IATACodeName;
            }
            else {
                myResult = this.ChargesTypeName;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "Quantity", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.Quantity;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.Quantity;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.Quantity;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.Quantity != newValue) {
                    this.PayablePM.Quantity = Tools_1.AppTool.Round(newValue, 3);
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.Quantity != newValue) {
                    this.ReceivablePM.Quantity = Tools_1.AppTool.Round(newValue, 3);
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.Quantity != newValue) {
                    this.AWBPrintOnlyPM.Quantity = Tools_1.AppTool.Round(newValue, 3);
                }
            }
            this.OnQuantityChanged();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "UnitPrice", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.UnitPrice;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.UnitPrice;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.UnitPrice;
            }
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.UnitPrice != newValue) {
                    this.PayablePM.UnitPrice = Tools_1.AppTool.Round(newValue, 3);
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.UnitPrice != newValue) {
                    this.ReceivablePM.UnitPrice = Tools_1.AppTool.Round(newValue, 3);
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.UnitPrice != newValue) {
                    this.AWBPrintOnlyPM.UnitPrice = Tools_1.AppTool.Round(newValue, 3);
                }
            }
            this.ComputeAmount();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "TotalAmount", {
        get: function () {
            return this.Amount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "Amount", {
        get: function () {
            var myResult = null;
            if (this.PayablePM != null) {
                myResult = this.PayablePM.ExpectedAmount;
            }
            else if (this.ReceivablePM != null) {
                myResult = this.ReceivablePM.TotalAmount;
            }
            else if (this.AWBPrintOnlyPM != null) {
                myResult = this.AWBPrintOnlyPM.Amount;
            }
            //if (AppTool.IsNullOrEmpty(myResult)) {
            //    myResult = 0;
            //}
            return myResult;
        },
        set: function (newValue) {
            if (this.PayablePM != null) {
                if (this.PayablePM.ExpectedAmount != newValue) {
                    this.PayablePM.ExpectedAmount = Tools_1.AppTool.Round(newValue, 2);
                }
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.TotalAmount != newValue) {
                    this.ReceivablePM.TotalAmount = Tools_1.AppTool.Round(newValue, 2);
                }
            }
            else if (this.AWBPrintOnlyPM != null) {
                if (this.AWBPrintOnlyPM.Amount != newValue) {
                    this.AWBPrintOnlyPM.Amount = Tools_1.AppTool.Round(newValue, 2);
                }
            }
            this.fatherComponent.ComputeTotals();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AWBWizardOtherChargeItem.prototype, "AmountColor", {
        get: function () {
            var myResult = Tools_1.FontTool.Black;
            if (this.CurrencyId != this.ShipmentPM.AWBCurrencyId) {
                myResult = Tools_1.FontTool.Red;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    AWBWizardOtherChargeItem.prototype.SetLineStatus = function () {
        if (this.PayablePM != null) {
            Tools_2.ShipmentTool.SetPayableLineStatus(this.PayablePM);
        }
        else if (this.ReceivablePM != null) {
            Tools_2.ShipmentTool.SetReceivableLineStatus(this.ReceivablePM);
        }
    };
    AWBWizardOtherChargeItem.prototype.ComputeAmount = function () {
        var amount = this.Quantity * this.UnitPrice;
        var _AmountLocal = null;
        var _AmountProfit = null;
        if (this.AWBPrintOnlyPM != null) {
            this.Amount = amount;
        }
        else {
            this.SetLineStatus();
            if (this.PayablePM != null) {
                if (this.PayablePM.QuoteCostMinAmount != null || this.PayablePM.QuoteCostMaxAmount != null) {
                    if (amount == null) {
                        amount = this.PayablePM.QuoteCostMinAmount;
                    }
                    if (amount != null) {
                        if (amount < this.PayablePM.QuoteCostMinAmount) {
                            amount = this.PayablePM.QuoteCostMinAmount;
                        }
                        else if (amount > this.PayablePM.QuoteCostMaxAmount) {
                            amount = this.PayablePM.QuoteCostMaxAmount;
                        }
                    }
                }
                /* From Tariff */
                if (this.PayablePM.MinAmount != null || this.PayablePM.MaxAmount != null) {
                    if (amount != null) {
                        if (amount < this.PayablePM.MinAmount) {
                            amount = this.PayablePM.MinAmount;
                        }
                        else if (amount > this.PayablePM.MaxAmount) {
                            amount = this.PayablePM.MaxAmount;
                        }
                    }
                }
                _AmountLocal = amount * this.ExchangeRate;
                if (this.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
                    _AmountProfit = amount;
                }
                else {
                    _AmountProfit = _AmountLocal / this.PayablePM.ProfitCurrencyExchangeRate;
                }
                this.Amount = amount;
                this.PayablePM.ExpectedAmountLocal = _AmountLocal;
                this.PayablePM.ExpectedAmountInProfitCurrency = _AmountProfit;
                this.ComputePayableOtherAmounts();
                Tools_2.ShipmentTool.ComputeTotals(this.ShipmentPM);
            }
            else if (this.ReceivablePM != null) {
                if (this.ReceivablePM.QuoteSaleMinAmount != null || this.ReceivablePM.QuoteSaleMaxAmount != null) {
                    if (amount == null) {
                        amount = this.ReceivablePM.QuoteSaleMinAmount;
                    }
                    if (amount != null) {
                        if (amount < this.ReceivablePM.QuoteSaleMinAmount) {
                            amount = this.ReceivablePM.QuoteSaleMinAmount;
                        }
                        else if (amount > this.ReceivablePM.QuoteSaleMaxAmount) {
                            amount = this.ReceivablePM.QuoteSaleMaxAmount;
                        }
                    }
                }
                _AmountLocal = amount * this.ExchangeRate;
                if (this.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
                    _AmountProfit = amount;
                }
                else {
                    _AmountProfit = _AmountLocal / this.ReceivablePM.ProfitCurrencyExchangeRate;
                }
                this.Amount = amount;
                this.ReceivablePM.TotalAmountLocal = _AmountLocal;
                this.ReceivablePM.AmountInProfitCurrency = _AmountProfit;
                Tools_2.ShipmentTool.ComputeTotals(this.ShipmentPM);
            }
        }
    };
    AWBWizardOtherChargeItem.prototype.OnQuantityChanged = function () {
        var baseQuote = null;
        if (this.PayablePM != null) {
            if (this.PayablePM.IsChargeBySteps) {
                Tools_2.ShipmentTool.SetPayableUnitPriceBySteps(this.PayablePM, baseQuote);
            }
        }
        else if (this.ReceivablePM != null) {
            if (this.ReceivablePM.IsChargeBySteps) {
                Tools_2.ShipmentTool.SetReceivableUnitPriceBySteps(this.ReceivablePM, baseQuote);
            }
        }
        this.ComputeAmount();
    };
    AWBWizardOtherChargeItem.prototype.ComputePayableOtherAmounts = function () {
        if (this.PayablePM != null) {
            if (this.PayablePM.ShipmentPayableLineStatusCode == "EMPT" || this.PayablePM.ShipmentPayableLineStatusCode == "OAMT") {
                this.PayablePM.AccountedAmount = 0;
                this.PayablePM.CorrectionAmount = 0;
                this.PayablePM.OpenAmount = this.PayablePM.ExpectedAmount;
            }
            else {
                var expe = this.PayablePM.ExpectedAmount == null ? 0 : this.PayablePM.ExpectedAmount;
                var acct = this.PayablePM.AccountedAmount == null ? 0 : this.PayablePM.AccountedAmount;
                var open = this.PayablePM.OpenAmount == null ? 0 : this.PayablePM.OpenAmount;
                this.PayablePM.CorrectionAmount = expe - acct - open;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.PayablePM.CorrectionByUserId)) {
                    this.SetLineStatus();
                }
            }
            this.PayablePM.AccountedAmountInLocalCurrency = this.PayablePM.AccountedAmount * this.PayablePM.Rate;
            ;
            this.PayablePM.AccountedAmountInProfitCurrency = this.PayablePM.AccountedAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;
            ;
            this.PayablePM.OpenAmountInLocalCurrency = this.PayablePM.OpenAmount * this.PayablePM.Rate;
            this.PayablePM.OpenAmountInProfitCurrency = this.PayablePM.OpenAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;
            this.PayablePM.CorrectionByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            this.PayablePM.CorrectionDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
    };
    return AWBWizardOtherChargeItem;
}(BaseComponent_1.BaseComponent));
exports.AWBWizardOtherChargeItem = AWBWizardOtherChargeItem;
//# sourceMappingURL=OtherChargesTabComponent.js.map