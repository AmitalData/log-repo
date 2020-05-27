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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var QuoteChargePM_1 = require("../../../../Quote/EntityPMs/QuoteChargePM");
var LCLChargesComponent_1 = require("../../../QuoteCharges/Components/LCLChargesComponent");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CardListService_1 = require("../../../../Common/Services/StandardLists/CardListService");
var TariffsComponent = /** @class */ (function () {
    function TariffsComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ObjectTableName = "TarrifHeader";
        this.ItemsSource = [];
        this.TarrifCharges = [];
        this.IsResourcesReady = false;
        this.AllCards = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.showMyCarrier = true;
        this.showAllCarriers = false;
        this.selectedItem = null;
        this.myCardListService = new CardListService_1.CardListService();
        this.myDomainService = new PartnersDomainService_1.PartnersDomainService();
    }
    TariffsComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args.EntityPM;
        this.fatherComponent = args;
        this.entityResourceService.getEntityResourceByTableName("TarrifHeader").subscribe(function (res) {
            _this.entityResourceService.getEntityResourceByTableName("TarrifCharge").subscribe(function (res2) {
                _this.IsResourcesReady = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.MainCarriageCarrierId)) {
                    _this.CurrentSession.StartBusyIndicatorLoading();
                    _this.myCardListService.getSingle(_this.EntityPM.MainCarriageCarrierId).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list) {
                                _this.AllCards.push(list);
                            }
                        }
                        _this.LoadData();
                    });
                }
                else {
                    _this.LoadData();
                }
            });
        });
    };
    Object.defineProperty(TariffsComponent.prototype, "ShowMyCarrier", {
        get: function () { return this.showMyCarrier; },
        set: function (value) {
            if (this.showMyCarrier != value) {
                this.showMyCarrier = value;
                this.showAllCarriers = !value;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffsComponent.prototype, "ShowAllCarriers", {
        get: function () { return this.showAllCarriers; },
        set: function (value) {
            if (this.showAllCarriers != value) {
                this.showAllCarriers = value;
                this.showMyCarrier = !value;
                this.LoadData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffsComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
                this.TarrifCharges = [];
                if (value) {
                    this.TarrifCharges = value.EntityPM.TarrifCharges;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffsComponent.prototype.LoadData = function () {
        var _this = this;
        this.ItemsSource = [];
        this.SelectedItem = null;
        if (this.ShowMyCarrier) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MainCarriageCarrierId)) {
                this.CurrentSession.StartBusyIndicatorLoading();
                this.myDomainService.GetTarrifHeadersByCardIdAndTypeCode(this.EntityPM.MainCarriageCarrierId, "S", false).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        _this.BuildItemsSource(myResponse.Result);
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            this.myDomainService.GetTarrifHeadersByCardIdAndTypeCode(null, "S", false).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.BuildItemsSource(myResponse.Result);
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    TariffsComponent.prototype.BuildItemsSource = function (list) {
        var _this = this;
        list.forEach(function (item) {
            _this.ItemsSource.push(new TariffsItem(item, _this));
        });
        this.SelectedItem = this.ItemsSource[0];
        if (this.ItemsSource.length == 1) {
            var firstItem = this.ItemsSource[0];
            if (firstItem != null) {
                firstItem.IsChecked = true;
            }
        }
    };
    TariffsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TariffsComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var itemChecked = this.ItemsSource.filter(function (f) { return f.IsChecked == true; })[0];
        if (itemChecked != null) {
            var myCharges = itemChecked.EntityPM.TarrifCharges;
            myCharges.forEach(function (item) {
                var chargeItem = _this.fatherComponent.ItemsSource.Collection.filter(function (d) { return d.ChargesTypeId == item.ChargesTypeId && d.CostMeasurementId == item.MeasurementId && d.CostCurrencyId == item.CurrencyId; })[0];
                if (chargeItem != null) {
                    chargeItem.CostUnitPrice = item.UnitPrice;
                    chargeItem.CostMinAmount = item.MinPrice;
                    chargeItem.CostMaxAmount = item.MaxPrice;
                }
                else {
                    var chargePM = new QuoteChargePM_1.QuoteChargePM(_this.EntityPM);
                    chargePM.Tenant = _this.EntityPM.Tenant;
                    chargePM.QuoteId = _this.EntityPM.Id;
                    chargePM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    chargePM.MarkUpTypeCode = "F";
                    chargePM.MarkUpValue = 0;
                    chargePM.QuoteTypeCode = _this.EntityPM.QuoteTypeCode;
                    chargePM.SaleCurrencyId = _this.EntityPM.SaleCurrencyId;
                    chargePM.SaleCurrencyCode = _this.fatherComponent.GetCurrencyCode(_this.EntityPM.SaleCurrencyId);
                    chargePM.SaleExchangeRate = _this.fatherComponent.GetCurrencyRate(_this.EntityPM.SaleCurrencyId);
                    _this.EntityPM.AddQuoteChargePM(chargePM);
                    chargeItem = new LCLChargesComponent_1.QuoteChargeItem(chargePM, _this.fatherComponent, false);
                    chargeItem.ChargesTypeId = item.ChargesTypeId;
                    chargeItem.CostMeasurementId = item.MeasurementId;
                    chargeItem.CostCurrencyId = item.CurrencyId;
                    chargeItem.CostUnitPrice = item.UnitPrice;
                    chargeItem.CostMinAmount = item.MinPrice;
                    chargeItem.CostMaxAmount = item.MaxPrice;
                    _this.fatherComponent.ItemsSource.Insert(chargeItem);
                }
                chargeItem.ComputeCostAmounts();
                chargeItem.ComputeSalePrice();
            });
            this.fatherComponent.BuildItemsSource();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    };
    TariffsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TariffsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], TariffsComponent);
    return TariffsComponent;
}());
exports.TariffsComponent = TariffsComponent;
var TariffsItem = /** @class */ (function () {
    function TariffsItem(entityPM, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.isChecked = false;
        this.Id = entityPM.Id;
        this.EntityPM = entityPM;
        this.GetCarrier();
        this.GetDateStatus();
    }
    Object.defineProperty(TariffsItem.prototype, "CardId", {
        get: function () { return this.EntityPM.CardId; },
        enumerable: true,
        configurable: true
    });
    TariffsItem.prototype.GetCarrier = function () {
        var _this = this;
        if (this.CardId) {
            var list = this.fatherComponent.AllCards.filter(function (f) { return f.Id == _this.CardId; })[0];
            if (list) {
                this.Carrier = "(" + list.Code + ") " + list.EnglishName;
            }
            else {
                this.fatherComponent.myCardListService.getSingle(this.CardId).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        if (list) {
                            if (_this.fatherComponent.AllCards.filter(function (f) { return f.Id == _this.CardId; }).length == 0) {
                                _this.fatherComponent.AllCards.push(list);
                            }
                            _this.Carrier = "(" + list.Code + ") " + list.EnglishName;
                        }
                    }
                });
            }
        }
    };
    Object.defineProperty(TariffsItem.prototype, "FromDate", {
        get: function () { return this.EntityPM.FromDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffsItem.prototype, "ToDate", {
        get: function () { return this.EntityPM.ToDate; },
        enumerable: true,
        configurable: true
    });
    TariffsItem.prototype.GetDateStatus = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromDate) && !Tools_1.AppTool.IsNullOrEmpty(this.ToDate)) {
            var todayDateTicks = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateTicks;
            var fromDateTicks = Tools_1.DateTool.GetDateParts(this.FromDate).DateTicks;
            var toDateTicks = Tools_1.DateTool.GetDateParts(this.ToDate).DateTicks;
            if (todayDateTicks >= fromDateTicks && todayDateTicks <= toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Present") + ")";
                this.DateStatusColor = Tools_1.FontTool.Green;
            }
            else if (todayDateTicks < fromDateTicks && todayDateTicks < toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Future") + ")";
                this.DateStatusColor = Tools_1.FontTool.Magenta;
            }
            else if (todayDateTicks > fromDateTicks && todayDateTicks > toDateTicks) {
                this.DateStatus = "(" + TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Past") + ")";
                this.DateStatusColor = Tools_1.FontTool.Red;
            }
        }
    };
    Object.defineProperty(TariffsItem.prototype, "FromLocationString", {
        get: function () { return this.EntityPM.FromLocationString; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffsItem.prototype, "ToLocationString", {
        get: function () { return this.EntityPM.ToLocationString; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffsItem.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TariffsItem.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    this.fatherComponent.SelectedItem = this;
                    this.fatherComponent.ItemsSource.filter(function (f) { return f.Id != _this.Id; }).forEach(function (item) {
                        item.UnCheck();
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    TariffsItem.prototype.UnCheck = function () {
        this.isChecked = false;
    };
    return TariffsItem;
}());
//# sourceMappingURL=TariffsComponent.js.map