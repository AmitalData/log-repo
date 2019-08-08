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
var Tools_1 = require("../../../../Infrastructure/Tools");
var PaddingPipe_1 = require("../../../../Infrastructure/Pipes/PaddingPipe");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AWBStackDomainService_1 = require("../../../Services/AWBStackDomainService");
var AssignToShipperComponent = /** @class */ (function (_super) {
    __extends(AssignToShipperComponent, _super);
    function AssignToShipperComponent() {
        var _this = _super.call(this) || this;
        _this.IsCustomerMode = false;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isPartialSelection = false;
        _this.shipperId = null;
        _this.total = null;
        _this.from = null;
        _this.to = null;
        _this.StackDomainService = new AWBStackDomainService_1.AWBStackDomainService();
        return _this;
    }
    AssignToShipperComponent.prototype.SetWindowArgs = function (args) {
        this.ShipperId = args['ShipperId'];
        this.IsCustomerMode = args['IsCustomerMode'];
        this.Entity = args['StockSeriesItem'];
        this.SetUIProperties();
        var pipe = new PaddingPipe_1.PaddingPipe();
        this.from = pipe.transform(this.Entity.From, "L", 8, "0");
        this.to = pipe.transform(this.Entity.To, "L", 8, "0");
        this.total = this.Entity.Total;
    };
    AssignToShipperComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("ShipperId", null, !this.IsCustomerMode);
        this.UIProperties.SetEnabled("Total", null, this.IsPartialSelection);
        this.UIProperties.SetEnabled("From", null, this.IsPartialSelection);
        this.UIProperties.SetEnabled("To", null, false);
    };
    Object.defineProperty(AssignToShipperComponent.prototype, "IsPartialSelection", {
        get: function () { return this.isPartialSelection; },
        set: function (newValue) {
            if (this.isPartialSelection != newValue) {
                this.isPartialSelection = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AssignToShipperComponent.prototype, "ShipperId", {
        get: function () { return this.shipperId; },
        set: function (newValue) {
            if (this.shipperId != newValue) {
                this.shipperId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AssignToShipperComponent.prototype, "Total", {
        get: function () { return this.total; },
        set: function (newValue) {
            if (this.total != newValue) {
                this.total = newValue;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AssignToShipperComponent.prototype, "From", {
        get: function () { return this.from; },
        set: function (newValue) {
            if (this.from != newValue) {
                this.from = newValue;
                this.Validate();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AssignToShipperComponent.prototype, "To", {
        get: function () { return this.to; },
        set: function (newValue) {
            if (this.to != newValue) {
                this.to = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AssignToShipperComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AssignToShipperComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.Validate();
        if (this.ValidationErrorsList.length == 0) {
            var errors = [];
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            if (this.ShipperId == null) {
                errors.push(msg.replace("%FieldName", "Shipper"));
            }
            this.ValidationErrorsList = errors;
        }
        if (this.ValidationErrorsList.length > 0) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            this.StackDomainService.AssignStockSeriesToCustomer(+this.From, +this.To, this.Entity.AirlineId, this.ShipperId).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            });
        }
    };
    AssignToShipperComponent.prototype.Validate = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (Tools_1.AppTool.IsNullOrEmpty(this.From)) {
            errors.push(msg.replace("%FieldName", "From"));
        }
        else if (this.From.length != 8) {
            errors.push("From Field length should be 8 digits");
        }
        else {
            var pipe = new PaddingPipe_1.PaddingPipe();
            // Origin Data
            var fromString = pipe.transform(this.Entity.From, "L", 8, "0");
            var toString = pipe.transform(this.Entity.To, "L", 8, "0");
            var start = +fromString.substr(0, fromString.length - 1);
            var end = +toString.substr(0, toString.length - 1);
            var fr = +this.From.substr(0, this.From.length - 1);
            var to = +this.To.substr(0, this.To.length - 1);
            var myTotal = +this.Total;
            if (myTotal != 0) {
                if ((myTotal > this.Entity.Total) || (fr + myTotal - 1) > end) {
                    errors.push("Selected series is out of available range");
                }
                else {
                    this.To = ((fr + myTotal - 1).toString() + ((fr + myTotal - 1) % 7).toString());
                }
            }
            else {
                errors.push("Total Field can't be zero");
            }
            if (this.From.length == 8) {
                var checkDigit1 = +this.From.substr(7, 1);
                var ck = fr % 7;
                if (checkDigit1 != ck) {
                    errors.push("From Field check digit is invalid it should be: " + ck);
                }
                else {
                    if ((fr < start) || (fr + myTotal - 1) > end) {
                        errors.push("Selected series is out of available range");
                    }
                    else {
                        this.To = ((fr + myTotal - 1).toString() + ((fr + myTotal - 1) % 7).toString());
                    }
                }
            }
        }
        this.ValidationErrorsList = errors;
    };
    AssignToShipperComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AssignToShipperComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AssignToShipperComponent);
    return AssignToShipperComponent;
}(BaseComponent_1.BaseComponent));
exports.AssignToShipperComponent = AssignToShipperComponent;
//# sourceMappingURL=AssignToShipperComponent.js.map