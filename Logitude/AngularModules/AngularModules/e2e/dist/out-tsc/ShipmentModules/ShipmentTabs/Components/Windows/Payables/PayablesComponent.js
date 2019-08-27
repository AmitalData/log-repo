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
var NumbersPipe_1 = require("../../../../../Infrastructure/Pipes/NumbersPipe");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ShipmentReceivablePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentReceivablePM");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_2 = require("../../../../../Shipment/Tools");
var ChargesTypeListService_1 = require("../../../../../Common/Services/StandardLists/ChargesTypeListService");
var PackageTypeListService_1 = require("../../../../../Common/Services/StandardLists/PackageTypeListService");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var PayablesComponent = /** @class */ (function (_super) {
    __extends(PayablesComponent, _super);
    function PayablesComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.AllChargesTypes = [];
        _this.AllPackageTypes = [];
        _this.PayableText = null;
        _this.ReceivableText = null;
        _this.QuantityText = null;
        _this.UnitPriceText = null;
        _this.AmountText = null;
        _this.AmountLocalText = null;
        _this.payables = null;
        _this.receivables = null;
        _this.IsOkButtonEnabled = false;
        _this.SummaryWidth = 250;
        _this.linesCloners = [];
        _this.linesChildsCloners = [];
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.InitializeServices();
        return _this;
    }
    PayablesComponent.prototype.InitializeServices = function () {
        this.myChargesTypeListService = new ChargesTypeListService_1.ChargesTypeListService();
        this.myPackageTypeListService = new PackageTypeListService_1.PackageTypeListService();
    };
    PayablesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['EntityPM'];
        this.BaseQuote = args['BaseQuote'];
        this.entityResourceService.getEntityResourceByTableName("ShipmentReceivable").subscribe(function (res) {
            _this.entityResourceService.getEntityResourceByTableName("ShipmentPayable").subscribe(function (res2) {
                _this.SetLabels();
                _this.myChargesTypeListService.getAllFromCache().subscribe(function (myResponse1) {
                    if (!myResponse1.HasError) {
                        _this.AllChargesTypes = myResponse1.Result;
                        _this.myPackageTypeListService.getAllFromCache().subscribe(function (myResponse2) {
                            if (!myResponse2.HasError) {
                                _this.AllPackageTypes = myResponse2.Result;
                                _this.BuildObsList();
                                _this.ComputeTotals();
                                _this.ComputeWidth();
                                _this.Clone();
                                _this.IsResourcesReady = true;
                            }
                        });
                    }
                });
            });
        });
    };
    PayablesComponent.prototype.SetLabels = function () {
        this.PayableText = TextCodeTranslator_1.TextCodeTranslator.Translate('Shipment.S.Receivables.Payable');
        this.ReceivableText = TextCodeTranslator_1.TextCodeTranslator.Translate('Shipment.S.Receivables.Receivable');
        this.QuantityText = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentReceivable.F.Quantity");
        this.UnitPriceText = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentReceivable.F.UnitPrice");
        this.AmountText = TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentReceivable.F.Amount.Short");
        this.AmountLocalText = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Receivables.AmountLocal").replace("%LocalCurrencyCode", SessionLocator_1.SessionLocator.LocalCurrencyCode);
    };
    PayablesComponent.prototype.BuildObsList = function () {
        var _this = this;
        var itemsCollection = [];
        this.EntityPM.ShipmentPayables.forEach(function (item) {
            itemsCollection.push(new GenerateFromPayablesModelData(item, _this));
        });
        this.ItemsSource.InsertCollection(itemsCollection);
    };
    Object.defineProperty(PayablesComponent.prototype, "Payables", {
        get: function () { return this.payables; },
        set: function (value) {
            if (this.payables != value) {
                this.payables = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PayablesComponent.prototype, "Receivables", {
        get: function () { return this.receivables; },
        set: function (value) {
            if (this.receivables != value) {
                this.receivables = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PayablesComponent.prototype, "Profit", {
        get: function () { return this.EntityPM.ProfitInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.ProfitInLocalCurrency != value) {
                this.EntityPM.ProfitInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PayablesComponent.prototype, "EstimateProfit", {
        get: function () { return this.EntityPM.EstimateProfitInLocalCurrency; },
        set: function (value) {
            if (this.EntityPM.EstimateProfitInLocalCurrency != value) {
                this.EntityPM.EstimateProfitInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PayablesComponent.prototype, "ProfitColor", {
        get: function () {
            var myResult = Tools_1.FontTool.Black;
            if (this.Profit < 0) {
                myResult = Tools_1.FontTool.Red;
            }
            else if (this.Profit > 0) {
                myResult = Tools_1.FontTool.Green;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    PayablesComponent.prototype.ComputeTotals = function () {
        var myPayables = 0;
        var myReceivables = null;
        if (this.ItemsSource.Collection.length > 0) {
            myPayables = Tools_1.ArrayTool.Sum(this.ItemsSource.Collection, "PayableAmountLocal");
            if (this.ItemsSource.Collection.filter(function (f) { return f.IsAdded == true; }).length > 0) {
                myReceivables = Tools_1.ArrayTool.Sum(this.ItemsSource.Collection.filter(function (f) { return f.IsAdded == true; }), "ReceivableAmountLocal");
            }
        }
        this.Payables = myPayables;
        this.Receivables = myReceivables;
    };
    PayablesComponent.prototype.ComputeAmounts = function () {
        this.ComputeTotals();
        var myProfit = 0;
        var isOkButtonEnabled = false;
        if (this.ItemsSource.Collection.length > 0) {
            if (this.ItemsSource.Collection.filter(function (f) { return f.IsAdded == true; }).length > 0) {
                isOkButtonEnabled = true;
                var myPayables = Tools_1.ArrayTool.Sum(this.ItemsSource.Collection, "PayableAmountLocal");
                var myReceivables = Tools_1.ArrayTool.Sum(this.ItemsSource.Collection.filter(function (f) { return f.IsAdded == true; }), "ReceivableAmountLocal");
                myProfit = myReceivables - myPayables;
            }
        }
        this.Profit = myProfit == null ? 0 : myProfit;
        this.IsOkButtonEnabled = isOkButtonEnabled;
        this.ComputeWidth();
    };
    PayablesComponent.prototype.ComputeWidth = function () {
        var PayablesWidth = 0;
        var RecevableWidth = 0;
        var ProfitWidth = 0;
        var mySummaryWidth = 0;
        var myRightSideWidth = 0;
        var myPipe = new NumbersPipe_1.NumbersPipe();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Payables)) {
            PayablesWidth = Tools_1.AppTool.GetTextWidth(myPipe.transform(this.Payables, 'N2'));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Receivables)) {
            RecevableWidth = Tools_1.AppTool.GetTextWidth(myPipe.transform(this.Receivables, 'N2'));
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Profit)) {
            ProfitWidth = Tools_1.AppTool.GetTextWidth(myPipe.transform(this.Profit, 'N2'));
        }
        if (PayablesWidth > myRightSideWidth) {
            myRightSideWidth = PayablesWidth;
        }
        if (RecevableWidth > myRightSideWidth) {
            myRightSideWidth = RecevableWidth;
        }
        if (ProfitWidth > myRightSideWidth) {
            myRightSideWidth = ProfitWidth;
        }
        mySummaryWidth = myRightSideWidth + 120 + 50;
        if (mySummaryWidth < 250) {
            mySummaryWidth = 250;
        }
        this.SummaryWidth = mySummaryWidth;
    };
    PayablesComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    PayablesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ItemsSource.Collection.forEach(function (p) {
            if (p.IsAdded)
                _this.EntityPM.AddReceivable(p.ReceivablePM);
        });
        this.BuildObsList();
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    PayablesComponent.prototype.Clone = function () {
        var _this = this;
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('Profit');
        this.myCloner.AddField('EstimateProfit');
        this.myCloner.AddEntity(this.EntityPM);
        var lines = this.EntityPM.ShipmentPayables;
        lines.forEach(function (itemPayablePM) {
            var lineCloner = new Cloner_1.Cloner(itemPayablePM);
            lineCloner.AddField('Quantity');
            lineCloner.AddField('UnitPrice');
            lineCloner.AddField('ExpectedAmount');
            lineCloner.AddField('ExpectedAmountLocal');
            lineCloner.AddField('ExpectedAmountInProfitCurrency');
            lineCloner.AddField('OpenAmount');
            lineCloner.AddField('OpenAmountInLocalCurrency');
            lineCloner.AddField('OpenAmountInProfitCurrency');
            lineCloner.AddField('ShipmentPayableLineStatusCode');
            lineCloner.AddField('CorrectionAmount');
            lineCloner.AddField('CorrectionByUserId');
            lineCloner.AddField('CorrectionDate');
            lineCloner.AddEntity(itemPayablePM);
            _this.linesCloners.push(lineCloner);
            if (_this.EntityPM.ShipmentLevelCode == "C") {
                itemPayablePM.ChildShipmentPayables.forEach(function (itemChild) {
                    var lineChildCloner = new Cloner_1.Cloner(itemChild);
                    lineChildCloner.AddField('Quantity');
                    lineChildCloner.AddField('UnitPrice');
                    lineChildCloner.AddField('ExpectedAmount');
                    lineChildCloner.AddField('ExpectedAmountLocal');
                    lineChildCloner.AddField('ExpectedAmountInProfitCurrency');
                    lineChildCloner.AddField('OpenAmount');
                    lineChildCloner.AddField('OpenAmountInLocalCurrency');
                    lineChildCloner.AddField('OpenAmountInProfitCurrency');
                    lineChildCloner.AddField('AccountedAmount');
                    lineChildCloner.AddField('AccountedAmountInLocalCurrency');
                    lineChildCloner.AddField('AccountedAmountInProfitCurrency');
                    lineChildCloner.AddEntity(itemChild);
                    _this.linesChildsCloners.push(lineChildCloner);
                });
            }
        });
    };
    PayablesComponent.prototype.RejectChanges = function () {
        this.linesCloners.forEach(function (lineCloner) {
            lineCloner.RejectChanges();
        });
        this.linesChildsCloners.forEach(function (lineChildCloner) {
            lineChildCloner.RejectChanges();
        });
        this.myCloner.RejectChanges();
    };
    PayablesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PayablesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], PayablesComponent);
    return PayablesComponent;
}(BaseComponent_1.BaseComponent));
exports.PayablesComponent = PayablesComponent;
var GenerateFromPayablesModelData = /** @class */ (function (_super) {
    __extends(GenerateFromPayablesModelData, _super);
    function GenerateFromPayablesModelData(myPayablePM, FatherComponent) {
        var _this = _super.call(this) || this;
        _this.FatherComponent = FatherComponent;
        _this.markup = "0";
        _this.isAdded = false;
        _this.ShipmentPM = FatherComponent.EntityPM;
        _this.PayablePM = myPayablePM;
        _this.CreateReceivable();
        return _this;
    }
    GenerateFromPayablesModelData.prototype.CreateReceivable = function () {
        var _this = this;
        var chargesType = this.FatherComponent.AllChargesTypes.filter(function (f) { return f.Id == _this.PayablePM.ChargesTypeId; })[0];
        if (chargesType) {
            this.ReceivablePM = new ShipmentReceivablePM_1.ShipmentReceivablePM(null);
            this.ReceivablePM.Tenant = this.PayablePM.Tenant;
            this.ReceivablePM.ShipmentId = this.ShipmentPM.Id;
            this.ReceivablePM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
            this.ReceivablePM.ChargesTypeId = chargesType.Id;
            this.ReceivablePM.ChargesTypeCode = chargesType.Code;
            this.ReceivablePM.ChargesTypeName = chargesType.EnglishName;
            this.ReceivablePM.ChargesGroupCode = chargesType.ChargesGroupCode;
            this.ReceivablePM.CurrencyId = this.PayablePM.CurrencyId;
            this.ReceivablePM.CurrencyCode = this.PayablePM.CurrencyCode;
            this.ReceivablePM.DueTypeCode = chargesType.DueTypeCode;
            this.ReceivablePM.IATACodeId = chargesType.IATACodeId;
            this.ReceivablePM.MeasurementId = this.PayablePM.MeasurementId;
            this.ReceivablePM.MeasurementCode = this.PayablePM.MeasurementCode;
            this.ReceivablePM.MeasurementShortName = this.PayablePM.MeasurementShortName;
            this.ReceivablePM.Quantity = this.PayablePM.Quantity;
            this.ReceivablePM.PrepaidCollectId = chargesType.ChargesGroupCode == "FRT" ? this.ShipmentPM.FreightPrepaidCollectId : this.ShipmentPM.OtherPrepaidCollectId;
            this.ReceivablePM.Rate = this.PayablePM.Rate;
            this.ReceivablePM.TotalAmount = this.PayablePM.ExpectedAmount;
            this.ReceivablePM.TotalAmountLocal = this.PayablePM.ExpectedAmountLocal;
            this.ReceivablePM.UnitPrice = this.PayablePM.UnitPrice;
            this.ReceivablePM.UpdateByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
            this.ReceivablePM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            this.ReceivablePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
            this.ReceivablePM.VatTypeId = chargesType.VatTypeId;
            this.ReceivablePM.ViewOrder = chargesType.ViewOrder;
            this.ReceivablePM.ShipmentReceivableLineStatusCode = "OAMT";
            this.ReceivablePM.AmountInProfitCurrency = this.PayablePM.ExpectedAmountInProfitCurrency;
            this.ReceivablePM.ProfitCurrencyExchangeRate = this.PayablePM.ProfitCurrencyExchangeRate;
            this.ReceivablePM.DueTypeName = this.PayablePM.DueTypeName;
            this.ReceivablePM.IsExpense = chargesType.IsExpense;
        }
    };
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "ChargesTypeCode", {
        get: function () { return this.PayablePM.ChargesTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "ChargesTypeName", {
        get: function () { return this.PayablePM.ChargesTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "MeasurementCode", {
        get: function () { return this.PayablePM.MeasurementCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "CurrencyCode", {
        get: function () { return this.PayablePM.CurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "PayableQuantity", {
        // Payable
        get: function () { return this.PayablePM.Quantity; },
        set: function (value) {
            if (this.PayablePM.Quantity != value) {
                this.PayablePM.Quantity = Tools_1.AppTool.Round(value, 2);
                if (this.PayablePM.IsChargeBySteps) {
                    Tools_2.ShipmentTool.SetPayableUnitPriceBySteps(this.PayablePM, this.FatherComponent.BaseQuote);
                }
                this.ComputePayableTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "PayableUnitPrice", {
        get: function () { return this.PayablePM.UnitPrice; },
        set: function (value) {
            if (this.PayablePM.UnitPrice != value) {
                this.PayablePM.UnitPrice = Tools_1.AppTool.Round(value, 3);
                this.ComputePayableTotalAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "PayableAmount", {
        get: function () { return this.PayablePM.ExpectedAmount; },
        set: function (value) {
            if (this.PayablePM.ExpectedAmount != value) {
                this.PayablePM.ExpectedAmount = Tools_1.AppTool.Round(value, 2);
                this.SetLineStatus();
                this.ComputeUnitPrice();
                this.ComputePayableTotalAmountLocal();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "PayableAmountLocal", {
        get: function () { return this.PayablePM.ExpectedAmountLocal; },
        set: function (value) {
            if (this.PayablePM.ExpectedAmountLocal != value) {
                this.PayablePM.ExpectedAmountLocal = Tools_1.AppTool.Round(value, 2);
                if (this.ShipmentPM.ShipmentLevelCode == "C") {
                    this.ComputeInsidePayablesData();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "PayableAmountInProfitCurrency", {
        get: function () { return this.PayablePM.ExpectedAmountInProfitCurrency; },
        set: function (value) {
            if (this.PayablePM.ExpectedAmountInProfitCurrency != value) {
                this.PayablePM.ExpectedAmountInProfitCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "PayableOpenAmount", {
        get: function () { return this.PayablePM.OpenAmount; },
        set: function (value) {
            if (this.PayablePM.OpenAmount != value) {
                this.PayablePM.OpenAmount = Tools_1.AppTool.Round(value, 2);
                this.OpenAmountInLocalCurrency = value * this.PayablePM.Rate;
                this.OpenAmountInProfitCurrency = this.OpenAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;
                var expe = this.PayablePM.ExpectedAmount == null ? 0 : this.PayablePM.ExpectedAmount;
                var acct = this.PayablePM.AccountedAmount == null ? 0 : this.PayablePM.AccountedAmount;
                var open = value == null ? 0 : value;
                this.CorrectionAmount = expe - acct - open;
                if (!Tools_1.AppTool.IsNullOrEmpty(this.PayablePM.CorrectionByUserId)) {
                    this.SetLineStatus();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "OpenAmountInLocalCurrency", {
        get: function () { return this.PayablePM.OpenAmountInLocalCurrency; },
        set: function (value) {
            if (this.PayablePM.OpenAmountInLocalCurrency != value) {
                this.PayablePM.OpenAmountInLocalCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "OpenAmountInProfitCurrency", {
        get: function () { return this.PayablePM.OpenAmountInProfitCurrency; },
        set: function (value) {
            if (this.PayablePM.OpenAmountInProfitCurrency != value) {
                this.PayablePM.OpenAmountInProfitCurrency = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "CorrectionAmount", {
        get: function () { return this.PayablePM.CorrectionAmount; },
        set: function (value) {
            if (this.PayablePM.CorrectionAmount != value) {
                this.PayablePM.CorrectionAmount = Tools_1.AppTool.Round(value, 2);
                this.CorrectionByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.CorrectionDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "CorrectionByUserId", {
        get: function () { return this.PayablePM.CorrectionByUserId; },
        set: function (value) {
            if (this.PayablePM.CorrectionByUserId != value) {
                this.PayablePM.CorrectionByUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "CorrectionDate", {
        get: function () { return this.PayablePM.CorrectionDate; },
        set: function (value) {
            if (this.PayablePM.CorrectionDate != value) {
                this.PayablePM.CorrectionDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    GenerateFromPayablesModelData.prototype.SetLineStatus = function () {
        Tools_2.ShipmentTool.SetPayableLineStatus(this.PayablePM);
    };
    GenerateFromPayablesModelData.prototype.ComputeUnitPrice = function () {
        if (!this.PayablePM.IsChargeBySteps) {
            var myResult = null;
            if (this.PayablePM.ExpectedAmount != null && this.PayablePM.Quantity != null) {
                if (this.PayablePM.Quantity == 0) {
                    myResult = 0;
                }
                else {
                    myResult = this.PayablePM.ExpectedAmount / this.PayablePM.Quantity;
                }
            }
            this.PayablePM.UnitPrice = Tools_1.AppTool.Round(myResult, 3);
        }
    };
    GenerateFromPayablesModelData.prototype.ComputePayableTotalAmount = function () {
        var myResult = null;
        this.SetLineStatus();
        if (this.PayablePM.Quantity != null && this.PayablePM.UnitPrice != null) {
            myResult = this.PayablePM.Quantity * this.PayablePM.UnitPrice;
        }
        /* From Tarrifs */
        if (this.PayablePM.MinAmount != null || this.PayablePM.MaxAmount != null) {
            if (myResult != null) {
                if (myResult < this.PayablePM.MinAmount) {
                    myResult = this.PayablePM.MinAmount;
                }
                else if (myResult > this.PayablePM.MaxAmount) {
                    myResult = this.PayablePM.MaxAmount;
                }
            }
        }
        if (this.PayablePM.QuoteCostMinAmount != null) {
            if (myResult == null) {
                myResult = this.PayablePM.QuoteCostMinAmount;
            }
            else {
                if (myResult < this.PayablePM.QuoteCostMinAmount) {
                    myResult = this.PayablePM.QuoteCostMinAmount;
                }
            }
        }
        this.PayablePM.ExpectedAmount = Tools_1.AppTool.Round(myResult, 2);
        this.ComputePayableTotalAmountLocal();
    };
    GenerateFromPayablesModelData.prototype.ComputePayableTotalAmountLocal = function () {
        if (this.PayablePM.ExpectedAmount != null && this.PayablePM.Rate != null) {
            this.PayableAmountLocal = Tools_1.AppTool.Round(this.PayablePM.ExpectedAmount * this.PayablePM.Rate, 2);
        }
        else {
            this.PayableAmountLocal = null;
        }
        this.ComputePayableTotalAmountInProfitCurrency();
        this.ComputeInsidePayablesData();
        this.FatherComponent.ComputeAmounts();
    };
    GenerateFromPayablesModelData.prototype.ComputePayableTotalAmountInProfitCurrency = function () {
        if (this.PayablePM.CurrencyId == this.ShipmentPM.ProfitCurrencyId) {
            this.PayableAmountInProfitCurrency = this.PayablePM.ExpectedAmount;
        }
        else {
            this.PayableAmountInProfitCurrency = this.PayableAmountLocal / this.PayablePM.ProfitCurrencyExchangeRate;
        }
        this.ComputePayableOtherAmounts();
    };
    GenerateFromPayablesModelData.prototype.ComputePayableOtherAmounts = function () {
        if (this.PayablePM.ShipmentPayableLineStatusCode == "EMPT" || this.PayablePM.ShipmentPayableLineStatusCode == "OAMT") {
            this.PayablePM.CorrectionAmount = 0;
            this.PayablePM.AccountedAmount = 0;
            this.PayablePM.AccountedAmountInLocalCurrency = 0;
            this.PayablePM.AccountedAmountInProfitCurrency = 0;
            if (this.PayablePM.OpenAmount != this.PayablePM.ExpectedAmount) {
                this.PayableOpenAmount = this.PayablePM.ExpectedAmount;
            }
            else {
                this.OpenAmountInLocalCurrency = this.PayablePM.OpenAmount * this.PayablePM.Rate;
                this.OpenAmountInProfitCurrency = this.OpenAmountInLocalCurrency / this.PayablePM.ProfitCurrencyExchangeRate;
            }
        }
    };
    GenerateFromPayablesModelData.prototype.ComputeInsidePayablesData = function () {
        var _this = this;
        if (this.ShipmentPM.ShipmentLevelCode == "C") {
            var _QuantityTotal = null;
            var _Ratio = null;
            var quantity = null;
            var unitPrice = null;
            this.PayablePM.ChildShipmentPayables.forEach(function (item) {
                switch (_this.MeasurementCode) {
                    case "VOLU": {
                        _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "Volume");
                        _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                        quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].Volume;
                        break;
                    }
                    case "GRWT": {
                        _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "GrossWeight");
                        _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                        quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].GrossWeight;
                        break;
                    }
                    case "GWTN": {
                        _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "GrossWeightPerTon");
                        _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                        quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].GrossWeightPerTon;
                        break;
                    }
                    case "QTY": {
                        if (Tools_2.ShipmentTool.IsLCL(_this.ShipmentPM)) {
                            _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "NumberOfPackages");
                            _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                            unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                            quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].NumberOfPackages;
                        }
                        else {
                            _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "NumberOfContainers");
                            _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                            unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                            quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].NumberOfContainers;
                        }
                        break;
                    }
                    case "CHWT": {
                        _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "ChargeableWeight");
                        _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                        quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].ChargeableWeight;
                        break;
                    }
                    case "BTEU": {
                        _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "TEU");
                        _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                        unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                        quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].TEU;
                        break;
                    }
                    case "FIXD": {
                        _Ratio = _this.PayablePM.Quantity / _this.ShipmentPM.ShipmentConsoleShipments.length;
                        unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                        quantity = 1;
                        break;
                    }
                    default: {
                        if ((_this.ShipmentPM.TransportModeId == "O" && _this.ShipmentPM.ShipmentTypeId == "FCLD") || (_this.ShipmentPM.TransportModeId == "I" && _this.ShipmentPM.ShipmentTypeId == "FTL")) {
                            var list = _this.FatherComponent.AllPackageTypes.filter(function (d) { return d.MeasurementId == _this.PayablePM.MeasurementId && d.Tenant == _this.PayablePM.Tenant; })[0];
                            if (list) {
                                var houseRecord = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0];
                                if (houseRecord != null) {
                                    var fclData = houseRecord.FCLDataList.filter(function (d) { return d.Id == list.Id; })[0];
                                    if (fclData != null) {
                                        quantity = fclData.Quantity;
                                    }
                                }
                                unitPrice = _this.PayablePM.UnitPrice;
                            }
                        }
                        else {
                            // Groupage by Chargeable
                            _QuantityTotal = Tools_1.ArrayTool.Sum(_this.ShipmentPM.ShipmentConsoleShipments, "ChargeableWeight");
                            _Ratio = _this.PayablePM.Quantity / _QuantityTotal;
                            unitPrice = _Ratio * _this.PayablePM.UnitPrice;
                            quantity = _this.ShipmentPM.ShipmentConsoleShipments.filter(function (d) { return d.Id == item.ShipmentId; })[0].ChargeableWeight;
                        }
                        break;
                    }
                }
                item.Quantity = Tools_1.AppTool.Round(quantity, 2);
                item.UnitPrice = Tools_1.AppTool.Round(unitPrice, 3);
                var expectedAmount = quantity * unitPrice;
                var expectedAmountLocal = expectedAmount * _this.PayablePM.Rate;
                var expectedAmountInProfitCurrency = expectedAmountLocal / _this.PayablePM.ProfitCurrencyExchangeRate;
                item.ExpectedAmount = Tools_1.AppTool.Round(expectedAmount, 2);
                item.ExpectedAmountLocal = Tools_1.AppTool.Round(expectedAmountLocal, 2);
                item.ExpectedAmountInProfitCurrency = Tools_1.AppTool.Round(expectedAmountInProfitCurrency, 2);
                item.OpenAmount = item.ExpectedAmount;
                item.OpenAmountInLocalCurrency = item.ExpectedAmountLocal;
                item.OpenAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;
                item.AccountedAmount = 0;
                item.AccountedAmountInLocalCurrency = 0;
                item.AccountedAmountInProfitCurrency = 0;
            });
        }
    };
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "ReceivableQuantity", {
        // Receivable
        get: function () { return this.ReceivablePM.Quantity; },
        set: function (value) {
            if (this.ReceivablePM.Quantity != value) {
                this.ReceivablePM.Quantity = Tools_1.AppTool.Round(value, 2);
                this.ComputeReceivableAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "ReceivableUnitPrice", {
        get: function () { return this.ReceivablePM.UnitPrice; },
        set: function (value) {
            if (this.ReceivablePM.UnitPrice != value) {
                this.ReceivablePM.UnitPrice = Tools_1.AppTool.Round(value, 2);
                this.ComputeReceivableAmount();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "ReceivableAmount", {
        get: function () { return this.ReceivablePM.TotalAmount; },
        set: function (value) {
            if (this.ReceivablePM.TotalAmount != value) {
                this.ReceivablePM.TotalAmount = Tools_1.AppTool.Round(value, 2);
                this.ComputeReceivableAmountLocal();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "ReceivableAmountLocal", {
        get: function () { return this.ReceivablePM.TotalAmountLocal; },
        set: function (value) {
            if (this.ReceivablePM.TotalAmountLocal != value) {
                this.ReceivablePM.TotalAmountLocal = Tools_1.AppTool.Round(value, 2);
            }
        },
        enumerable: true,
        configurable: true
    });
    GenerateFromPayablesModelData.prototype.ComputeReceivableAmount = function () {
        var amount = null;
        var amountLocal = null;
        var amountProfit = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ReceivableQuantity) && !Tools_1.AppTool.IsNullOrEmpty(this.ReceivableUnitPrice)) {
            amount = this.ReceivableQuantity * this.ReceivableUnitPrice;
            amountLocal = amount * this.ReceivablePM.Rate;
            amountProfit = amountLocal / this.ReceivablePM.ProfitCurrencyExchangeRate;
        }
        this.ReceivablePM.TotalAmount = Tools_1.AppTool.Round(amount, 2);
        this.ReceivablePM.TotalAmountLocal = Tools_1.AppTool.Round(amountLocal, 2);
        this.ReceivablePM.AmountInProfitCurrency = Tools_1.AppTool.Round(amountProfit, 2);
        this.ReceivablePM.ShipmentReceivableLineStatusCode = (this.ReceivablePM.Quantity != null && this.ReceivablePM.UnitPrice != null) ? "OAMT" : "EMPT";
        this.ComputeMarkp();
        this.FatherComponent.ComputeAmounts();
    };
    GenerateFromPayablesModelData.prototype.ComputeReceivableAmountLocal = function () {
        var amountLocal = this.ReceivableAmount * this.ReceivablePM.Rate;
        var amountProfit = amountLocal / this.ReceivablePM.ProfitCurrencyExchangeRate;
        this.ReceivableAmountLocal = amountLocal;
        this.ReceivablePM.AmountInProfitCurrency = Tools_1.AppTool.Round(amountProfit, 2);
    };
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "Markup", {
        get: function () {
            var myResult = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.markup)) {
                var str = this.markup.replace("%", "");
                var myPipe = new NumbersPipe_1.NumbersPipe();
                myResult = myPipe.transform(Tools_1.AppTool.Round(parseFloat(str), 3), "N3") + "";
                if (this.markup.includes("%")) {
                    myResult = myResult + "%";
                }
            }
            return myResult;
        },
        set: function (value) {
            this.markup = value;
            if (this.IsAdded == false) {
                this.IsAdded = true;
            }
            this.OnMarkUpChanged();
        },
        enumerable: true,
        configurable: true
    });
    GenerateFromPayablesModelData.prototype.OnMarkUpChanged = function () {
        var result = this.PayablePM.ExpectedAmount;
        if (this.PayablePM.UnitPrice != null && !Tools_1.AppTool.IsNullOrEmpty(this.markup)) {
            if (this.Markup.includes("%")) {
                var markupValue = parseFloat(this.markup.replace("%", ""));
                result = this.PayablePM.ExpectedAmount + (this.PayablePM.ExpectedAmount * (markupValue / 100));
            }
            else {
                var markupValue = parseFloat(this.markup);
                result = this.PayablePM.ExpectedAmount + markupValue;
            }
        }
        this.ReceivableAmount = result;
        if (this.ReceivablePM.Quantity != null && result != null) {
            this.ReceivablePM.UnitPrice = Tools_1.AppTool.Round(result / this.ReceivablePM.Quantity, 3);
        }
        this.FatherComponent.ComputeAmounts();
    };
    GenerateFromPayablesModelData.prototype.ComputeMarkp = function () {
        var result = 0;
        if (this.PayablePM.ExpectedAmount != null && this.ReceivablePM.TotalAmount != null) {
            if (this.markup.includes("%")) {
                result = ((this.ReceivablePM.TotalAmount - this.PayablePM.ExpectedAmount) * 100) / this.PayablePM.ExpectedAmount;
            }
            else {
                result = this.ReceivablePM.TotalAmount - this.PayablePM.ExpectedAmount;
            }
        }
        var str = result + "";
        if (this.markup.includes("%")) {
            str = str + "%";
        }
        this.markup = str;
    };
    Object.defineProperty(GenerateFromPayablesModelData.prototype, "IsAdded", {
        get: function () { return this.isAdded; },
        set: function (value) {
            if (this.isAdded != value) {
                this.isAdded = value;
                this.FatherComponent.ComputeAmounts();
            }
        },
        enumerable: true,
        configurable: true
    });
    return GenerateFromPayablesModelData;
}(BaseComponent_1.BaseComponent));
exports.GenerateFromPayablesModelData = GenerateFromPayablesModelData;
//# sourceMappingURL=PayablesComponent.js.map