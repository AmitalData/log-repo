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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var UpdateProcessCodeComponent = /** @class */ (function (_super) {
    __extends(UpdateProcessCodeComponent, _super);
    function UpdateProcessCodeComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.IsDisplayOnly = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsAddMessageVisible = false;
        _this.AddMessage = null;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.UIProperties.SetEnabled("FromNumber", null, false);
        _this.UIProperties.SetEnabled("ToNumber", null, false);
        _this.IsAddButtonEnabled = false;
        return _this;
    }
    UpdateProcessCodeComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.SupplierInvoicePM = args.SupplierInvoicePM;
        }
    };
    UpdateProcessCodeComponent.prototype.BuildActualInvoiceLines = function () {
        this.ItemsSource.Clear();
        var dataString = "";
    };
    Object.defineProperty(UpdateProcessCodeComponent.prototype, "FromNumber", {
        get: function () { return this.fromNumber; },
        set: function (value) { this.fromNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateProcessCodeComponent.prototype, "ToNumber", {
        get: function () { return this.toNumber; },
        set: function (value) { this.toNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateProcessCodeComponent.prototype, "UpdateAll", {
        get: function () { return this.updateAll; },
        set: function (value) {
            this.updateAll = value;
            if (value) {
                this.UpdateSelected = false;
                this.UIProperties.SetEnabled("FromNumber", null, false);
                this.UIProperties.SetEnabled("ToNumber", null, false);
                this.IsAddButtonEnabled = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateProcessCodeComponent.prototype, "UpdateSelected", {
        get: function () { return this.updateSelected; },
        set: function (value) {
            this.updateSelected = value;
            if (value) {
                this.UpdateAll = false;
                this.UIProperties.SetEnabled("FromNumber", null, true);
                this.UIProperties.SetEnabled("ToNumber", null, true);
                this.IsAddButtonEnabled = true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateProcessCodeComponent.prototype, "ProcessTypeCode", {
        get: function () { return this.processTypeCode; },
        set: function (value) {
            this.processTypeCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateProcessCodeComponent.prototype, "IsAddButtonEnabled", {
        get: function () { return this.isAddButtonEnabled; },
        set: function (value) {
            this.isAddButtonEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    UpdateProcessCodeComponent.prototype.UpdateAllRadio = function (newValue) {
        this.UpdateAll = newValue;
    };
    UpdateProcessCodeComponent.prototype.UpdateSelectedRadio = function (newValue) {
        this.UpdateSelected = newValue;
    };
    UpdateProcessCodeComponent.prototype.CancelButtonClicked = function () {
        this.ItemsSource = null;
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    UpdateProcessCodeComponent.prototype.AddLines = function () {
        var _this = this;
        this.IsAddMessageVisible = false;
        this.AddMessage = null;
        if (this.ToNumber == null) {
            if (this.ItemsSource.Collection.length < this.SupplierInvoicePM.SupplierInvoiceItems.length) {
                this.ItemsSource.Insert(new SelectedItem(null, this));
            }
            else {
                this.IsAddMessageVisible = true;
                this.AddMessage = "קיימים רק " + this.ItemsSource.Collection.length + " פריטים";
            }
        }
        else if (this.FromNumber != null && this.ToNumber >= this.FromNumber && this.ToNumber > 0 && this.FromNumber >= 0) {
            var toNumber = this.SupplierInvoicePM.SupplierInvoiceItems.filter(function (d) { return d.SequenceNumeric == _this.ToNumber; })[0];
            if (!toNumber) {
                this.IsAddMessageVisible = true;
                this.AddMessage = "עד מספר גדול ממספר הפריטים";
            }
            else {
                for (var i = this.FromNumber; i <= this.ToNumber; i++) {
                    var number = i;
                    var existed = this.ItemsSource.Collection.filter(function (d) { return d.Number == number; })[0];
                    if (existed == null && number != 0) {
                        var line = new SelectedItem(number, this);
                        this.ItemsSource.Insert(line);
                    }
                }
                this.FromNumber = null;
                this.ToNumber = null;
            }
        }
    };
    UpdateProcessCodeComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        var errors = [];
        if (!this.UpdateAll && !this.UpdateSelected) {
            errors.push("בחר פריטים לעדכון");
        }
        if (this.ProcessTypeCode == null) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ProcessTypeRequired"));
        }
        if (this.UpdateSelected && this.ItemsSource.Length == 0) {
            errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.SelectItems"));
        }
        var _loop_1 = function (item) {
            if (item.Number == null) {
                errors.push("חובה לבחור ערך בשדה מספר");
            }
            else {
                exist = this_1.SupplierInvoicePM.SupplierInvoiceItems.filter(function (d) { return d.SequenceNumeric == item.Number; })[0];
                if (!exist) {
                    errors.push("מספר " + item.Number + " אינו קיים בפריטים");
                    //  errors.push("חשבון זה מכיל " + this.SupplierInvoicePM.SupplierInvoiceItems.length + " שורות, לא ניתן לבחור מספר גדול מ- " + this.SupplierInvoicePM.SupplierInvoiceItems.length);
                }
            }
        };
        var this_1 = this, exist;
        for (var _i = 0, _a = this.ItemsSource.Collection; _i < _a.length; _i++) {
            var item = _a[_i];
            _loop_1(item);
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }
    };
    UpdateProcessCodeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UpdateProcessCodeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], UpdateProcessCodeComponent);
    return UpdateProcessCodeComponent;
}(BaseComponent_1.BaseComponent));
exports.UpdateProcessCodeComponent = UpdateProcessCodeComponent;
var SelectedItem = /** @class */ (function (_super) {
    __extends(SelectedItem, _super);
    function SelectedItem(number, Parent) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.Number = number;
        _this.parent = Parent;
        return _this;
    }
    Object.defineProperty(SelectedItem.prototype, "Number", {
        get: function () { return this.number; },
        set: function (value) { this.number = value; },
        enumerable: true,
        configurable: true
    });
    SelectedItem.prototype.DeleteButtonClicked = function () {
        if (this.parent.ItemsSource.Collection.includes(this)) {
            this.parent.ItemsSource.Remove(this);
        }
    };
    return SelectedItem;
}(BaseComponent_1.BaseComponent));
exports.SelectedItem = SelectedItem;
//# sourceMappingURL=UpdateProcessCodeComponent.js.map