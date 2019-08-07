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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var CurrencyRatesService_1 = require("../../../Common/Services/CurrencyRatesService");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var RatesTablePM_1 = require("../../../Infrastructure/EntityPMs/RatesTablePM");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var RatesMainTabComponent = /** @class */ (function (_super) {
    __extends(RatesMainTabComponent, _super);
    function RatesMainTabComponent() {
        var _this = _super.call(this) || this;
        //Props 
        _this.TodayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.ItemsSource = [];
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.BuildData();
        return _this;
    }
    //Commands
    RatesMainTabComponent.prototype.EditRate = function (item) {
        var text = TextCodeTranslator_1.TextCodeTranslator.Translate("RatesTable.O.EditCurrencyRate");
        this.RunRateWindow(item, text);
    };
    RatesMainTabComponent.prototype.ViewHistory = function (item) {
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("RatesTable.O.CurrencyHistory") + ": " + item.Code;
        ;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = item.LastRate;
        logWindow.Show('./Common/Components/Maintenance/RatesHistoryComponent');
    };
    RatesMainTabComponent.prototype.RunRateWindow = function (itemComponent, windowTitle) {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 400;
        logitudeWindow.Title = windowTitle;
        var entityPM = new RatesTablePM_1.RatesTablePM();
        entityPM.Tenant = this.TenantPM.Id;
        entityPM.BaseCurrencyId = this.TenantPM.CurrencyId;
        entityPM.ForeignCurrencyId = itemComponent.LastRate.ForeignCurrencyId;
        entityPM.LogDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        itemComponent.EntityPM = entityPM;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./Common/Components/Maintenance/EditLastRateComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnEditWindowClosed($event); });
    };
    RatesMainTabComponent.prototype.OnEditWindowClosed = function (arg) {
        if (arg == 'ok') {
            this.BuildData();
        }
    };
    RatesMainTabComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    //public ViewHistoryIsEnabled: boolean = false;
    // BuildData
    RatesMainTabComponent.prototype.BuildData = function () {
        var _this = this;
        this.ItemsSource = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.TenantPM.CurrencyId)) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("The System Base Currency is unknown!");
            return;
        }
        else {
            var list = new Array();
            var myService = new CurrencyRatesService_1.CurrencyRatesService();
            var loadingDate = this.TodayDate;
            if (loadingDate == null) {
                loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            }
            if (loadingDate != null) {
                //loadingDate = Date.SpecifyKind(loadingDate, Date.UTC);
            }
            myService.GetCurrenciesExchangeRateByValueDate(this.TenantPM.CurrencyId, loadingDate).subscribe(function (resp) {
                var result = resp;
                if (!result.HasError) {
                    result.Result.forEach(function (item) {
                        var itemData = new RatesItem(item);
                        _this.ItemsSource.push(itemData);
                    });
                }
                //else {
                //    var errors = result.ErrorsArray;
                //}
            });
        }
    };
    RatesMainTabComponent = __decorate([
        core_1.Component({
            selector: 'RatesMainTabComponent',
            moduleId: module.id,
            templateUrl: './RatesMainTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], RatesMainTabComponent);
    return RatesMainTabComponent;
}(BaseComponent_1.BaseComponent));
exports.RatesMainTabComponent = RatesMainTabComponent;
var RatesItem = /** @class */ (function (_super) {
    __extends(RatesItem, _super);
    function RatesItem(entityPM) {
        var _this = _super.call(this) || this;
        _this.LastRate = new CurrencyRatesService_1.LastRate();
        _this.ObjectTableName = "RatesTable";
        _this.DataContext = _this;
        _this.LastRate = entityPM;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.CreateRatesTablePM();
        return _this;
    }
    RatesItem.prototype.CreateRatesTablePM = function () {
        this.EntityPM = new RatesTablePM_1.RatesTablePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.BaseCurrencyId = this.TenantPM.CurrencyId;
        this.EntityPM.ForeignCurrencyId = this.LastRate.ForeignCurrencyId;
        this.EntityPM.LogDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.EntityPM.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
    };
    RatesItem.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    RatesItem.prototype.SetUIProperties = function () {
        var isEnabled = this.IsEditingEnabled;
        this.UIProperties.SetEnabled("Rate", "RatesTable", isEnabled);
        this.UIProperties.SetEnabled("ValueDate", "RatesTable", isEnabled);
    };
    Object.defineProperty(RatesItem.prototype, "ValueDate", {
        // Props 
        get: function () {
            return this.EntityPM.ValueDate;
        },
        set: function (value) {
            if (this.EntityPM.ValueDate != value) {
                this.EntityPM.ValueDate = null;
                if (value != null) {
                    //this.RatesTablePM.ValueDate = value.Value.Date;
                    this.EntityPM.ValueDate = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "Rate", {
        get: function () {
            return this.EntityPM.Rate;
        },
        set: function (value) {
            var varValue = Tools_1.AppTool.Round(value, 5);
            if (this.EntityPM.Rate != varValue) {
                this.EntityPM.Rate = varValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "IsEditingEnabled", {
        get: function () {
            var myResult = true;
            if (this.TenantPM.Id == 65) {
                myResult = false;
                if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "LocalCode", {
        get: function () {
            return this.LastRate.BaseCurrencyCode;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "Code", {
        get: function () {
            return this.LastRate.ForeignCurrencyCode;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "Name", {
        get: function () {
            return this.LastRate.ForeignCurrencyName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "CurrentValueDate", {
        get: function () {
            return this.LastRate.ValueDate;
        },
        set: function (value) {
            this.LastRate.ValueDate = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "CurrentRate", {
        get: function () {
            return this.LastRate.Rate;
        },
        set: function (value) {
            this.LastRate.Rate = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "ViewHistoryIsEnabled", {
        get: function () {
            if (this.LastRate.HistoryCount > 0)
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RatesItem.prototype, "ViewHistoryOpacity", {
        get: function () {
            if (this.LastRate.HistoryCount > 0)
                return 1;
            return 0.5;
        },
        enumerable: true,
        configurable: true
    });
    return RatesItem;
}(BaseComponent_1.BaseComponent));
exports.RatesItem = RatesItem;
//# sourceMappingURL=RatesMainTabComponent.js.map