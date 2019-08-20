"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Infrastructure/Tools");
var QuoteStageListService_1 = require("./Services/StandardLists/QuoteStageListService");
var QuoteTool = /** @class */ (function () {
    function QuoteTool() {
    }
    QuoteTool.IsQuoteEditEnabled = function (entityPM) {
        var myResult = true;
        var d = "ayman";
        if (entityPM != null) {
            if (entityPM.IsClosed) {
                myResult = false;
            }
            else if (entityPM.IsCancelled) {
                myResult = false;
            }
            else if (entityPM.IsQuoteDataExternal && entityPM.IsQuoteDocumentExternal) {
                myResult = false;
            }
            else {
                var myService = new QuoteStageListService_1.QuoteStageListService();
                myService.getAllFromCache().subscribe(function (resp) {
                    if (!resp.HasError) {
                        var allStages = resp.Result;
                        var mySentStageId = "";
                        var mySentStage = allStages.filter(function (d) { return d.Code == "QTST"; })[0];
                        if (mySentStage != null) {
                            mySentStageId = mySentStage.Id;
                        }
                        if (entityPM.StageId == mySentStageId) {
                            myResult = false;
                        }
                    }
                });
            }
        }
        return myResult;
    };
    QuoteTool.IsLCLQuote = function (entityPM) {
        var myResult = false;
        if (entityPM != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.TransportModeId) && entityPM.TransportModeId.toUpperCase() == "A") {
                myResult = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.TransportModeId) && entityPM.TransportModeId.toUpperCase() == "O" && entityPM.ShipmentTypeId.toUpperCase() == "LCLD") {
                myResult = true;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.TransportModeId) && entityPM.TransportModeId.toUpperCase() == "I" && entityPM.ShipmentTypeId.toUpperCase() == "LTL") {
                myResult = true;
            }
        }
        return myResult;
    };
    QuoteTool.GetFromPortTextCode = function (TransportModeId) {
        var myResult = null;
        switch (TransportModeId) {
            case "A": {
                myResult = "Quote.S.NewQuote.Gateway";
                break;
            }
            case "O": {
                myResult = "Quote.S.NewQuote.LoadingPort";
                break;
            }
            case "I": {
                myResult = "Quote.S.NewQuote.From";
                break;
            }
            default: {
                myResult = "Quote.S.NewQuote.From";
                break;
            }
        }
        return myResult;
    };
    QuoteTool.GetToPortTextCode = function (TransportModeId) {
        var myResult = null;
        switch (TransportModeId) {
            case "A": {
                myResult = "Quote.S.NewQuote.Destination";
                break;
            }
            case "O": {
                myResult = "Quote.S.NewQuote.DischargePort";
                break;
            }
            case "I": {
                myResult = "Quote.S.NewQuote.To";
                break;
            }
            default: {
                myResult = "Quote.S.NewQuote.To";
                break;
            }
        }
        return myResult;
    };
    QuoteTool.OnQuoteQuantitiesChanged = function (entityPM) {
        if (entityPM) {
            if (entityPM.QuoteTypeCode == "A") {
                var isLCL = QuoteTool.IsLCLQuote(entityPM);
                if (isLCL) {
                    if (entityPM.QuoteCharges.filter(function (d) { return d.SaleUnitPrice != null || d.CostUnitPrice != null; }).length == 0) {
                        entityPM.QuoteCharges.forEach(function (item) {
                            var myCostQuantity = null;
                            switch (item.CostMeasurementCode) {
                                case "GRWT": {
                                    myCostQuantity = entityPM.GrossWeight;
                                    break;
                                }
                                case "CHWT": {
                                    myCostQuantity = entityPM.ChargeableWeight;
                                    break;
                                }
                                case "VOLU": {
                                    myCostQuantity = entityPM.Volume;
                                    break;
                                }
                                case "BTEU": {
                                    myCostQuantity = entityPM.TEU;
                                    break;
                                }
                                case "FIXD": {
                                    myCostQuantity = 1;
                                    break;
                                }
                                case "GWTN": {
                                    myCostQuantity = entityPM.GrossWeightPerTon;
                                    break;
                                }
                                case "PRVL": {
                                    myCostQuantity = entityPM.ValueOfGoods;
                                    break;
                                }
                                case "PRFR": {
                                    myCostQuantity = Tools_1.ArrayTool.Sum(entityPM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT"; }), "CostTotalAmount");
                                    break;
                                }
                                case "QTY": {
                                    myCostQuantity = entityPM.NumberOfPackages;
                                    break;
                                }
                                case "CWKG": {
                                    myCostQuantity = entityPM.ChargeableWeightInKG;
                                    break;
                                }
                                case "GWKG": {
                                    myCostQuantity = entityPM.GrossWeightInKG;
                                    break;
                                }
                                case "VCBM": {
                                    myCostQuantity = entityPM.VolumeInCBM;
                                    break;
                                }
                                default: {
                                    break;
                                }
                            }
                            var mySaleQuantity = null;
                            switch (item.SaleMeasurementCode) {
                                case "GRWT": {
                                    mySaleQuantity = entityPM.GrossWeight;
                                    break;
                                }
                                case "CHWT": {
                                    mySaleQuantity = entityPM.ChargeableWeight;
                                    break;
                                }
                                case "VOLU": {
                                    mySaleQuantity = entityPM.Volume;
                                    break;
                                }
                                case "BTEU": {
                                    mySaleQuantity = entityPM.TEU;
                                    break;
                                }
                                case "FIXD": {
                                    mySaleQuantity = 1;
                                    break;
                                }
                                case "GWTN": {
                                    mySaleQuantity = entityPM.GrossWeightPerTon;
                                    break;
                                }
                                case "PRVL": {
                                    mySaleQuantity = entityPM.ValueOfGoods;
                                    break;
                                }
                                case "PRFR": {
                                    mySaleQuantity = Tools_1.ArrayTool.Sum(entityPM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT"; }), "SaleTotalAmount");
                                    break;
                                }
                                case "QTY": {
                                    mySaleQuantity = entityPM.NumberOfPackages;
                                    break;
                                }
                                case "CWKG": {
                                    mySaleQuantity = entityPM.ChargeableWeightInKG;
                                    break;
                                }
                                case "GWKG": {
                                    mySaleQuantity = entityPM.GrossWeightInKG;
                                    break;
                                }
                                case "VCBM": {
                                    mySaleQuantity = entityPM.VolumeInCBM;
                                    break;
                                }
                                default: {
                                    break;
                                }
                            }
                            if (item.CostQuantity != myCostQuantity) {
                                item.CostQuantity = Tools_1.AppTool.Round(myCostQuantity, 2);
                            }
                            if (item.SaleQuantity != mySaleQuantity) {
                                item.SaleQuantity = Tools_1.AppTool.Round(mySaleQuantity, 2);
                            }
                            // Ayman
                            // No need to Compute anything else (Amounts = Quantity * UnitPrice)
                            // this Code is applied only when there is NO unit price
                        });
                    }
                }
                else {
                }
            }
        }
    };
    return QuoteTool;
}());
exports.QuoteTool = QuoteTool;
//# sourceMappingURL=Tools.js.map