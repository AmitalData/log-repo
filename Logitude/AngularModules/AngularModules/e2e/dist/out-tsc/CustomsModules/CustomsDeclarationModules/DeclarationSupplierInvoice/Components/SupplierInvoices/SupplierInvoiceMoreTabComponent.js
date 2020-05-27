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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CustomsExchangeRateExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var SupplierInvoiceModificationPM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoiceModificationPM");
var SupplierInvoiceMoreTabComponent = /** @class */ (function (_super) {
    __extends(SupplierInvoiceMoreTabComponent, _super);
    function SupplierInvoiceMoreTabComponent() {
        var _this = _super.call(this) || this;
        _this.FillValidationErrorList = new core_1.EventEmitter();
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.SupplierInvoice";
        _this.IsDisplayOnly = false;
        //Services
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService;
        _this.ModificationsList = new ObservableCollection_1.ObservableCollection([]);
        // initialize query filters for Parent Account
        _this.TypeCodeFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.TypeCodeFilterItems.addAdditionalFilter("Code", "I02", null, null, "Exclude", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        //this.TypeCodeFilterItems.addAdditionalFilter("LocalName", "I02", null, null, "Exclude", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        _this.TypeCodeFilterItems.addAdditionalFilter("LocalName", "I02", null, null, "NotContains", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        //this.TypeCodeFilterItems.addAdditionalFilter("SearchFields", "I02", null, null, "Exclude", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        _this.TypeCodeFilterItems.addAdditionalFilter("SearchFields", "I02", null, null, "NotContains", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        _this.TypeCodeFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.TypeCodeFilterItems.addAdditionalFilter("Code", "I02", null, null, "NotContains", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        return _this;
    }
    SupplierInvoiceMoreTabComponent.prototype.SetTabArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.InvoicePM = args.InvoicePM;
            this.declarationPM = args.DeclarationPM;
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.Parent = args.Parent;
            this.Parent.ReloadModificationEvent.subscribe(function () {
                _this.FillGridData();
            });
            this.FillGridData();
        }
    };
    SupplierInvoiceMoreTabComponent.prototype.FillGridData = function () {
        this.ModificationsList = new ObservableCollection_1.ObservableCollection([]);
        for (var _i = 0, _a = this.InvoicePM.SupplierInvoiceModifications; _i < _a.length; _i++) {
            var item = _a[_i];
            if (item.TypeCode != "I02")
                this.ModificationsList.Insert(new ModificationItemModel(item, this));
        }
    };
    Object.defineProperty(SupplierInvoiceMoreTabComponent.prototype, "ActualPayedCurrencyTypeCode", {
        //#region Properties
        get: function () { return this.InvoicePM.ActualPayedCurrencyTypeCode; },
        set: function (value) {
            if (this.InvoicePM.ActualPayedCurrencyTypeCode != value) {
                this.InvoicePM.ActualPayedCurrencyTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceMoreTabComponent.prototype, "ActualPayedAmount", {
        get: function () { return this.InvoicePM.ActualPayedAmount; },
        set: function (value) {
            if (this.InvoicePM.ActualPayedAmount != value) {
                this.InvoicePM.ActualPayedAmount = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceMoreTabComponent.prototype.AddModificationButton = function () {
        // 1- check if list have invalid items
        if (this.ModificationsList.Length > 0) {
            var exist = this.ModificationsList.Collection.find(function (d) { return !d.isValid; });
            if (exist) {
                return;
            }
        }
        // 2- get counter
        var modificationCounter = 0;
        if (this.ModificationsList.Length > 0) {
            modificationCounter = this.getMax(this.ModificationsList.Collection, "ModificationCounterKey");
        }
        modificationCounter += 1;
        // 3- build new item
        var item = new SupplierInvoiceModificationPM_1.SupplierInvoiceModificationPM(this.InvoicePM);
        item.DeclarationId = this.InvoicePM.DeclarationId;
        item.InvoiceCounterKey = this.InvoicePM.InvoiceCounterKey;
        item.Tenant = SessionLocator_1.SessionLocator.Tenant;
        item.ChangeSetOp = "Insert";
        item.ModificationCounterKey = modificationCounter;
        // 4- add it to entityPM
        this.InvoicePM.AddSupplierInvoiceModification(item);
        // 5- add it to observable list
        this.ModificationsList.Insert(new ModificationItemModel(item, this));
        this.NewModificationAdded = true;
    };
    SupplierInvoiceMoreTabComponent.prototype.RemoveModification = function (item) {
        console.log("... Removing ", item);
        this.ModificationsList.Remove(item);
        this.InvoicePM.RemoveSupplierInvoiceModification(item.ModificationPM); // remove from entity
    };
    SupplierInvoiceMoreTabComponent.prototype.OnRowEnded = function ($event) {
        console.log("Length : " + this.ModificationsList.Length);
        if (($event) == this.ModificationsList.Length) {
            this.AddModificationButton();
        }
    };
    SupplierInvoiceMoreTabComponent.prototype.OnFocus = function () {
        //if (this.ModificationsList.Length == 0) {
        //    this.AddModificationButton();
        //}
    };
    SupplierInvoiceMoreTabComponent.prototype.getMax = function (list, propertyName) {
        var max = -99999;
        var maxObj = list ? list.reduce(function (prev, current) { return (prev[propertyName] > current[propertyName]) ? prev : current; }) : null;
        if (maxObj != null)
            if (max <= maxObj[propertyName])
                max = maxObj[propertyName];
        return max;
    };
    //#endregion
    SupplierInvoiceMoreTabComponent.prototype.FillErrors = function (errors) {
        console.log("[ERRORs] SupplierInvoiceMoreTabComponent: ", errors);
        this.FillValidationErrorList.emit(errors); // clear validation msgs
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SupplierInvoiceMoreTabComponent.prototype, "FillValidationErrorList", void 0);
    SupplierInvoiceMoreTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SupplierInvoiceMoreTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SupplierInvoiceMoreTabComponent);
    return SupplierInvoiceMoreTabComponent;
}(BaseComponent_1.BaseComponent));
exports.SupplierInvoiceMoreTabComponent = SupplierInvoiceMoreTabComponent;
var ModificationItemModel = /** @class */ (function (_super) {
    __extends(ModificationItemModel, _super);
    function ModificationItemModel(modificationPM, parent) {
        var _this = _super.call(this) || this;
        _this.modificationPM = modificationPM;
        _this.parent = parent;
        _this.ModificationPM = null;
        _this.ObjectTableName = "Customs.SupplierInvoiceModification";
        _this.DataContext = _this;
        _this.customsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService_1.CustomsExchangeRateExtendedPMService();
        _this.doCalculate = false;
        _this.DiscountInNIS = 0;
        _this.InvoiceCurrencyExchangeRtae = 0;
        _this.DiscountInDsicCurrency = 0;
        _this.ModificationPM = modificationPM;
        _this.isValid = true;
        _this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(_this.parent.InvoicePM.InvoiceCurrencyTypeCode, _this.parent.declarationPM.TaxationDateTime).subscribe(function (response) {
            if (response) {
                if (response.Result) {
                    var rate = response.Result[0];
                    if (rate) {
                        _this.InvoiceCurrencyExchangeRtae = rate.ExchangeRate;
                    }
                }
            }
        });
        return _this;
    }
    Object.defineProperty(ModificationItemModel.prototype, "TypeCode", {
        //#region Properties
        get: function () { return this.ModificationPM.TypeCode; },
        set: function (value) {
            if (this.ModificationPM.TypeCode != value) {
                //this.ModificationPM.TypeCode = value;
                if (value == "I02") {
                    this.ModificationPM.TypeCode = value;
                    this.isValid = false;
                    var errors = [];
                    errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee") + " - מסך נוספים");
                    this.parent.FillErrors(errors);
                }
                else {
                    var exists;
                    if (this.parent.InvoicePM.SupplierInvoiceModifications.length != 0) {
                        exists = this.parent.InvoicePM.SupplierInvoiceModifications.find(function (d) { return d.TypeCode == value; });
                    }
                    if (exists) {
                        this.ModificationPM.TypeCode = value;
                        //ModificationAndDiscountTypeList modificationAndDiscountType = ModificationAndDiscountTypeDataProvider.GetCachedList<ModificationAndDiscountTypeList>().Where(d => d.Code == value).FirstOrDefault();
                        //TypeName = modificationAndDiscountType != null ? modificationAndDiscountType.LocalName : null;
                        this.isValid = false;
                        var errors = [];
                        errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType") + " - מסך נוספים");
                        this.parent.FillErrors(errors);
                    }
                    else {
                        this.ModificationPM.TypeCode = value;
                        //FirePropertyChanged("TypeCode");
                        this.isValid = true;
                        //ModificationAndDiscountTypeList modificationAndDiscountType = ModificationAndDiscountTypeDataProvider.GetCachedList<ModificationAndDiscountTypeList>().Where(d => d.Code == value).FirstOrDefault();
                        //TypeName = modificationAndDiscountType != null ? modificationAndDiscountType.LocalName : null;
                        var errors = [];
                        this.parent.FillErrors(errors);
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "TypeName", {
        get: function () { return this.ModificationPM.TypeName; },
        set: function (value) {
            if (this.ModificationPM.TypeName != value) {
                this.ModificationPM.TypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "CurrencyTypeCode", {
        get: function () { return this.ModificationPM.CurrencyTypeCode; },
        set: function (value) {
            if (this.ModificationPM.CurrencyTypeCode != value) {
                this.ModificationPM.CurrencyTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "CurrencyTypeName", {
        get: function () { return this.ModificationPM.CurrencyTypeName; },
        set: function (value) {
            if (this.ModificationPM.CurrencyTypeName != value) {
                this.ModificationPM.CurrencyTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModificationItemModel.prototype, "Amount", {
        get: function () { return this.ModificationPM.Amount; },
        set: function (value) {
            if (this.ModificationPM.Amount != value) {
                this.ModificationPM.Amount = value;
                //    this.doCalculate = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    ModificationItemModel.prototype.AmountOriginalText = function (originalText) {
        this.OriginalText = originalText;
        if (this.OriginalText) {
            if ((this.OriginalText + "").indexOf('%') > -1) {
                this.doCalculate = true;
            }
        }
        this.OriginalText = null;
    };
    //#endregion
    ModificationItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    ModificationItemModel.prototype.OnAmountLostFocus = function () {
        var _this = this;
        if (this.doCalculate) {
            var value = this.Amount;
            this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.CurrencyTypeCode, this.parent.declarationPM.TaxationDateTime).subscribe(function (response) {
                if (_this.parent.InvoicePM.InvoiceAmount) {
                    _this.DiscountInNIS = _this.parent.InvoicePM.InvoiceAmount * _this.InvoiceCurrencyExchangeRtae * value;
                    //  value = value * this.parent.InvoicePM.InvoiceAmount;
                    if (response) {
                        if (response.Result) {
                            var result = response.Result[0];
                            if (result) {
                                var amount = _this.DiscountInNIS / result.ExchangeRate;
                                if (amount) {
                                    _this.Amount = amount;
                                }
                            }
                        }
                    }
                }
                _this.doCalculate = false;
            });
        }
    };
    return ModificationItemModel;
}(BaseComponent_1.BaseComponent));
exports.ModificationItemModel = ModificationItemModel;
//# sourceMappingURL=SupplierInvoiceMoreTabComponent.js.map