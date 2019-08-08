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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FBLStockPM_1 = require("../../../../Shipment/EntityPMs/FBLStockPM");
var FBLStockExtenedPMService_1 = require("../../../../Shipment/Services/ExtendedPMs/FBLStockExtenedPMService");
var NewFBLStockComponent = /** @class */ (function (_super) {
    __extends(NewFBLStockComponent, _super);
    function NewFBLStockComponent() {
        var _this = _super.call(this) || this;
        _this.IsCustomerMode = false;
        _this.DataContext = _this;
        _this.ObjectTableName = "FBLStock";
        _this.ValidationErrorsList = [];
        _this.ItemsSource = [];
        _this.FBLStocksList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isByNumber = true;
        _this.startNumber = null;
        _this.endNumber = null;
        _this.amount = null;
        _this.isOkButtonClicked = false;
        _this.SelectedItem = null;
        _this.IsListGenerated = false;
        _this.GenerateErrors = [];
        _this.myStartNumber = null;
        _this.myEndNumber = null;
        _this.FBLStockExtenedPMService = new FBLStockExtenedPMService_1.FBLStockExtenedPMService();
        return _this;
    }
    NewFBLStockComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            // this.AirlineId = args['AirlineId'];
            //this.CustomerId = args['CustomerId'];
            // this.IsCustomerMode = args['IsCustomerMode'];
            this.FBLStocksList = args['FBLStocksList'];
            this.SetUIProperties();
        }
    };
    NewFBLStockComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("StartNumber", null, Tools_1.AppTool.IsNullOrEmpty(this.StartNumber) ? true : false);
        this.UIProperties.SetRequired("EndNumber", null, this.IsByNumber && Tools_1.AppTool.IsNullOrEmpty(this.EndNumber) ? true : false);
        this.UIProperties.SetRequired("Amount", null, !this.IsByNumber && Tools_1.AppTool.IsNullOrEmpty(this.Amount) ? true : false);
        this.UIProperties.SetEnabled("EndNumber", null, this.IsByNumber);
        this.UIProperties.SetEnabled("Amount", null, !this.IsByNumber);
    };
    NewFBLStockComponent.prototype.SetIsByNumber = function (newValue) {
        this.IsByNumber = newValue;
    };
    Object.defineProperty(NewFBLStockComponent.prototype, "IsByNumber", {
        get: function () { return this.isByNumber; },
        set: function (newValue) {
            if (this.isByNumber != newValue) {
                this.isByNumber = newValue;
                this.Validate();
                this.RunGenerator();
                this.UIProperties.SetEnabled("EndNumber", null, this.IsByNumber);
                this.UIProperties.SetEnabled("Amount", null, !this.IsByNumber);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFBLStockComponent.prototype, "StartNumber", {
        get: function () { return this.startNumber; },
        set: function (newValue) {
            if (this.startNumber != newValue) {
                this.startNumber = newValue;
                this.Validate();
                this.RunGenerator();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFBLStockComponent.prototype, "EndNumber", {
        get: function () { return this.endNumber; },
        set: function (newValue) {
            if (this.endNumber != newValue) {
                this.endNumber = newValue;
                this.Validate();
                this.RunGenerator();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewFBLStockComponent.prototype, "Amount", {
        get: function () { return this.amount; },
        set: function (newValue) {
            if (this.amount != newValue) {
                this.amount = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.endNumber = null;
                }
                this.Validate();
                this.RunGenerator();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewFBLStockComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewFBLStockComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.isOkButtonClicked = true;
        this.Validate();
        this.GenerateErrors.forEach(function (item) {
            _this.ValidationErrorsList.push(item);
        });
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsListGenerated) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.FBLStockExtenedPMService.CreateFBLStocksOperation(this.myStartNumber, this.myEndNumber).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
        }
    };
    NewFBLStockComponent.prototype.Validate = function () {
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.isOkButtonClicked) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.StartNumber)) {
                errors.push(msg.replace("%FieldName", "Start Number"));
            }
            if (this.IsByNumber) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EndNumber)) {
                    errors.push(msg.replace("%FieldName", "End Number"));
                }
            }
            else {
                if (Tools_1.AppTool.IsNullOrEmpty(this.Amount)) {
                    errors.push(msg.replace("%FieldName", "Amount"));
                }
            }
        }
        this.UIProperties.SetRequired("StartNumber", null, false);
        this.UIProperties.SetRequired("EndNumber", null, false);
        this.UIProperties.SetRequired("Amount", null, false);
        this.UIProperties.SetValidity("StartNumber", null, true, "");
        this.UIProperties.SetValidity("EndNumber", null, true, "");
        this.UIProperties.SetValidity("Amount", null, true, "");
        // Start Number
        if (Tools_1.AppTool.IsNullOrEmpty(this.StartNumber)) {
            this.UIProperties.SetRequired("StartNumber", null, true);
        }
        else if (!Tools_1.FormatTool.IsNumeric(this.StartNumber)) {
            var msgStartNumbe = "Invalid " + TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.O.StartNumber") + ": (should be 20 or less digits)";
            errors.push(msgStartNumbe);
            this.UIProperties.SetValidity("StartNumber", null, false, msgStartNumbe);
        }
        else if (this.StartNumber.length > 20) {
            var msgStartNumbe = "Invalid " + TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.O.StartNumber") + ": (should be less than 20 digits)";
            errors.push(msgStartNumbe);
            this.UIProperties.SetValidity("StartNumber", null, false, msgStartNumbe);
        }
        // End Number
        if (this.IsByNumber) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EndNumber)) {
                this.UIProperties.SetRequired("EndNumber", null, true);
            }
            else if (!Tools_1.FormatTool.IsNumeric(this.EndNumber)) {
                var msgEndNumber = "Invalid " + TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.O.EndNumber") + ": (should be 20 or less digits)";
                errors.push(msgEndNumber);
                this.UIProperties.SetValidity("EndNumber", null, false, msgEndNumber);
            }
            else if (this.EndNumber.length > 20) {
                var msgStartNumbe = "Invalid " + TextCodeTranslator_1.TextCodeTranslator.Translate("FBLStock.O.EndNumber") + ": (should be less than 20 digits)";
                errors.push(msgStartNumbe);
                this.UIProperties.SetValidity("StartNumber", null, false, msgStartNumbe);
            }
        }
        // Amount
        if (!this.IsByNumber) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.Amount)) {
                this.UIProperties.SetRequired("Amount", null, true);
            }
            else if (!Tools_1.FormatTool.IsNumeric(this.Amount)) {
                var msgAmount = "Invalid Amount: should be digits";
                errors.push(msgAmount);
                this.UIProperties.SetValidity("Amount", null, false, msgAmount);
            }
        }
        this.ValidationErrorsList = errors;
    };
    NewFBLStockComponent.prototype.RunGenerator = function () {
        this.ItemsSource = [];
        this.GenerateErrors = [];
        this.SelectedItem = null;
        if (this.IsByNumber) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.StartNumber) && !Tools_1.AppTool.IsNullOrEmpty(this.EndNumber)) {
                if (Tools_1.FormatTool.IsNumeric(this.StartNumber) && Tools_1.FormatTool.IsNumeric(this.EndNumber)) {
                    if (this.StartNumber.length <= 20 && this.EndNumber.length <= 20) {
                        var NumericStartNumber = +this.StartNumber;
                        var NumericEndNumber = +this.EndNumber;
                        var NumericAmount = NumericEndNumber - NumericStartNumber + 1;
                        this.amount = NumericAmount.toString();
                        this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                    }
                }
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.StartNumber) && !Tools_1.AppTool.IsNullOrEmpty(this.Amount)) {
                if (Tools_1.FormatTool.IsNumeric(this.StartNumber) && Tools_1.FormatTool.IsNumeric(this.Amount)) {
                    if (this.StartNumber.length <= 20) {
                        var NumericStartNumber = +this.StartNumber;
                        var NumericAmount = +this.Amount;
                        var NumericEndNumber = NumericStartNumber + NumericAmount - 1;
                        this.endNumber = NumericEndNumber.toString();
                        this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                    }
                }
            }
        }
    };
    NewFBLStockComponent.prototype.GenerateList = function (myStartNumber, myEndNumber, myAmount) {
        var _this = this;
        this.IsListGenerated = false;
        this.myStartNumber = myStartNumber;
        this.myEndNumber = myEndNumber;
        var FBLStocksListCount = this.FBLStocksList == null ? 0 : this.FBLStocksList.length;
        if (myEndNumber < myStartNumber) {
            this.GenerateErrors.push("End number must be greater than start number");
        }
        else if (myAmount > 1000) {
            this.GenerateErrors.push("Maximum number of added stacks in one transaction is 1000");
        }
        else if ((myAmount > 10000) || ((FBLStocksListCount + myAmount) > 10000)) {
            this.GenerateErrors.push("Maximum number of generated stacks is 10000");
        }
        else {
            while (myStartNumber <= myEndNumber) {
                this.IsListGenerated = true;
                //var chk = myStartNumber % 7;
                var newNumberStr = "" + myStartNumber.toString(); //+ chk;
                var newNumber = +newNumberStr;
                var newEntityPM = new FBLStockPM_1.FBLStockPM();
                newEntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                newEntityPM.Number = newNumber;
                if (this.ItemsSource.filter(function (f) { return f.Number == newNumber; })[0] == null) {
                    this.ItemsSource.push(newEntityPM);
                }
                else {
                    this.GenerateErrors.push("FBL Number: " + newNumber + " already exists in the stack!");
                    myStartNumber = myEndNumber + 1;
                }
                myStartNumber++;
            }
        }
        this.GenerateErrors.forEach(function (item) {
            _this.ValidationErrorsList.push(item);
        });
    };
    NewFBLStockComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewFBLStockComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewFBLStockComponent);
    return NewFBLStockComponent;
}(BaseComponent_1.BaseComponent));
exports.NewFBLStockComponent = NewFBLStockComponent;
//# sourceMappingURL=NewFBLStockComponent.js.map