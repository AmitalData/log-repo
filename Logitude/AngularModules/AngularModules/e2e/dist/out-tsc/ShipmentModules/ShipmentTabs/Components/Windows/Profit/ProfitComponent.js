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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var NumbersPipe_1 = require("../../../../../Infrastructure/Pipes/NumbersPipe");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ShipmentDomainService_1 = require("../../../../../Shipment/Services/ShipmentDomainService");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ProfitComponent = /** @class */ (function () {
    function ProfitComponent() {
        this.IsByLocalCurrency = false;
        this.IsCurrencyFilterVisible = false;
        this.LocalCurrencyCode = null;
        this.ProfitCurrencyCode = null;
        this.SelectedCurrencyCode = null;
        this.ProfitsCollection = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isPayablesExists = false;
        this.isReceivablesExists = false;
        this.allHousesIdsString = null;
        this.AllPayables = [];
        this.AllRecievables = [];
        this.TotalProfit = null;
        this.TotalProfitText = "N/A";
        this.TotalPayablesText = "N/A";
        this.TotalReceivablesText = "N/A";
        this.EstimateProfit = null;
        this.Difference = null;
        this.myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
    }
    ProfitComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args['ShipmentPM'];
        this.IsByLocalCurrency = args['IsByLocalCurrency'];
        this.IsCurrencyFilterVisible = args['IsCurrencyFilterVisible'];
        this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        this.ProfitCurrencyCode = this.EntityPM.ProfitCurrencyCode;
        this.SelectedCurrencyCode = this.IsByLocalCurrency ? this.LocalCurrencyCode : this.ProfitCurrencyCode;
        this.SetLabels();
        this.BuildBaseData();
    };
    ProfitComponent.prototype.SetLabels = function () {
        this.ReceivablesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate('Shipment.S.Profit.Receivables').replace('%LocalCurrencyCode', this.SelectedCurrencyCode);
        this.PayablesHeader = TextCodeTranslator_1.TextCodeTranslator.Translate('Shipment.S.Profit.Payables').replace('%LocalCurrencyCode', this.SelectedCurrencyCode);
        this.ProfitHeader = TextCodeTranslator_1.TextCodeTranslator.Translate('Shipment.S.Profit.Profit').replace('%LocalCurrencyCode', this.SelectedCurrencyCode);
    };
    ProfitComponent.prototype.BuildBaseData = function () {
        if (this.EntityPM.ShipmentLevelCode != "C" || this.EntityPM.ShipmentConsoleShipments.length == 0) {
            if (this.EntityPM.ShipmentPayables.length > 0) {
                this.isPayablesExists = true;
            }
            if (this.EntityPM.ShipmentReceivables.length > 0) {
                this.isReceivablesExists = true;
            }
            this.AllPayables = this.EntityPM.ShipmentPayables;
            this.AllRecievables = this.EntityPM.ShipmentReceivables;
            this.BuildProfitData();
        }
        else {
            if (this.EntityPM.ConnectedShipmentsPayablesCount > 0) {
                this.isPayablesExists = true;
            }
            if (this.EntityPM.ProrateReceivables) {
                if (this.EntityPM.ConnectedShipmentsReceivablesCount > 0) {
                    this.isReceivablesExists = true;
                }
            }
            else {
                if (this.EntityPM.ShipmentReceivables.length > 0 || this.EntityPM.ConnectedShipmentsReceivablesCount > 0) {
                    this.isReceivablesExists = true;
                }
            }
            var housesIds = [];
            this.EntityPM.ShipmentConsoleShipments.forEach(function (item) {
                housesIds.push(item.Id);
            });
            this.allHousesIdsString = Tools_1.AppTool.GetIdsArrayText(housesIds);
            if (this.EntityPM.ProrateReceivables == false) {
                this.AllRecievables = this.EntityPM.ShipmentReceivables;
            }
            if (this.EntityPM.ProrateReceivables && this.EntityPM.ConnectedShipmentsReceivablesCount > 0) {
                this.LoadMasterHousesReceivables();
            }
            else if (this.EntityPM.ConnectedShipmentsPayablesCount > 0) {
                this.LoadMasterHousesPayables();
            }
            else {
                this.BuildProfitData();
            }
        }
    };
    ProfitComponent.prototype.LoadMasterHousesPayables = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetAllMasterHousesPayables(this.allHousesIdsString).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.AllPayables = myResponse.Result;
                _this.BuildProfitData();
            }
        });
    };
    ProfitComponent.prototype.LoadMasterHousesReceivables = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetAllMasterHousesReceivables(this.allHousesIdsString).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var loadedList = myResponse.Result;
                loadedList.forEach(function (item) {
                    _this.AllRecievables.push(item);
                });
                _this.LoadMasterHousesPayables();
            }
        });
    };
    ProfitComponent.prototype.BuildProfitData = function () {
        this.CurrentSession.StopBusyIndicator();
        this.ProfitsCollection = [];
        if (this.IsByLocalCurrency) {
            this.BuildDataInLocalCurrency();
        }
        else {
            this.BuildDataInProfitCurrency();
        }
        this.ComputeTotals();
    };
    ProfitComponent.prototype.BuildDataInLocalCurrency = function () {
        var _this = this;
        // Receivables Group by
        var myReceivables = [];
        this.AllRecievables.forEach(function (itemReceivable) {
            if (!Tools_1.AppTool.IsNullOrEmpty(itemReceivable.TotalAmountLocal)) {
                var itemReceivableOpenedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "OAMT" || itemReceivable.ShipmentReceivableLineStatusCode == "DRFT") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(itemReceivable.TotalAmountLocal)) {
                        itemReceivableOpenedAmount = itemReceivable.TotalAmountLocal;
                    }
                }
                var itemReceivableAcountedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "ACCT") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(itemReceivable.TotalAmountLocal)) {
                        itemReceivableAcountedAmount = itemReceivable.TotalAmountLocal;
                    }
                }
                var itemGrouped = myReceivables.filter(function (f) { return f.ChargeTypeId == itemReceivable.ChargesTypeId; })[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemReceivable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemReceivable.ChargesTypeName;
                    itemGrouped.ReceivableOpenedAmount = itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount = itemReceivableAcountedAmount;
                    myReceivables.push(itemGrouped);
                }
                else {
                    itemGrouped.ReceivableOpenedAmount += itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount += itemReceivableAcountedAmount;
                }
            }
        });
        // Payables Group by
        var myPayables = [];
        this.AllPayables.forEach(function (itemPayable) {
            if (!Tools_1.AppTool.IsNullOrEmpty(itemPayable.OpenAmountInLocalCurrency) || !Tools_1.AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInLocalCurrency)) {
                var itemPayableOpenedAmount = 0;
                if (!Tools_1.AppTool.IsNullOrEmpty(itemPayable.OpenAmountInLocalCurrency)) {
                    itemPayableOpenedAmount = itemPayable.OpenAmountInLocalCurrency;
                }
                var itemPayableAcountedAmount = 0;
                if (!Tools_1.AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInLocalCurrency)) {
                    itemPayableAcountedAmount = itemPayable.AccountedAmountInLocalCurrency;
                }
                var itemGrouped = myPayables.filter(function (f) { return f.ChargeTypeId == itemPayable.ChargesTypeId; })[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemPayable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemPayable.ChargesTypeName;
                    itemGrouped.PayableOpenedAmount = itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount = itemPayableAcountedAmount;
                    myPayables.push(itemGrouped);
                }
                else {
                    itemGrouped.PayableOpenedAmount += itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount += itemPayableAcountedAmount;
                }
            }
        });
        // Push Payables
        myPayables.forEach(function (item) {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.PayableOpenedAmount = _this.Fixed(item.PayableOpenedAmount);
            record.PayableAcountedAmount = _this.Fixed(item.PayableAcountedAmount);
            record.PayableAmount = _this.Fixed(item.PayableOpenedAmount + item.PayableAcountedAmount);
            var rec = myReceivables.filter(function (f) { return f.ChargeTypeId == item.ChargeTypeId; })[0];
            if (rec != null) {
                myReceivables.splice(myReceivables.indexOf(rec), 1);
                record.ReceivableOpenedAmount = _this.Fixed(rec.ReceivableOpenedAmount);
                record.ReceivableAcountedAmount = _this.Fixed(rec.ReceivableAcountedAmount);
                record.ReceivableAmount = _this.Fixed(rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount);
                record.Profit = (rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount) - (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }
            else {
                record.Profit = -1 * (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }
            _this.ProfitsCollection.push(record);
        });
        // Push Receivables
        myReceivables.forEach(function (item) {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.ReceivableOpenedAmount = _this.Fixed(item.ReceivableOpenedAmount);
            record.ReceivableAcountedAmount = _this.Fixed(item.ReceivableAcountedAmount);
            record.ReceivableAmount = _this.Fixed(item.ReceivableOpenedAmount + item.ReceivableAcountedAmount);
            record.Profit = item.ReceivableOpenedAmount + item.ReceivableAcountedAmount;
            _this.ProfitsCollection.push(record);
        });
    };
    ProfitComponent.prototype.BuildDataInProfitCurrency = function () {
        var _this = this;
        // Receivables Group by
        var myReceivables = [];
        this.AllRecievables.forEach(function (itemReceivable) {
            if (!Tools_1.AppTool.IsNullOrEmpty(itemReceivable.AmountInProfitCurrency)) {
                var itemReceivableOpenedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "OAMT" || itemReceivable.ShipmentReceivableLineStatusCode == "DRFT") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(itemReceivable.AmountInProfitCurrency)) {
                        itemReceivableOpenedAmount = itemReceivable.AmountInProfitCurrency;
                    }
                }
                var itemReceivableAcountedAmount = 0;
                if (itemReceivable.ShipmentReceivableLineStatusCode == "ACCT") {
                    if (!Tools_1.AppTool.IsNullOrEmpty(itemReceivable.AmountInProfitCurrency)) {
                        itemReceivableAcountedAmount = itemReceivable.AmountInProfitCurrency;
                    }
                }
                var itemGrouped = myReceivables.filter(function (f) { return f.ChargeTypeId == itemReceivable.ChargesTypeId; })[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemReceivable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemReceivable.ChargesTypeName;
                    itemGrouped.ReceivableOpenedAmount = itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount = itemReceivableAcountedAmount;
                    myReceivables.push(itemGrouped);
                }
                else {
                    itemGrouped.ReceivableOpenedAmount += itemReceivableOpenedAmount;
                    itemGrouped.ReceivableAcountedAmount += itemReceivableAcountedAmount;
                }
            }
        });
        // Payables Group by
        var myPayables = [];
        this.AllPayables.forEach(function (itemPayable) {
            if (!Tools_1.AppTool.IsNullOrEmpty(itemPayable.OpenAmountInProfitCurrency) || !Tools_1.AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInProfitCurrency)) {
                var itemPayableOpenedAmount = 0;
                if (!Tools_1.AppTool.IsNullOrEmpty(itemPayable.OpenAmountInProfitCurrency)) {
                    itemPayableOpenedAmount = itemPayable.OpenAmountInProfitCurrency;
                }
                var itemPayableAcountedAmount = 0;
                if (!Tools_1.AppTool.IsNullOrEmpty(itemPayable.AccountedAmountInProfitCurrency)) {
                    itemPayableAcountedAmount = itemPayable.AccountedAmountInProfitCurrency;
                }
                var itemGrouped = myPayables.filter(function (f) { return f.ChargeTypeId == itemPayable.ChargesTypeId; })[0];
                if (itemGrouped == null) {
                    itemGrouped = new ProfitClass();
                    itemGrouped.ChargeTypeId = itemPayable.ChargesTypeId;
                    itemGrouped.ChargeTypeName = itemPayable.ChargesTypeName;
                    itemGrouped.PayableOpenedAmount = itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount = itemPayableAcountedAmount;
                    myPayables.push(itemGrouped);
                }
                else {
                    itemGrouped.PayableOpenedAmount += itemPayableOpenedAmount;
                    itemGrouped.PayableAcountedAmount += itemPayableAcountedAmount;
                }
            }
        });
        // Push Payables
        myPayables.forEach(function (item) {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.PayableOpenedAmount = _this.Fixed(item.PayableOpenedAmount);
            record.PayableAcountedAmount = _this.Fixed(item.PayableAcountedAmount);
            record.PayableAmount = _this.Fixed(item.PayableOpenedAmount + item.PayableAcountedAmount);
            var rec = myReceivables.filter(function (f) { return f.ChargeTypeId == item.ChargeTypeId; })[0];
            if (rec != null) {
                myReceivables.splice(myReceivables.indexOf(rec), 1);
                record.ReceivableOpenedAmount = _this.Fixed(rec.ReceivableOpenedAmount);
                record.ReceivableAcountedAmount = _this.Fixed(rec.ReceivableAcountedAmount);
                record.ReceivableAmount = _this.Fixed(rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount);
                record.Profit = (rec.ReceivableOpenedAmount + rec.ReceivableAcountedAmount) - (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }
            else {
                record.Profit = -1 * (item.PayableOpenedAmount + item.PayableAcountedAmount);
            }
            _this.ProfitsCollection.push(record);
        });
        // Push Receivables
        myReceivables.forEach(function (item) {
            var record = new ProfitClass();
            record.ChargeTypeId = item.ChargeTypeId;
            record.ChargeTypeName = item.ChargeTypeName;
            record.ReceivableOpenedAmount = _this.Fixed(item.ReceivableOpenedAmount);
            record.ReceivableAcountedAmount = _this.Fixed(item.ReceivableAcountedAmount);
            record.ReceivableAmount = _this.Fixed(item.ReceivableOpenedAmount + item.ReceivableAcountedAmount);
            record.Profit = item.ReceivableOpenedAmount + item.ReceivableAcountedAmount;
            _this.ProfitsCollection.push(record);
        });
    };
    ProfitComponent.prototype.ComputeTotals = function () {
        var _this = this;
        var myTotalProfitText = "N/A";
        var myTotalPayablesText = "N/A";
        var myTotalReceivablesText = "N/A";
        var myProfit = null;
        var myEstimateProfit = null;
        var myDifference = null;
        if (this.ProfitsCollection.length > 0) {
            var myTotalProfit_1 = 0;
            var myTotalPayables_1 = 0;
            var myTotalReceivables_1 = 0;
            this.ProfitsCollection.forEach(function (item) {
                if (_this.isPayablesExists) {
                    if (item.PayableOpenedAmount != null) {
                        myTotalPayables_1 += item.PayableOpenedAmount;
                    }
                    if (item.PayableAcountedAmount != null) {
                        myTotalPayables_1 += item.PayableAcountedAmount;
                    }
                }
                if (_this.isReceivablesExists) {
                    if (item.ReceivableOpenedAmount != null) {
                        myTotalReceivables_1 += item.ReceivableOpenedAmount;
                    }
                    if (item.ReceivableAcountedAmount != null) {
                        myTotalReceivables_1 += item.ReceivableAcountedAmount;
                    }
                }
                if (item.Profit != null) {
                    myTotalProfit_1 += item.Profit;
                }
            });
            var myPipe = new NumbersPipe_1.NumbersPipe();
            if (this.isPayablesExists) {
                myTotalPayablesText = myPipe.transform(myTotalPayables_1, 'N2');
            }
            if (this.isReceivablesExists) {
                myTotalReceivablesText = myPipe.transform(myTotalReceivables_1, 'N2');
            }
            if (this.isPayablesExists || this.isReceivablesExists) {
                myProfit = myTotalProfit_1;
                myTotalProfitText = myPipe.transform(myTotalProfit_1, 'N2');
            }
        }
        this.TotalProfit = myProfit;
        this.TotalProfitText = myTotalProfitText;
        this.TotalPayablesText = myTotalPayablesText;
        this.TotalReceivablesText = myTotalReceivablesText;
        if (this.IsByLocalCurrency) {
            myEstimateProfit = this.EntityPM.EstimateProfitInLocalCurrency;
        }
        else {
            myEstimateProfit = this.EntityPM.EstimateProfitInProfitCurrency;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myProfit) && !Tools_1.AppTool.IsNullOrEmpty(myEstimateProfit)) {
            myDifference = myProfit - myEstimateProfit;
        }
        this.EstimateProfit = myEstimateProfit;
        this.Difference = myDifference;
    };
    ProfitComponent.prototype.Fixed = function (value) {
        return value == 0 ? null : value;
    };
    ProfitComponent.prototype.OnSelectCurrency = function (myCurrencyCode) {
        this.SelectedCurrencyCode = myCurrencyCode;
        if (myCurrencyCode == this.LocalCurrencyCode) {
            this.IsByLocalCurrency = true;
        }
        else {
            this.IsByLocalCurrency = false;
        }
        this.SetLabels();
        this.BuildProfitData();
    };
    ProfitComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ProfitComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ProfitComponent);
    return ProfitComponent;
}());
exports.ProfitComponent = ProfitComponent;
var ProfitClass = /** @class */ (function () {
    function ProfitClass() {
        this.ChargeTypeId = null;
        this.ChargeTypeName = null;
        this.ReceivableOpenedAmount = null;
        this.ReceivableAcountedAmount = null;
        this.PayableOpenedAmount = null;
        this.PayableAcountedAmount = null;
        this.ReceivableAmount = null;
        this.PayableAmount = null;
        this.Profit = null;
    }
    return ProfitClass;
}());
//# sourceMappingURL=ProfitComponent.js.map