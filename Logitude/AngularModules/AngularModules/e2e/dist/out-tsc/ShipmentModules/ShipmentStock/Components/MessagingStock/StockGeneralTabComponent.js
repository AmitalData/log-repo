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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var MessagingStockPMService_1 = require("../../../../Shipment/Services/StandardPMs/MessagingStockPMService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var StockGeneralTabComponent = /** @class */ (function (_super) {
    __extends(StockGeneralTabComponent, _super);
    function StockGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "MessagingStock";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SelectedStockTypeItem = null;
        _this.IsEditingEnabled = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.ItemsSource = [];
        _this.StockTypesList = [];
        _this.StockTypesList.push(new CodeNameClass_1.CodeNameClass("1", 'Champ'));
        _this.StockTypesList.push(new CodeNameClass_1.CodeNameClass("2", 'INTTRA'));
        _this.SelectedStockTypeItem = _this.StockTypesList.filter(function (f) { return f.Name == _this.EntityPM.StockType; })[0];
        _this.SetUIProperties();
        _this.BuildItemsSource();
        _this.Listen();
        return _this;
    }
    StockGeneralTabComponent.prototype.SelectedStockTypeChanged = function (item) {
        if (this.SelectedStockTypeItem != item) {
            this.SelectedStockTypeItem = item;
            var myResult = null;
            if (item != null) {
                myResult = item.Name;
            }
            this.StockType = myResult;
        }
    };
    StockGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.SetUIProperties();
                }
            });
        }
    };
    StockGeneralTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("Remaining", this.ObjectTableName, false);
        var isFieldEnabled = true;
        if (this.EntityPM.IsCancelled) {
            isFieldEnabled = false;
        }
        else if (this.EntityPM.Status != "New") {
            isFieldEnabled = false;
        }
        this.IsEditingEnabled = isFieldEnabled;
        this.UIProperties.SetEnabled("StockType", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("StartDate", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("EndDate", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Amount", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("Notes", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("TotalPrice", this.ObjectTableName, isFieldEnabled);
    };
    Object.defineProperty(StockGeneralTabComponent.prototype, "StockType", {
        get: function () { return this.EntityPM.StockType; },
        set: function (newValue) {
            if (this.EntityPM.StockType != newValue) {
                this.EntityPM.StockType = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockGeneralTabComponent.prototype, "TenantNumber", {
        get: function () { return this.EntityPM.TenantNumber; },
        set: function (newValue) {
            if (this.EntityPM.TenantNumber != newValue) {
                this.EntityPM.TenantNumber = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockGeneralTabComponent.prototype, "StartDate", {
        get: function () { return this.EntityPM.StartDate; },
        set: function (newValue) {
            if (this.EntityPM.StartDate != newValue) {
                this.EntityPM.StartDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockGeneralTabComponent.prototype, "EndDate", {
        get: function () { return this.EntityPM.EndDate; },
        set: function (newValue) {
            if (this.EntityPM.EndDate != newValue) {
                this.EntityPM.EndDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockGeneralTabComponent.prototype, "Amount", {
        get: function () { return this.EntityPM.Amount; },
        set: function (newValue) {
            if (this.EntityPM.Amount != newValue) {
                this.EntityPM.Amount = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockGeneralTabComponent.prototype, "Remaining", {
        get: function () { return this.EntityPM.Remaining; },
        set: function (newValue) {
            if (this.EntityPM.Remaining != newValue) {
                this.EntityPM.Remaining = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockGeneralTabComponent.prototype, "TotalPrice", {
        get: function () { return this.EntityPM.TotalPrice; },
        set: function (newValue) {
            if (this.EntityPM.TotalPrice != newValue) {
                this.EntityPM.TotalPrice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StockGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    StockGeneralTabComponent.prototype.BuildItemsSource = function () {
        this.ItemsSource = this.EntityPM.StockUsageHistories.sort(function (a, b) { return a.LastActionDate.valueOf() == b.LastActionDate.valueOf() ? 0 : a.LastActionDate.valueOf() < b.LastActionDate.valueOf() ? -1 : 1; });
    };
    StockGeneralTabComponent.prototype.RefreshButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        if (this.myService == null) {
            this.myService = new MessagingStockPMService_1.MessagingStockPMService();
        }
        this.myService.get(this.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.EntityPM = myResponse.Result;
                    _this.BuildItemsSource();
                }
            }
        });
    };
    StockGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: './ShipmentModules/ShipmentStock/Components/MessagingStock/',
            templateUrl: './StockGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], StockGeneralTabComponent);
    return StockGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.StockGeneralTabComponent = StockGeneralTabComponent;
//# sourceMappingURL=StockGeneralTabComponent.js.map