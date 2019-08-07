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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CustomerProductExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/CustomerProductExtendedService");
var BlockedCustomerComponent = /** @class */ (function (_super) {
    __extends(BlockedCustomerComponent, _super);
    function BlockedCustomerComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customer";
        _this.DataContext = _this;
        _this.ProductsObslist = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ProductsObslist = [];
        return _this;
    }
    BlockedCustomerComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.entityList = args.CustomerList;
            this.LoadData();
        }
    };
    Object.defineProperty(BlockedCustomerComponent.prototype, "Code", {
        // Properties 
        get: function () { return this.entityList.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "Name", {
        get: function () { return this.entityList.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "VAT", {
        get: function () { return this.entityList.VatNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "Status", {
        get: function () { return this.entityList.CustomerStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "StatusCode", {
        get: function () { return this.entityList.CustomerStatusCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "Salesman", {
        get: function () { return this.entityList.SalesmanUserEnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "StartWorkingDate", {
        get: function () { return this.entityList.StartWorkingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "LastShipmentDate", {
        get: function () { return this.entityList.LastShipmentDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BlockedCustomerComponent.prototype, "LastInteractionDate", {
        get: function () { return this.entityList.LastInteractionDate; },
        enumerable: true,
        configurable: true
    });
    //LoadDate
    BlockedCustomerComponent.prototype.LoadData = function () {
        var _this = this;
        this.ProductsObslist = [];
        var service = new CustomerProductExtendedService_1.CustomerProductExtendedService();
        service.GetCustomerProducts(this.entityList.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            //this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!response.HasError) {
                var myResult = response.Result;
                if (myResult) {
                    myResult.forEach(function (item) {
                        _this.ProductsObslist.push(new ProductObslistItem(item));
                    });
                }
            }
        });
    };
    // Commands 
    BlockedCustomerComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    BlockedCustomerComponent = __decorate([
        core_1.Component({
            selector: 'BlockedCustomerComponent',
            moduleId: module.id,
            templateUrl: './BlockedCustomerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], BlockedCustomerComponent);
    return BlockedCustomerComponent;
}(BaseComponent_1.BaseComponent));
exports.BlockedCustomerComponent = BlockedCustomerComponent;
var ProductObslistItem = /** @class */ (function () {
    function ProductObslistItem(item) {
        this.entityPM = item;
    }
    Object.defineProperty(ProductObslistItem.prototype, "Name", {
        get: function () { return this.entityPM.ProductTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductObslistItem.prototype, "LastShipmentDate", {
        get: function () {
            var myResult = "No Shipments";
            //if (this.entityPM.LastShipmentDate != null) {
            //    var dateTime = this.entityPM.LastShipmentDate;
            //    var todayDate = DateTool.GetCurrentDateTimeAsUtc();
            //    if (dateTime.valueOf() == todayDate.valueOf()) {
            //        myResult = TextCodeTranslator.Translate("General.O.Today");
            //    }
            //    else if (dateTime.valueOf() == todayDate.Date.AddDays(-1)) {
            //        myResult = TextCodeTranslator.Translate("General.O.Yesterday");
            //    }
            //    else if (dateTime..valueOf() == todayDate.Date.AddDays(1)) {
            //        myResult = TextCodeTranslator.Translate("General.O.Tomorrow");
            //    }
            //    else {
            //        myResult = dateTime.ToString("d", CultureInfo.CurrentCulture);
            //    }
            //}
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    return ProductObslistItem;
}());
exports.ProductObslistItem = ProductObslistItem;
//# sourceMappingURL=BlockedCustomerComponent.js.map