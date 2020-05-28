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
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var AddEditActualLinesComponent = /** @class */ (function (_super) {
    __extends(AddEditActualLinesComponent, _super);
    function AddEditActualLinesComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        return _this;
    }
    AddEditActualLinesComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.invoiceItemPM = args.SupplierInvoiceItemPM;
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.BuildActualInvoiceLines();
        }
    };
    AddEditActualLinesComponent.prototype.BuildActualInvoiceLines = function () {
        this.ItemsSource.Clear();
        var dataString = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(this.invoiceItemPM.ActualInvoiceLines)) {
            dataString = this.invoiceItemPM.ActualInvoiceLines.trim();
            var dataArray = dataString.split(',');
            for (var _i = 0, dataArray_1 = dataArray; _i < dataArray_1.length; _i++) {
                var number = dataArray_1[_i];
                this.ItemsSource.Insert(new ActualLineItem(number.toString(), this));
            }
        }
    };
    Object.defineProperty(AddEditActualLinesComponent.prototype, "FromNumber", {
        get: function () { return this.fromNumber; },
        set: function (value) { this.fromNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditActualLinesComponent.prototype, "ToNumber", {
        get: function () { return this.toNumber; },
        set: function (value) { this.toNumber = value; },
        enumerable: true,
        configurable: true
    });
    AddEditActualLinesComponent.prototype.AddActualLines = function () {
        if (this.ToNumber == null) {
            this.ItemsSource.Insert(new ActualLineItem("", this));
        }
        else if (this.FromNumber != null && this.ToNumber >= this.FromNumber && this.ToNumber > 0 && this.FromNumber >= 0) {
            for (var i = this.FromNumber; i <= this.ToNumber; i++) {
                var number = i.toString();
                var existed = this.ItemsSource.Collection.filter(function (d) { return d.Number == number; })[0];
                if (existed == null && number != "0") {
                    var line = new ActualLineItem(number, this);
                    this.ItemsSource.Insert(line);
                }
            }
            this.FromNumber = null;
            this.ToNumber = null;
        }
    };
    AddEditActualLinesComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    AddEditActualLinesComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        var errors = [];
        var dataString = null;
        for (var _i = 0, _a = this.ItemsSource.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            if (Tools_1.AppTool.IsNullOrEmpty(item.Number)) {
                errors.push("חסר ערכים בשורה");
            }
            if (Tools_1.AppTool.IsNullOrEmpty(dataString)) {
                dataString = item.Number;
            }
            else {
                dataString += "," + item.Number;
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit(dataString);
        }
    };
    AddEditActualLinesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditActualLinesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditActualLinesComponent);
    return AddEditActualLinesComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditActualLinesComponent = AddEditActualLinesComponent;
var ActualLineItem = /** @class */ (function (_super) {
    __extends(ActualLineItem, _super);
    function ActualLineItem(number, Parent) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.Number = number;
        _this.parent = Parent;
        return _this;
    }
    Object.defineProperty(ActualLineItem.prototype, "Number", {
        get: function () { return this.number; },
        set: function (value) { this.number = value; },
        enumerable: true,
        configurable: true
    });
    ActualLineItem.prototype.DeleteButtonClicked = function () {
        if (this.parent.ItemsSource.Collection.includes(this)) {
            this.parent.ItemsSource.Remove(this);
        }
    };
    return ActualLineItem;
}(BaseComponent_1.BaseComponent));
exports.ActualLineItem = ActualLineItem;
//# sourceMappingURL=AddEditActualLinesComponent.js.map