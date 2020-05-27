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
Object.defineProperty(exports, "__esModule", { value: true });
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AirCostTariffLineData = /** @class */ (function (_super) {
    __extends(AirCostTariffLineData, _super);
    function AirCostTariffLineData(entity, FatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.FatherComponent = FatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "TariffLine";
        _this.IsNewEntity = false;
        _this.IsEditEnabled = false;
        _this.MinPriceComparingTextColor = null;
        _this.Step1ComparingTextColor = null;
        _this.Step2ComparingTextColor = null;
        _this.Step3ComparingTextColor = null;
        _this.Step4ComparingTextColor = null;
        _this.Step5ComparingTextColor = null;
        _this.Step6ComparingTextColor = null;
        _this.Step7ComparingTextColor = null;
        _this.Step8ComparingTextColor = null;
        _this.DefaultColor = "blue";
        _this.EntityPM = entity;
        _this.IsNewEntity = isNew;
        _this.IsEditEnabled = FatherComponent.IsDraftVersion;
        _this.SetUIProperties();
        return _this;
    }
    AirCostTariffLineData.prototype.SetCellsComparingText = function () {
        if (this.ComparedEntity != null) {
            var value1 = Tools_1.AppTool.IsNullOrZero(this.MinPrice) ? 0 : this.MinPrice;
            var value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.MinPrice) ? 0 : this.ComparedEntity.MinPrice;
            var minPriceComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(minPriceComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.MinPriceComparingPrice = Tools_1.AppTool.Round((minPriceComparingValue / value2) * 100, 2);
                this.MinPriceComparingTextColor = this.ComputeWarningPercentageColor(this.MinPriceComparingPrice);
            }
            else {
                this.MinPriceComparingPrice = null;
                this.MinPriceComparingTextColor = this.DefaultColor;
            }
            // step 1
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step1Price) ? 0 : this.Step1Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step1Price) ? 0 : this.ComparedEntity.Step1Price;
            var step1ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step1ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step1ComparingPrice = (step1ComparingValue / value2) * 100;
                this.Step1ComparingTextColor = this.ComputeWarningPercentageColor(this.Step1ComparingPrice);
            }
            else {
                this.Step1ComparingPrice = null;
                this.Step1ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step2Price) ? 0 : this.Step2Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step2Price) ? 0 : this.ComparedEntity.Step2Price;
            var step2ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step2ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step2ComparingPrice = (step2ComparingValue / value2) * 100;
                this.Step2ComparingTextColor = this.ComputeWarningPercentageColor(this.Step2ComparingPrice);
            }
            else {
                this.Step2ComparingPrice = null;
                this.Step2ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step3Price) ? 0 : this.Step3Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step3Price) ? 0 : this.ComparedEntity.Step3Price;
            var step3ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step3ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step3ComparingPrice = (step3ComparingValue / value2) * 100;
                this.Step3ComparingTextColor = this.ComputeWarningPercentageColor(this.Step3ComparingPrice);
            }
            else {
                this.Step3ComparingPrice = null;
                this.Step3ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step4Price) ? 0 : this.Step4Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step4Price) ? 0 : this.ComparedEntity.Step4Price;
            var step4ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step4ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step4ComparingPrice = (step4ComparingValue / value2) * 100;
                this.Step4ComparingTextColor = this.ComputeWarningPercentageColor(this.Step4ComparingPrice);
            }
            else {
                this.Step4ComparingPrice = null;
                this.Step4ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step5Price) ? 0 : this.Step5Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step5Price) ? 0 : this.ComparedEntity.Step5Price;
            var step5ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step5ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step5ComparingPrice = (step5ComparingValue / value2) * 100;
                this.Step5ComparingTextColor = this.ComputeWarningPercentageColor(this.Step5ComparingPrice);
            }
            else {
                this.Step5ComparingPrice = null;
                this.Step5ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step6Price) ? 0 : this.Step6Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step6Price) ? 0 : this.ComparedEntity.Step6Price;
            var step6ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step6ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step6ComparingPrice = (step6ComparingValue / value2) * 100;
                this.Step6ComparingTextColor = this.ComputeWarningPercentageColor(this.Step6ComparingPrice);
            }
            else {
                this.Step6ComparingPrice = null;
                this.Step6ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step7Price) ? 0 : this.Step7Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step7Price) ? 0 : this.ComparedEntity.Step7Price;
            var step7ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step7ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step7ComparingPrice = (step7ComparingValue / value2) * 100;
                this.Step7ComparingTextColor = this.ComputeWarningPercentageColor(this.Step7ComparingPrice);
            }
            else {
                this.Step7ComparingPrice = null;
                this.Step7ComparingTextColor = this.DefaultColor;
            }
            value1 = Tools_1.AppTool.IsNullOrZero(this.Step8Price) ? 0 : this.Step8Price;
            value2 = Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Step8Price) ? 0 : this.ComparedEntity.Step8Price;
            var step8ComparingValue = value1 - value2;
            if (!Tools_1.AppTool.IsNullOrZero(step8ComparingValue) && !Tools_1.AppTool.IsNullOrZero(value2)) {
                this.Step8ComparingPrice = (step8ComparingValue / value2) * 100;
                this.Step8ComparingTextColor = this.ComputeWarningPercentageColor(this.Step8ComparingPrice);
            }
            else {
                this.Step8ComparingPrice = null;
                this.Step8ComparingTextColor = this.DefaultColor;
            }
        }
    };
    AirCostTariffLineData.prototype.ComputeWarningPercentageColor = function (price) {
        var color = "blue";
        if (this.FatherComponent.WarningPercentage == null) {
            color = "blue";
        }
        else {
            var price_abs = Math.abs(price);
            if (price_abs > this.FatherComponent.WarningPercentage) {
                color = "red";
            }
        }
        return color;
    };
    Object.defineProperty(AirCostTariffLineData.prototype, "HasErrors", {
        get: function () {
            return this.EntityPM.HasErrors;
        },
        set: function (value) {
            if (this.EntityPM.HasErrors != value) {
                this.EntityPM.HasErrors = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "ErrorText", {
        get: function () {
            return this.EntityPM.ErrorText;
        },
        set: function (value) {
            if (this.EntityPM.ErrorText != value) {
                this.EntityPM.ErrorText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AirCostTariffLineData.prototype.CheckIfLineHasError = function () {
        if (this.ErrorText != 'Line is a duplicate') {
            var error = false;
            var errorText;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Port with code " + this.EntityPM.OriginPortText + " not found";
                }
                else {
                    errorText = errorText + ", Port with code " + this.EntityPM.OriginPortText + " not found";
                }
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Missing Origin Port";
                }
                else {
                    errorText = errorText + ", Missing Origin Port";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
                }
                else {
                    errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found";
                }
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Missing Destination Port";
                }
                else {
                    errorText = errorText + ", Missing Destination Port";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MinPriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Min price format is invalid";
                }
                else {
                    errorText = errorText + ", Min price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step1PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 1 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 1 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step2PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 2 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 2 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step3PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 3 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 3 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step4PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 4 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 4 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step5PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 5 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 5 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step6PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 6 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 6 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step7PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 7 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 7 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Step8PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Step 8 price format is invalid";
                }
                else {
                    errorText = errorText + ", Step 8 price format is invalid";
                }
            }
            this.HasErrors = error;
            this.ErrorText = errorText;
        }
    };
    AirCostTariffLineData.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OriginPortId));
        this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DestinationPortId));
    };
    Object.defineProperty(AirCostTariffLineData.prototype, "OriginPortId", {
        // Origin Port
        get: function () {
            return this.EntityPM.OriginPortId;
        },
        set: function (value) {
            if (this.EntityPM.OriginPortId != value) {
                this.EntityPM.OriginPortId = value;
                this.SetUIProperties();
                this.CheckIfLineHasError();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "OriginPortCode", {
        get: function () {
            return this.EntityPM.OriginPortCode;
        },
        set: function (value) {
            if (this.EntityPM.OriginPortCode != value) {
                this.EntityPM.OriginPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "OriginPort", {
        get: function () { return this.originPort; },
        set: function (value) {
            if (this.originPort != value) {
                this.originPort = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.OriginPortCode = value.Code;
            }
            else {
                this.OriginPortCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "OriginPortValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCode)) {
                return this.EntityPM.OriginPortCode;
            }
            else {
                return this.EntityPM.OriginPortText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "OriginPortColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "DestinationPortId", {
        // Destination Port
        get: function () {
            return this.EntityPM.DestinationPortId;
        },
        set: function (value) {
            if (this.EntityPM.DestinationPortId != value) {
                this.EntityPM.DestinationPortId = value;
                this.SetUIProperties();
                this.CheckIfLineHasError();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "DestinationPortCode", {
        get: function () {
            return this.EntityPM.DestinationPortCode;
        },
        set: function (value) {
            if (this.EntityPM.DestinationPortCode != value) {
                this.EntityPM.DestinationPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "DestinationPort", {
        get: function () { return this.destinationPort; },
        set: function (value) {
            if (this.destinationPort != value) {
                this.destinationPort = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.DestinationPortCode = value.Code;
            }
            else {
                this.DestinationPortCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "DestinationPortValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCode)) {
                return this.EntityPM.DestinationPortCode;
            }
            else {
                return this.EntityPM.DestinationPortText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "DestinationPortColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Notes", {
        get: function () {
            return this.EntityPM.Notes;
        },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "MinPrice", {
        // Min Price
        get: function () {
            return this.EntityPM.MinPrice;
        },
        set: function (value) {
            if (this.EntityPM.MinPrice != value) {
                this.EntityPM.MinPrice = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "MinPriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.MinPrice, "N3");
            }
            else {
                return this.EntityPM.MinPriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "MinPriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.MinPrice)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step1Price", {
        // Step 1
        get: function () {
            return this.EntityPM.Step1Price;
        },
        set: function (value) {
            if (this.EntityPM.Step1Price != value) {
                this.EntityPM.Step1Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step1PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step1Price, "N3");
            }
            else {
                return this.EntityPM.Step1PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step1PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step1Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step2Price", {
        // Step 2
        get: function () {
            return this.EntityPM.Step2Price;
        },
        set: function (value) {
            if (this.EntityPM.Step2Price != value) {
                this.EntityPM.Step2Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step2PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step2Price, "N3");
            }
            else {
                return this.EntityPM.Step2PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step2PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step2Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step3Price", {
        // Step 3
        get: function () {
            return this.EntityPM.Step3Price;
        },
        set: function (value) {
            if (this.EntityPM.Step3Price != value) {
                this.EntityPM.Step3Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step3PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step3Price, "N3");
            }
            else {
                return this.EntityPM.Step3PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step3PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step3Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step4Price", {
        // Step 4
        get: function () {
            return this.EntityPM.Step4Price;
        },
        set: function (value) {
            if (this.EntityPM.Step4Price != value) {
                this.EntityPM.Step4Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step4PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step4Price, "N3");
            }
            else {
                return this.EntityPM.Step4PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step4PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step4Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step5Price", {
        // Step 5
        get: function () {
            return this.EntityPM.Step5Price;
        },
        set: function (value) {
            if (this.EntityPM.Step5Price != value) {
                this.EntityPM.Step5Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step5PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step5Price, "N3");
            }
            else {
                return this.EntityPM.Step5PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step5PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step5Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step6Price", {
        // Step 6
        get: function () {
            return this.EntityPM.Step6Price;
        },
        set: function (value) {
            if (this.EntityPM.Step6Price != value) {
                this.EntityPM.Step6Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step6PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step6Price, "N3");
            }
            else {
                return this.EntityPM.Step6PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step6PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step6Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step7Price", {
        // Step 7
        get: function () {
            return this.EntityPM.Step7Price;
        },
        set: function (value) {
            if (this.EntityPM.Step7Price != value) {
                this.EntityPM.Step7Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step7PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step7Price, "N3");
            }
            else {
                return this.EntityPM.Step7PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step7PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step7Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step8Price", {
        // Step 8
        get: function () {
            return this.EntityPM.Step8Price;
        },
        set: function (value) {
            if (this.EntityPM.Step8Price != value) {
                this.EntityPM.Step8Price = value;
                this.CheckIfLineHasError();
                this.SetCellsComparingText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step8PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Step8Price, "N3");
            }
            else {
                return this.EntityPM.Step8PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirCostTariffLineData.prototype, "Step8PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Step8Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    return AirCostTariffLineData;
}(BaseComponent_1.BaseComponent));
exports.AirCostTariffLineData = AirCostTariffLineData;
var AirSurchargeTariffLineData = /** @class */ (function (_super) {
    __extends(AirSurchargeTariffLineData, _super);
    function AirSurchargeTariffLineData(entity, FatherComponent, isNew) {
        if (isNew === void 0) { isNew = false; }
        var _this = _super.call(this) || this;
        _this.FatherComponent = FatherComponent;
        _this.DataContext = _this;
        _this.ObjectTableName = "TariffLine";
        _this.IsNewEntity = false;
        _this.IsEditEnabled = false;
        _this.Surcharge1ComparingTextColor = null;
        _this.Surcharge2ComparingTextColor = null;
        _this.Surcharge3ComparingTextColor = null;
        _this.Surcharge4ComparingTextColor = null;
        _this.Surcharge5ComparingTextColor = null;
        _this.Surcharge6ComparingTextColor = null;
        _this.Surcharge7ComparingTextColor = null;
        _this.Surcharge8ComparingTextColor = null;
        _this.Surcharge9ComparingTextColor = null;
        _this.Surcharge10ComparingTextColor = null;
        _this.DefaultColor = "blue";
        _this.EntityPM = entity;
        _this.IsNewEntity = isNew;
        _this.initialIndex = entity.Index;
        _this.IsEditEnabled = FatherComponent.IsDraftVersion;
        _this.SetUIProperties();
        return _this;
    }
    AirSurchargeTariffLineData.prototype.CheckIfLineHasError = function () {
        if (this.ErrorText != 'Line is a duplicate') {
            var error = false;
            var errorText;
            if (!this.IsFromAllOtherPorts) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                    error = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Port with code " + this.EntityPM.OriginPortText + " not found";
                    }
                    else {
                        errorText = errorText + ", Port with code " + this.EntityPM.OriginPortText + " not found";
                    }
                }
                else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                    error = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Missing Origin Port";
                    }
                    else {
                        errorText = errorText + ", Missing Origin Port";
                    }
                }
            }
            if (!this.IsToAllOtherPorts) {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                    error = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Port with code " + this.EntityPM.DestinationPortText + " not found";
                    }
                    else {
                        errorText = errorText + ", Port with code " + this.EntityPM.DestinationPortText + " not found";
                    }
                }
                else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortText) && Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                    error = true;
                    if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                        errorText = "Missing Destination Port";
                    }
                    else {
                        errorText = errorText + ", Missing Destination Port";
                    }
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge1PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 1 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 1 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge2PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 2 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 2 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge3PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 3 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 3 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge4PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 4 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 4 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge5PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 5 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 5 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge6PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 6 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 6 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge7PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 7 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 7 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge8PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 8 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 8 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge9PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 9 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 9 price format is invalid";
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Surcharge10PriceText) && Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
                error = true;
                if (Tools_1.AppTool.IsNullOrEmpty(errorText)) {
                    errorText = "Surcharge 10 price format is invalid";
                }
                else {
                    errorText = errorText + ", Surcharge 10 price format is invalid";
                }
            }
            this.HasErrors = error;
            this.ErrorText = errorText;
        }
    };
    AirSurchargeTariffLineData.prototype.SetUIProperties = function () {
        this.SetUIProperties_From();
        this.SetUIProperties_To();
    };
    AirSurchargeTariffLineData.prototype.SetUIProperties_From = function () {
        if (this.IsFromAllOtherPorts) {
            this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("OriginPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OriginPortId));
            this.UIProperties.SetEnabled("OriginPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsFromAllOtherPorts", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.OriginPortId));
        }
    };
    AirSurchargeTariffLineData.prototype.SetUIProperties_To = function () {
        if (this.IsToAllOtherPorts) {
            this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetRequired("DestinationPortId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DestinationPortId));
            this.UIProperties.SetEnabled("DestinationPortId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("IsToAllOtherPorts", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DestinationPortId));
        }
    };
    AirSurchargeTariffLineData.prototype.SetCellsComparingText = function () {
        this.CompareSurcharge1Price();
        this.CompareSurcharge2Price();
        this.CompareSurcharge3Price();
        this.CompareSurcharge4Price();
        this.CompareSurcharge5Price();
        this.CompareSurcharge6Price();
        this.CompareSurcharge7Price();
        this.CompareSurcharge8Price();
        this.CompareSurcharge9Price();
        this.CompareSurcharge10Price();
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge1Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge1ComparingPrice = null;
            this.Surcharge1ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge1Price != null) {
                var surcharge1ComparingValue = this.Surcharge1Price - this.ComparedEntity.Surcharge1Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge1ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge1Price)) {
                    this.Surcharge1ComparingPrice = (surcharge1ComparingValue / this.ComparedEntity.Surcharge1Price) * 100;
                    this.Surcharge1ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge1ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge2Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge2ComparingPrice = null;
            this.Surcharge2ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge2Price != null) {
                var surcharge2ComparingValue = this.Surcharge2Price - this.ComparedEntity.Surcharge2Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge2ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge2Price)) {
                    this.Surcharge2ComparingPrice = (surcharge2ComparingValue / this.ComparedEntity.Surcharge2Price) * 100;
                    this.Surcharge2ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge2ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge3Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge3ComparingPrice = null;
            this.Surcharge3ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge3Price != null) {
                var surcharge3ComparingValue = this.Surcharge3Price - this.ComparedEntity.Surcharge3Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge3ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge3Price)) {
                    this.Surcharge3ComparingPrice = (surcharge3ComparingValue / this.ComparedEntity.Surcharge3Price) * 100;
                    this.Surcharge3ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge3ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge4Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge4ComparingPrice = null;
            this.Surcharge4ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge4Price != null) {
                var surcharge4ComparingValue = this.Surcharge4Price - this.ComparedEntity.Surcharge4Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge4ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge4Price)) {
                    this.Surcharge4ComparingPrice = (surcharge4ComparingValue / this.ComparedEntity.Surcharge4Price) * 100;
                    this.Surcharge4ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge4ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge5Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge5ComparingPrice = null;
            this.Surcharge5ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge5Price != null) {
                var surcharge5ComparingValue = this.Surcharge5Price - this.ComparedEntity.Surcharge5Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge5ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge5Price)) {
                    this.Surcharge5ComparingPrice = (surcharge5ComparingValue / this.ComparedEntity.Surcharge5Price) * 100;
                    this.Surcharge5ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge5ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge6Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge6ComparingPrice = null;
            this.Surcharge6ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge6Price != null) {
                var surcharge6ComparingValue = this.Surcharge6Price - this.ComparedEntity.Surcharge6Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge6ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge6Price)) {
                    this.Surcharge6ComparingPrice = (surcharge6ComparingValue / this.ComparedEntity.Surcharge6Price) * 100;
                    this.Surcharge6ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge6ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge7Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge7ComparingPrice = null;
            this.Surcharge7ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge7Price != null) {
                var surcharge7ComparingValue = this.Surcharge7Price - this.ComparedEntity.Surcharge7Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge7ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge7Price)) {
                    this.Surcharge7ComparingPrice = (surcharge7ComparingValue / this.ComparedEntity.Surcharge7Price) * 100;
                    this.Surcharge7ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge7ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge8Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge8ComparingPrice = null;
            this.Surcharge8ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge8Price != null) {
                var surcharge8ComparingValue = this.Surcharge8Price - this.ComparedEntity.Surcharge8Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge8ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge8Price)) {
                    this.Surcharge8ComparingPrice = (surcharge8ComparingValue / this.ComparedEntity.Surcharge8Price) * 100;
                    this.Surcharge8ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge8ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge9Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge9ComparingPrice = null;
            this.Surcharge9ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge9Price != null) {
                var surcharge9ComparingValue = this.Surcharge9Price - this.ComparedEntity.Surcharge9Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge9ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge9Price)) {
                    this.Surcharge9ComparingPrice = (surcharge9ComparingValue / this.ComparedEntity.Surcharge9Price) * 100;
                    this.Surcharge9ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge9ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.CompareSurcharge10Price = function () {
        if (this.ComparedEntity != null) {
            this.Surcharge10ComparingPrice = null;
            this.Surcharge10ComparingTextColor = this.DefaultColor;
            if (this.ComparedEntity.Surcharge10Price != null) {
                var surcharge10ComparingValue = this.Surcharge10Price - this.ComparedEntity.Surcharge10Price;
                if (!Tools_1.AppTool.IsNullOrZero(surcharge10ComparingValue) && !Tools_1.AppTool.IsNullOrZero(this.ComparedEntity.Surcharge10Price)) {
                    this.Surcharge10ComparingPrice = (surcharge10ComparingValue / this.ComparedEntity.Surcharge10Price) * 100;
                    this.Surcharge10ComparingTextColor = this.ComputeWarningPercentageColor(this.Surcharge10ComparingPrice);
                }
            }
        }
    };
    AirSurchargeTariffLineData.prototype.ComputeWarningPercentageColor = function (price) {
        var color = "blue";
        if (this.FatherComponent.WarningPercentage == null) {
            color = "blue";
        }
        else {
            var price_abs = Math.abs(price);
            if (price_abs > this.FatherComponent.WarningPercentage) {
                color = "red";
            }
        }
        return color;
    };
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "HasErrors", {
        get: function () {
            return this.EntityPM.HasErrors;
        },
        set: function (value) {
            if (this.EntityPM.HasErrors != value) {
                this.EntityPM.HasErrors = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "ErrorText", {
        get: function () {
            return this.EntityPM.ErrorText;
        },
        set: function (value) {
            if (this.EntityPM.ErrorText != value) {
                this.EntityPM.ErrorText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "OriginPortId", {
        // Origin Port
        get: function () {
            return this.EntityPM.OriginPortId;
        },
        set: function (value) {
            if (this.EntityPM.OriginPortId != value) {
                this.EntityPM.OriginPortId = value;
                this.SetUIProperties_From();
                this.CheckIfLineHasError();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "OriginPortCode", {
        get: function () {
            return this.EntityPM.OriginPortCode;
        },
        set: function (value) {
            if (this.EntityPM.OriginPortCode != value) {
                this.EntityPM.OriginPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "OriginPort", {
        get: function () { return this.originPort; },
        set: function (value) {
            if (this.originPort != value) {
                this.originPort = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.OriginPortCode = value.Code;
            }
            else {
                this.OriginPortCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "OriginPortValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortCode)) {
                return this.EntityPM.OriginPortCode;
            }
            else {
                return this.EntityPM.OriginPortText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "OriginPortColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OriginPortId)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "DestinationPortId", {
        // Destination Port
        get: function () {
            return this.EntityPM.DestinationPortId;
        },
        set: function (value) {
            if (this.EntityPM.DestinationPortId != value) {
                this.EntityPM.DestinationPortId = value;
                this.SetUIProperties_To();
                this.CheckIfLineHasError();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "DestinationPortCode", {
        get: function () {
            return this.EntityPM.DestinationPortCode;
        },
        set: function (value) {
            if (this.EntityPM.DestinationPortCode != value) {
                this.EntityPM.DestinationPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "DestinationPort", {
        get: function () { return this.destinationPort; },
        set: function (value) {
            if (this.destinationPort != value) {
                this.destinationPort = value;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.DestinationPortCode = value.Code;
            }
            else {
                this.DestinationPortCode = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "DestinationPortValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortCode)) {
                return this.EntityPM.DestinationPortCode;
            }
            else {
                return this.EntityPM.DestinationPortText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "DestinationPortColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DestinationPortId)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "StartDate", {
        get: function () { return this.EntityPM.StartDate; },
        set: function (value) {
            if (this.EntityPM.StartDate != value) {
                this.EntityPM.StartDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "ExpirationDate", {
        get: function () { return this.EntityPM.ExpirationDate; },
        set: function (value) {
            if (this.EntityPM.ExpirationDate != value) {
                this.EntityPM.ExpirationDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Notes", {
        get: function () {
            return this.EntityPM.Notes;
        },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge1Price", {
        // Surcharge 1
        get: function () {
            return this.EntityPM.Surcharge1Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge1Price != value) {
                this.EntityPM.Surcharge1Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge1Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge1PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge1Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge1PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge1PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge1Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge2Price", {
        // Surcharge 2
        get: function () {
            return this.EntityPM.Surcharge2Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge2Price != value) {
                this.EntityPM.Surcharge2Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge2Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge2PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge2Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge2PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge2PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge2Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge3Price", {
        // Surcharge 3
        get: function () {
            return this.EntityPM.Surcharge3Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge3Price != value) {
                this.EntityPM.Surcharge3Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge3Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge3PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge3Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge3PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge3PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge3Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge4Price", {
        // Surcharge 4
        get: function () {
            return this.EntityPM.Surcharge4Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge4Price != value) {
                this.EntityPM.Surcharge4Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge4Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge4PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge4Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge4PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge4PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge4Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge5Price", {
        // Surcharge 5
        get: function () {
            return this.EntityPM.Surcharge5Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge5Price != value) {
                this.EntityPM.Surcharge5Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge5Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge5PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge5Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge5PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge5PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge5Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge6Price", {
        // Surcharge 6
        get: function () {
            return this.EntityPM.Surcharge6Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge6Price != value) {
                this.EntityPM.Surcharge6Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge6Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge6PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge6Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge6PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge6PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge6Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge7Price", {
        // Surcharge 7
        get: function () {
            return this.EntityPM.Surcharge7Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge7Price != value) {
                this.EntityPM.Surcharge7Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge7Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge7PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge7Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge7PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge7PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge7Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge8Price", {
        // Surcharge 8
        get: function () {
            return this.EntityPM.Surcharge8Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge8Price != value) {
                this.EntityPM.Surcharge8Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge8Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge8PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge8Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge8PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge8PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge9Price", {
        // Surcharge 9
        get: function () {
            return this.EntityPM.Surcharge9Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge9Price != value) {
                this.EntityPM.Surcharge9Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge9Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge9PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge8Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge9Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge9PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge9PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge9Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge10Price", {
        // Surcharge 10
        get: function () {
            return this.EntityPM.Surcharge10Price;
        },
        set: function (value) {
            if (this.EntityPM.Surcharge10Price != value) {
                this.EntityPM.Surcharge10Price = value;
                this.CheckIfLineHasError();
                this.CompareSurcharge10Price();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge10PriceValue", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
                return Tools_1.FormatTool.FormatNumber(this.EntityPM.Surcharge10Price, "N3");
            }
            else {
                return this.EntityPM.Surcharge10PriceText;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "Surcharge10PriceColor", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrZero(this.EntityPM.Surcharge10Price)) {
                return Tools_1.FontTool.Black;
            }
            else {
                return Tools_1.FontTool.Red;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "IsFromAllOtherPorts", {
        get: function () { return this.EntityPM.IsFromAllOtherPorts; },
        set: function (value) {
            if (this.EntityPM.IsFromAllOtherPorts != value) {
                this.EntityPM.IsFromAllOtherPorts = value;
                if (value) {
                    this.OriginPortId = null;
                    this.EntityPM.OriginPortText = null;
                }
                this.SetUIProperties_From();
                this.CheckIfLineHasError();
                this.ComputeIndex();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirSurchargeTariffLineData.prototype, "IsToAllOtherPorts", {
        get: function () { return this.EntityPM.IsToAllOtherPorts; },
        set: function (value) {
            if (this.EntityPM.IsToAllOtherPorts != value) {
                this.EntityPM.IsToAllOtherPorts = value;
                if (value) {
                    this.DestinationPortId = null;
                    this.EntityPM.DestinationPortText = null;
                }
                this.SetUIProperties_To();
                this.CheckIfLineHasError();
                this.ComputeIndex();
            }
        },
        enumerable: true,
        configurable: true
    });
    AirSurchargeTariffLineData.prototype.ComputeIndex = function () {
        if (this.IsFromAllOtherPorts || this.IsToAllOtherPorts) {
            this.EntityPM.Index = -1;
        }
        else {
            if (this.IsNewEntity) {
                this.EntityPM.Index = this.initialIndex;
            }
            else {
                if (this.initialIndex == -1) {
                    this.EntityPM.Index = 0;
                }
                else {
                    this.EntityPM.Index = this.initialIndex;
                }
            }
        }
    };
    return AirSurchargeTariffLineData;
}(BaseComponent_1.BaseComponent));
exports.AirSurchargeTariffLineData = AirSurchargeTariffLineData;
//# sourceMappingURL=TariffLineData.js.map