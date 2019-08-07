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
var ARInvoiceStockLinePM_1 = require("../../../../Invoice/EntityPMs/ARInvoiceStockLinePM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ARInvoiceStockPMService_1 = require("../../../../Invoice/Services/StandardPMs/ARInvoiceStockPMService");
var NewARInvoiceStockLinesComponent = /** @class */ (function (_super) {
    __extends(NewARInvoiceStockLinesComponent, _super);
    function NewARInvoiceStockLinesComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "ARInvoiceStockLine";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ItemsSource = [];
        _this.prefix = null;
        _this.suffix = null;
        _this.isByNumber = true;
        _this.startNumber = null;
        _this.endNumber = null;
        _this.size = null;
        _this.amount = null;
        _this.SelectedItem = null;
        _this.IsListGenerated = false;
        _this.GenerateErrors = [];
        _this.isOkButtonClicked = false;
        return _this;
    }
    NewARInvoiceStockLinesComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.Stock = args['Stock'];
            this.SetUIProperties();
        }
    };
    NewARInvoiceStockLinesComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("StartNumber", null, Tools_1.AppTool.IsNullOrEmpty(this.StartNumber) ? true : false);
        this.UIProperties.SetRequired("EndNumber", null, this.IsByNumber && Tools_1.AppTool.IsNullOrEmpty(this.EndNumber) ? true : false);
        this.UIProperties.SetRequired("Amount", null, !this.IsByNumber && Tools_1.AppTool.IsNullOrEmpty(this.Amount) ? true : false);
        this.UIProperties.SetEnabled("EndNumber", null, this.IsByNumber);
        this.UIProperties.SetEnabled("Amount", null, !this.IsByNumber);
    };
    Object.defineProperty(NewARInvoiceStockLinesComponent.prototype, "Prefix", {
        get: function () { return this.prefix; },
        set: function (newValue) {
            if (this.prefix != newValue) {
                this.prefix = newValue;
                this.Validate();
                this.RunGenerator();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceStockLinesComponent.prototype, "Suffix", {
        get: function () { return this.suffix; },
        set: function (newValue) {
            if (this.suffix != newValue) {
                this.suffix = newValue;
                this.Validate();
                this.RunGenerator();
            }
        },
        enumerable: true,
        configurable: true
    });
    NewARInvoiceStockLinesComponent.prototype.SetIsByNumber = function (newValue) {
        this.IsByNumber = newValue;
    };
    Object.defineProperty(NewARInvoiceStockLinesComponent.prototype, "IsByNumber", {
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
    Object.defineProperty(NewARInvoiceStockLinesComponent.prototype, "StartNumber", {
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
    Object.defineProperty(NewARInvoiceStockLinesComponent.prototype, "EndNumber", {
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
    Object.defineProperty(NewARInvoiceStockLinesComponent.prototype, "Size", {
        get: function () { return this.size; },
        set: function (newValue) {
            if (this.size != newValue) {
                this.size = newValue;
                this.Validate();
                this.RunGenerator();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewARInvoiceStockLinesComponent.prototype, "Amount", {
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
    NewARInvoiceStockLinesComponent.prototype.Validate = function () {
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
            var msgStartNumbe = "Invalid StartNumber: should be digits";
            errors.push(msgStartNumbe);
            this.UIProperties.SetValidity("StartNumber", null, false, msgStartNumbe);
        }
        // End Number
        if (this.IsByNumber) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EndNumber)) {
                this.UIProperties.SetRequired("EndNumber", null, true);
            }
            else if (!Tools_1.FormatTool.IsNumeric(this.EndNumber)) {
                var msgEndNumber = "Invalid End Number: should be digits";
                errors.push(msgEndNumber);
                this.UIProperties.SetValidity("EndNumber", null, false, msgEndNumber);
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
    NewARInvoiceStockLinesComponent.prototype.RunGenerator = function () {
        this.ItemsSource = [];
        this.GenerateErrors = [];
        this.SelectedItem = null;
        if (this.IsByNumber) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.StartNumber) && !Tools_1.AppTool.IsNullOrEmpty(this.EndNumber)) {
                if (Tools_1.FormatTool.IsNumeric(this.StartNumber) && Tools_1.FormatTool.IsNumeric(this.EndNumber)) {
                    var NumericStartNumber = +this.StartNumber;
                    var NumericEndNumber = +this.EndNumber;
                    var NumericAmount = NumericEndNumber - NumericStartNumber + 1;
                    this.amount = NumericAmount.toString();
                    this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                }
            }
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.StartNumber) && !Tools_1.AppTool.IsNullOrEmpty(this.Amount)) {
                if (Tools_1.FormatTool.IsNumeric(this.StartNumber) && Tools_1.FormatTool.IsNumeric(this.Amount)) {
                    var NumericStartNumber = +this.StartNumber;
                    var NumericAmount = +this.Amount;
                    var NumericEndNumber = NumericStartNumber + NumericAmount - 1;
                    this.endNumber = NumericEndNumber.toString();
                    this.GenerateList(NumericStartNumber, NumericEndNumber, NumericAmount);
                }
            }
        }
    };
    NewARInvoiceStockLinesComponent.prototype.GenerateList = function (myStartNumber, myEndNumber, myAmount) {
        var _this = this;
        this.IsListGenerated = false;
        var StocksListCount = this.Stock.ARInvoiceStockLines == null ? 0 : this.Stock.ARInvoiceStockLines.length;
        if (myEndNumber < myStartNumber) {
            this.GenerateErrors.push("End number must be greater than start number");
        }
        else if (myAmount > 1000) {
            this.GenerateErrors.push("Maximum number of added stacks in one transaction is 1000");
        }
        else if ((myAmount > 10000) || ((StocksListCount + myAmount) > 10000)) {
            this.GenerateErrors.push("Maximum number of generated stacks is 10000");
        }
        else if (!Tools_1.AppTool.IsNullOrZero(this.Size)) {
            if (myStartNumber.toString().length > this.Size) {
                this.GenerateErrors.push("Start Number should be " + this.Size + " digits maximum");
            }
            else if (myEndNumber.toString().length > this.Size) {
                this.GenerateErrors.push("End Number should be " + this.Size + " digits maximum");
            }
            else {
                this.StartGenerating(myStartNumber, myEndNumber);
            }
        }
        else {
            this.StartGenerating(myStartNumber, myEndNumber);
        }
        this.GenerateErrors.forEach(function (item) {
            _this.ValidationErrorsList.push(item);
        });
    };
    NewARInvoiceStockLinesComponent.prototype.StartGenerating = function (myStartNumber, myEndNumber) {
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        while (myStartNumber <= myEndNumber) {
            this.IsListGenerated = true;
            var paddingStartNumber = myStartNumber.toString();
            if (!Tools_1.AppTool.IsNullOrZero(this.Size)) {
                if (myStartNumber.toString().length < this.Size) {
                    paddingStartNumber = myStartNumber.toString().padStart(this.Size, "0");
                }
            }
            var invoiceNumber = paddingStartNumber.toString();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Prefix) && !Tools_1.AppTool.IsNullOrEmpty(this.Suffix)) {
                invoiceNumber = this.Prefix + paddingStartNumber.toString() + this.Suffix;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.Prefix) && Tools_1.AppTool.IsNullOrEmpty(this.Suffix)) {
                invoiceNumber = this.Prefix + paddingStartNumber.toString();
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(this.Prefix) && !Tools_1.AppTool.IsNullOrEmpty(this.Suffix)) {
                invoiceNumber = paddingStartNumber.toString() + this.Suffix;
            }
            var newEntityPM = new ARInvoiceStockLinePM_1.ARInvoiceStockLinePM(this.Stock);
            newEntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            newEntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            newEntityPM.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            newEntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            newEntityPM.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            newEntityPM.CreateDate = todayDate;
            newEntityPM.UpdateDate = todayDate;
            newEntityPM.Number = invoiceNumber;
            if (this.Stock.ARInvoiceStockLines.filter(function (f) { return f.Number == invoiceNumber; })[0] == null) {
                this.ItemsSource.push(newEntityPM);
            }
            else {
                this.GenerateErrors.push("Invoice Number: [" + invoiceNumber + "] already exists in the stock!");
                myStartNumber = myEndNumber + 1;
            }
            myStartNumber++;
        }
    };
    NewARInvoiceStockLinesComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewARInvoiceStockLinesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.isOkButtonClicked = true;
        this.Validate();
        this.GenerateErrors.forEach(function (item) {
            _this.ValidationErrorsList.push(item);
        });
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsListGenerated) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.ItemsSource.forEach(function (item) {
                    _this.Stock.AddARInvoiceStockLinePM(item);
                });
                var startNumber = this.StartNumber;
                var endNumber = this.EndNumber;
                var length = this.ItemsSource.length;
                startNumber = this.ItemsSource[0].Number;
                endNumber = this.ItemsSource[length - 1].Number;
                this.Stock.NumbersAdded = true;
                this.Stock.Amount = this.Stock.ARInvoiceStockLines.length;
                this.Stock.EventNotes = "Invoice numbers from [" + startNumber + "] to [" + endNumber + "] added";
                var stockPMService = new ARInvoiceStockPMService_1.ARInvoiceStockPMService();
                if (Tools_1.AppTool.IsNullOrEmpty(this.Stock.Id)) {
                    stockPMService.insert(this.Stock).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                    });
                }
                else {
                    stockPMService.update(this.Stock).subscribe(function (myResponse) {
                        _this.CurrentSession.StopBusyIndicator();
                        if (!myResponse.HasError) {
                            if (_this.CurrentSession.CurrentEditComponent != null) {
                                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            }
                            _this.CurrentSession.CloseCurrentWindowEmit("OK");
                        }
                        else {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                    });
                }
            }
        }
    };
    NewARInvoiceStockLinesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewARInvoiceStockLinesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewARInvoiceStockLinesComponent);
    return NewARInvoiceStockLinesComponent;
}(BaseComponent_1.BaseComponent));
exports.NewARInvoiceStockLinesComponent = NewARInvoiceStockLinesComponent;
//# sourceMappingURL=NewARInvoiceStockLinesComponent.js.map