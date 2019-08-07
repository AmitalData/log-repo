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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var QuoteUtilities_1 = require("../../../../Quote/Utilities/QuoteUtilities");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_2 = require("../../../../Quote/Tools");
var OrdersTabComponent = /** @class */ (function (_super) {
    __extends(OrdersTabComponent, _super);
    function OrdersTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Quote";
        _this.ChargeableWeightUnitCodeLabel = null;
        _this.TabSelectedEvent = null;
        _this.SaveCompletedEvent = null;
        _this.LoadCompletedEvent = null;
        _this.PropertyChangedEvent = null;
        _this.Retries = 0;
        _this.IsLCLQuote = false;
        _this.IsQuoteEditEnabled = true;
        _this.IsDimFactorVisible = false;
        _this.DimensionsDependencyProperty1 = null;
        _this.DimensionsDependencyProperty1IsList = false;
        // Measurment Units
        _this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Details.MeasurmentsSettings");
        _this.IsMeasurmentsHidden = true;
        _this.EntityPM = _this.entityArgs.EntityPM;
        _this.Listen();
        _this.ListenPropertyChanged();
        return _this;
    }
    OrdersTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.RunComponent();
            this.SetUIProperties();
            this.SetLabels();
        }
    };
    OrdersTabComponent.prototype.SetLabels = function () {
        if (this.EntityPM.TransportModeId == "A") {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.ChargeableWeightUnitCode");
        }
        else {
            this.ChargeableWeightUnitCodeLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.F.WtMsrUnitCode.Short");
        }
    };
    OrdersTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.ListenPropertyChanged();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.ListenPropertyChanged();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "QTOR") {
                    _this.SetUIProperties_DimFactor();
                    _this.SetUIProperties_DimensionsUnitCode();
                }
            });
        }
    };
    OrdersTabComponent.prototype.ListenPropertyChanged = function () {
        var _this = this;
        if (this.PropertyChangedEvent) {
            Tools_1.AppTool.KillEventEmitter(this.PropertyChangedEvent);
            this.PropertyChangedEvent = null;
        }
        this.PropertyChangedEvent = this.EntityPM.PropertyChanged.subscribe(function (s) {
            if (s) {
                if (s.PropertyName == "ValueOfGoods") {
                    Tools_2.QuoteTool.OnQuoteQuantitiesChanged(_this.EntityPM);
                }
            }
        });
    };
    OrdersTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.PropertyChangedEvent);
    };
    OrdersTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    OrdersTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    OrdersTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, "Quote.GeneralTabScreen");
        });
    };
    OrdersTabComponent.prototype.SetUIProperties = function () {
        this.IsLCLQuote = QuoteUtilities_1.QuoteUtilities.IsLCLQuote(this.EntityPM);
        this.IsQuoteEditEnabled = QuoteUtilities_1.QuoteUtilities.IsQuoteEditEnabled(this.EntityPM);
        this.SetUIProperties_DimFactor();
        this.SetUIProperties_EntityClosed();
        this.SetUIProperties_AutomaticallyClosed();
        this.SetUIProperties_DimensionsUnitCode();
    };
    OrdersTabComponent.prototype.SetUIProperties_DimFactor = function () {
        var isDimFactorVisibile = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DimensionsUnitCode)) {
            if (this.DimensionsUnitCode.toUpperCase() == "INC") {
                isDimFactorVisibile = true;
            }
        }
        this.IsDimFactorVisible = isDimFactorVisibile;
        this.UIProperties.SetVisibility("DimFactor", this.ObjectTableName, isDimFactorVisibile);
    };
    OrdersTabComponent.prototype.SetUIProperties_EntityClosed = function () {
        this.UIProperties.SetEnabled("IncotermId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("MoveTypeId", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ExpirationDays", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ExpirationDate", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("IsAutomaticallyClosed", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("AutomaticallyCloseDate", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("AutomaticallyCloseDays", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("ChargeableWeightUnitCode", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("Ratio", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("DimFactor", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("TransitTime", this.ObjectTableName, this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("DepartureFrequency", this.ObjectTableName, this.IsQuoteEditEnabled);
    };
    OrdersTabComponent.prototype.SetUIProperties_AutomaticallyClosed = function () {
        this.UIProperties.SetEnabled("AutomaticallyCloseDate", this.ObjectTableName, this.IsAutomaticallyClosed && this.IsQuoteEditEnabled);
        this.UIProperties.SetEnabled("AutomaticallyCloseDays", this.ObjectTableName, this.IsAutomaticallyClosed && this.IsQuoteEditEnabled);
    };
    OrdersTabComponent.prototype.SetUIProperties_DimensionsUnitCode = function () {
        var isFieldEnabled = false;
        if (this.IsQuoteEditEnabled && this.VolumeUnitCode == "CBF") {
            isFieldEnabled = true;
        }
        if (this.VolumeUnitCode == "CBF") {
            this.DimensionsDependencyProperty1 = "Ft,Inc";
            this.DimensionsDependencyProperty1IsList = true;
        }
        else {
            this.DimensionsDependencyProperty1 = null;
            this.DimensionsDependencyProperty1IsList = false;
        }
        this.UIProperties.SetEnabled("DimensionsUnitCode", this.ObjectTableName, isFieldEnabled);
    };
    OrdersTabComponent.prototype.MeasurmentsSettingsClicked = function () {
        this.IsMeasurmentsHidden = !this.IsMeasurmentsHidden;
        if (this.IsMeasurmentsHidden) {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Details.MeasurmentsSettings");
        }
        else {
            this.MeasurmentsButtonToolTip = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.B.Details.HideMeasurmentsSettings");
        }
    };
    Object.defineProperty(OrdersTabComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeightUnitCode != newValue) {
                this.EntityPM.GrossWeightUnitCode = newValue;
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "ChargeableWeightUnitCode", {
        get: function () { return this.EntityPM.ChargeableWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.ChargeableWeightUnitCode != newValue) {
                this.EntityPM.ChargeableWeightUnitCode = newValue;
                this.ComputeDimFactor();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "DimensionsUnitCode", {
        get: function () { return this.EntityPM.DimensionsUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.DimensionsUnitCode != newValue) {
                this.EntityPM.DimensionsUnitCode = newValue;
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "VolumeUnitCode", {
        get: function () { return this.EntityPM.VolumeUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.VolumeUnitCode != newValue) {
                this.EntityPM.VolumeUnitCode = newValue;
                this.EntityPM.DimensionsUnitCode = Tools_1.AppTool.GetDimentionsCodeFromVolumeCode(newValue);
                this.ComputeDimFactor();
                this.SetUIProperties_DimFactor();
                this.SetUIProperties_DimensionsUnitCode();
                this.OnMeasurmentsSettingsChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "Ratio", {
        get: function () { return this.EntityPM.Ratio; },
        set: function (newValue) {
            if (this.EntityPM.Ratio != newValue) {
                this.EntityPM.Ratio = newValue;
                this.ComputeDimFactor();
                QuoteUtilities_1.QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "DimFactor", {
        get: function () { return this.EntityPM.DimFactor; },
        set: function (newValue) {
            if (this.EntityPM.DimFactor != newValue) {
                this.EntityPM.DimFactor = newValue;
                this.EntityPM.Ratio = Tools_1.AppTool.GetRatioFromDimFactor(this.DimFactor, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
                QuoteUtilities_1.QuoteUtilities.OnQuoteRatioChanged(this.EntityPM);
            }
        },
        enumerable: true,
        configurable: true
    });
    OrdersTabComponent.prototype.ComputeDimFactor = function () {
        this.EntityPM.DimFactor = Tools_1.AppTool.GetDimFactorFromRatio(this.Ratio, this.DimensionsUnitCode, this.ChargeableWeightUnitCode);
    };
    OrdersTabComponent.prototype.OnMeasurmentsSettingsChanged = function () {
        QuoteUtilities_1.QuoteUtilities.RecalculateQuoteFields(this.EntityPM);
    };
    Object.defineProperty(OrdersTabComponent.prototype, "IncotermId", {
        //Quote Details
        get: function () { return this.EntityPM.IncotermId; },
        set: function (newValue) {
            if (this.EntityPM.IncotermId != newValue) {
                this.EntityPM.IncotermId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "MoveTypeId", {
        get: function () { return this.EntityPM.MoveTypeId; },
        set: function (newValue) {
            if (this.EntityPM.MoveTypeId != newValue) {
                this.EntityPM.MoveTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "ExpirationDays", {
        get: function () { return this.EntityPM.ExpirationDays; },
        set: function (newValue) {
            if (this.EntityPM.ExpirationDays != newValue) {
                this.EntityPM.ExpirationDays = newValue;
                if (newValue == null) {
                    this.EntityPM.ExpirationDate = null;
                }
                else {
                    var date = Tools_1.DateTool.GetDateByDay(newValue);
                    if (this.ExpirationDate.valueOf() != date.valueOf()) {
                        this.EntityPM.ExpirationDate = date;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "ExpirationDate", {
        get: function () { return this.EntityPM.ExpirationDate; },
        set: function (newValue) {
            if (this.EntityPM.ExpirationDate != newValue) {
                this.EntityPM.ExpirationDate = newValue;
                if (newValue == null) {
                    this.EntityPM.ExpirationDays = null;
                }
                else {
                    var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    var days = this.GetDaysBetweenDates(newValue, todayDate);
                    if (this.ExpirationDays != days) {
                        this.EntityPM.ExpirationDays = days;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "IsAutomaticallyClosed", {
        get: function () { return this.EntityPM.IsAutomaticallyClosed; },
        set: function (newValue) {
            if (this.EntityPM.IsAutomaticallyClosed != newValue) {
                this.EntityPM.IsAutomaticallyClosed = newValue;
                if (newValue) {
                    var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    this.EntityPM.AutomaticallyCloseDays = 30;
                    this.EntityPM.AutomaticallyCloseDate = Tools_1.DateTool.AddDays(todayDate, 30);
                }
                else {
                    this.EntityPM.AutomaticallyCloseDays = null;
                    this.EntityPM.AutomaticallyCloseDate = null;
                    this.EntityPM.QuoteClosingReasonCode = null;
                }
                this.SetUIProperties_AutomaticallyClosed();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "AutomaticallyCloseDays", {
        get: function () { return this.EntityPM.AutomaticallyCloseDays; },
        set: function (newValue) {
            if (this.EntityPM.AutomaticallyCloseDays != newValue) {
                this.EntityPM.AutomaticallyCloseDays = newValue;
                if (newValue == null) {
                    this.EntityPM.AutomaticallyCloseDate = null;
                }
                else {
                    var date = Tools_1.DateTool.GetDateByDay(newValue);
                    if (this.AutomaticallyCloseDate.valueOf() != date.valueOf()) {
                        this.EntityPM.AutomaticallyCloseDate = date;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "AutomaticallyCloseDate", {
        get: function () { return this.EntityPM.AutomaticallyCloseDate; },
        set: function (newValue) {
            if (this.EntityPM.AutomaticallyCloseDate != newValue) {
                this.EntityPM.AutomaticallyCloseDate = newValue;
                if (newValue == null) {
                    this.EntityPM.AutomaticallyCloseDays = null;
                }
                else {
                    var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    var days = this.GetDaysBetweenDates(newValue, todayDate);
                    if (this.AutomaticallyCloseDays != days) {
                        this.EntityPM.AutomaticallyCloseDays = days;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    OrdersTabComponent.prototype.GetDaysBetweenDates = function (date1, date2) {
        var myResult = 0;
        if (date1 != null && date2 != null) {
            if (date1 != undefined && date2 != undefined) {
                date1 = this.TruncateTime(date1);
                date2 = this.TruncateTime(date2);
                var d1 = new Date(date1.toString());
                var d2 = new Date(date2.toString());
                var timeDiff = d1.getTime() - d2.getTime();
                var Daysdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                myResult = Daysdiff;
            }
        }
        return myResult;
    };
    OrdersTabComponent.prototype.TruncateTime = function (date) {
        var myResult = null;
        if (date != null) {
            var myResult = new Date(date.toString());
            myResult.setUTCHours(0);
            myResult.setUTCMinutes(0);
            myResult.setUTCSeconds(0);
        }
        return myResult;
    };
    Object.defineProperty(OrdersTabComponent.prototype, "DepartureFrequency", {
        get: function () { return this.EntityPM.DepartureFrequency; },
        set: function (newValue) {
            if (this.EntityPM.DepartureFrequency != newValue) {
                this.EntityPM.DepartureFrequency = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OrdersTabComponent.prototype, "TransitTime", {
        get: function () { return this.EntityPM.TransitTime; },
        set: function (newValue) {
            if (this.EntityPM.TransitTime != newValue) {
                this.EntityPM.TransitTime = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], OrdersTabComponent.prototype, "viewContainerRef", void 0);
    OrdersTabComponent = __decorate([
        core_1.Component({
            selector: 'OrdersTabComponent',
            moduleId: module.id,
            templateUrl: './OrdersTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], OrdersTabComponent);
    return OrdersTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OrdersTabComponent = OrdersTabComponent;
//# sourceMappingURL=OrdersTabComponent.js.map