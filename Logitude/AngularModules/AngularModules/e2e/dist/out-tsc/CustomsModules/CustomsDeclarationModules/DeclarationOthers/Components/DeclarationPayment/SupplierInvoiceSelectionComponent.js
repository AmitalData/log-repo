"use strict";
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
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var SupplierInvoiceExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/SupplierInvoiceExtendedListService");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SupplierInvoiceSelectionComponent = /** @class */ (function () {
    function SupplierInvoiceSelectionComponent() {
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.supplierInvoiceExtendedListService = new SupplierInvoiceExtendedListService_1.SupplierInvoiceExtendedListService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsVisibile = false;
        this.preventSelectItem = false;
        this.SupplierInvoicesList = new ObservableCollection_1.ObservableCollection([SupplierInvoiceLine]);
        this.InvoiceItemsList = new ObservableCollection_1.ObservableCollection([]);
        this.SelectedInvoiceItems = new ObservableCollection_1.ObservableCollection([]);
        this.SelectedInvoices = new ObservableCollection_1.ObservableCollection([]);
        this.supplierInvoiceItemList = [];
    }
    SupplierInvoiceSelectionComponent.prototype.SetWindowArgs = function (args) {
        this.DeclarationPM = args.DeclarationPM;
        this.parent = args.Parent;
        this.paymentProtest = args.Protest;
        //this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
        this.BuildInvoicesList();
        //});
    };
    SupplierInvoiceSelectionComponent.prototype.BuildInvoicesList = function () {
        var _this = this;
        this.SupplierInvoicesList.Clear();
        this.Invoices = this.DeclarationPM.SupplierInvoices;
        var invoice = null;
        //for (let protest of this.parent.paymentPM.DeclarationPaymentProtests) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.paymentProtest)) {
            invoice = this.Invoices.filter(function (a) { return a.InvoiceNumber == _this.paymentProtest.InvoiceNumber && a.DeclarationId == _this.paymentProtest.DeclarationId && a.InvoiceCounterKey == _this.paymentProtest.InvoiceCounterKey; })[0];
        }
        if (invoice != null && invoice.InvoiceNumber != null) {
            if (!this.SelectedInvoices.Collection.includes(invoice)) {
                this.SelectedInvoices.Collection.push(invoice);
            }
            //}
        }
        for (var _i = 0, _a = this.Invoices; _i < _a.length; _i++) {
            var item = _a[_i];
            var line = new SupplierInvoiceLine(item, this);
            this.SupplierInvoicesList.Insert(line);
            if (this.SelectedInvoices.Collection.includes(item)) {
                line.IsSelected = true;
                //this.LoadSupplierInvoiceItems(line);
            }
        }
    };
    SupplierInvoiceSelectionComponent.prototype.OnRowSelected = function (item) {
        if (!this.preventSelect) {
            if (this.SelectedRow != item) {
                this.SelectedRow = item;
                item.IsSelected = true;
                //if (!item.IsSelected) {
                this.LoadSupplierInvoiceItems(item);
                //}
            }
        }
        this.preventSelect = false;
    };
    SupplierInvoiceSelectionComponent.prototype.LoadSupplierInvoiceItems = function (invoice) {
        //for (let item of this.InvoiceItemsList.Collection) {
        //    if (!item.IsSelected) {
        //        this.InvoiceItemsList.Remove(item);
        var _this = this;
        //    }
        //}
        this.InvoiceItemsList.Clear();
        this.supplierInvoiceExtendedListService.GetSupplierInvoiceItemsForInvoice(this.DeclarationPM.Id, invoice.InvoiceCounterKey).subscribe(function (response) {
            if (response) {
                var TempInvoiceItemsList = [];
                for (var _i = 0, _a = response.Result; _i < _a.length; _i++) {
                    var item = _a[_i];
                    var selectedItem = null;
                    if (_this.paymentProtest) {
                        selectedItem = response.Result.filter(function (a) { return a.SequenceNumeric == _this.paymentProtest.GoodsItemLineNumber && a.ClassificationCode == _this.paymentProtest.InvoiceItemClassificationCode && a.DeclarationId == _this.paymentProtest.DeclarationId; })[0];
                    }
                    if (selectedItem != null) {
                        if (invoice.IsAccumalated) {
                            if (selectedItem.IsParent) {
                                if (!_this.SelectedInvoiceItems.Collection.includes(selectedItem)) {
                                    _this.SelectedInvoiceItems.Collection.push(selectedItem);
                                }
                            }
                        }
                        else {
                            if (!_this.SelectedInvoiceItems.Collection.includes(selectedItem)) {
                                _this.SelectedInvoiceItems.Collection.push(selectedItem);
                            }
                        }
                    }
                    var invoiceItem = new SupplierInvoiceItemLine(item, _this);
                    //var x: SupplierInvoiceItemLine = this.InvoiceItemsList.Collection.filter(d => d.LineNumer == item.LineNumber && d.CounterKey == item.CounterKey)[0];
                    //  this.supplierInvoiceItemList.push(item);
                    //if (x == null) {
                    if (invoice.IsAccumalated) {
                        if (item.IsParent) {
                            TempInvoiceItemsList.push(invoiceItem);
                        }
                    }
                    else {
                        TempInvoiceItemsList.push(invoiceItem);
                    }
                    if (_this.SelectedInvoiceItems.Collection.includes(item)) {
                        invoiceItem.IsSelected = true;
                    }
                }
                _this.InvoiceItemsList.InsertCollection(TempInvoiceItemsList);
            }
        });
    };
    SupplierInvoiceSelectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    SupplierInvoiceSelectionComponent.prototype.OkButtonClicked = function () {
        this.parent.SelectedInvoiceItems = this.SelectedInvoiceItems;
        this.parent.SelectedInvoices = this.SelectedInvoices;
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    };
    SupplierInvoiceSelectionComponent.prototype.OnItemSelected = function (item) {
        //if (this.SelectedItem == item) {
        //    this.SelectedItem = null;
        //    item.IsSelected = false;
        //}
        //else {
        //    this.SelectedItem = item;
        //    item.IsSelected = true;
        //}
        if (!this.preventSelectItem) {
            if (this.SelectedItem != item) {
                this.SelectedItem = item;
                item.IsSelected = true;
            }
            else {
                this.SelectedItem = null;
                item.IsSelected = false;
            }
        }
        this.preventSelectItem = false;
    };
    SupplierInvoiceSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SupplierInvoiceSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SupplierInvoiceSelectionComponent);
    return SupplierInvoiceSelectionComponent;
}());
exports.SupplierInvoiceSelectionComponent = SupplierInvoiceSelectionComponent;
var SupplierInvoiceLine = /** @class */ (function () {
    function SupplierInvoiceLine(invoice, parent) {
        this.entity = invoice;
        this.Parent = parent;
    }
    Object.defineProperty(SupplierInvoiceLine.prototype, "SequenceNumeric", {
        get: function () { return this.entity.SequenceNumeric; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "InvoiceNumber", {
        get: function () { return this.entity.InvoiceNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "IssueDate", {
        get: function () { return this.entity.IssueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "IncotermCode", {
        get: function () { return this.entity.IncotermCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "InvoiceAmount", {
        get: function () { return this.entity.InvoiceAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "IssueCountryName", {
        get: function () { return this.entity.IssueCountryName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "VendorName", {
        get: function () { return this.entity.VendorName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "PreferenceDocumentTypeName", {
        get: function () { return this.entity.PreferenceDocumentTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "InsruanceCurrencyTypeCode", {
        get: function () { return this.entity.InsruanceCurrencyTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "InsuranceAmount", {
        get: function () { return this.entity.InsuranceAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "InvoiceCounterKey", {
        get: function () { return this.entity.InvoiceCounterKey; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "InvoiceCurrencyTypeCode", {
        get: function () { return this.entity.InvoiceCurrencyTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "TotalFreightInInvoiceCurrencyText", {
        get: function () { return this.entity.TotalFreightInFreightCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "IsAccumalated", {
        get: function () { return this.entity.IsAccumalated; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceLine.prototype, "IsSelected", {
        get: function () { return this.isSelected; },
        set: function (value) {
            this.isSelected = value;
            if (this.isSelected) {
                for (var _i = 0, _a = this.Parent.SupplierInvoicesList.Collection; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (item != this && item.IsSelected) {
                        item.IsSelected = false;
                    }
                    this.Parent.OnRowSelected(this);
                }
                if (!this.Parent.SelectedInvoices.Collection.includes(this.entity)) {
                    this.Parent.SelectedInvoices.Insert(this.entity);
                }
            }
            else {
                if (this.Parent.SelectedInvoices.Collection.includes(this.entity)) {
                    this.Parent.SelectedInvoices.Remove(this.entity);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    SupplierInvoiceLine.prototype.PreventSelect = function () {
        this.Parent.preventSelect = true;
    };
    return SupplierInvoiceLine;
}());
exports.SupplierInvoiceLine = SupplierInvoiceLine;
var SupplierInvoiceItemLine = /** @class */ (function () {
    function SupplierInvoiceItemLine(item, parent) {
        this.entity = item;
        this.Parent = parent;
    }
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "SequenceNumeric", {
        get: function () { return this.entity.SequenceNumeric; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ItemCode", {
        get: function () { return this.entity.ItemCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ClassificationCode", {
        get: function () { return this.entity.ClassificationCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "TradeAgreementName", {
        get: function () { return this.entity.TradeAgreementName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "StatisticQuantity", {
        get: function () { return this.entity.StatisticQuantity; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "InvoiceQuantity", {
        get: function () { return this.entity.InvoiceQuantity; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "CounterKey", {
        get: function () { return this.entity.CounterKey; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ActualInvoiceLines", {
        get: function () { return this.entity.ActualInvoiceLines; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "ItemPrice", {
        get: function () { return this.entity.ItemPrice; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "OriginCountryName", {
        get: function () { return this.entity.OriginCountryName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "LineNumber", {
        get: function () { return this.entity.LineNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "IsSelected", {
        get: function () { return this.isSelected; },
        set: function (value) {
            this.isSelected = value;
            if (this.isSelected) {
                this.Parent.preventSelect = false;
                for (var _i = 0, _a = this.Parent.InvoiceItemsList.Collection; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (item != this && item.IsSelected) {
                        item.IsSelected = false;
                    }
                    //   this.Parent.OnItemSelected(this);
                }
                if (!this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                    this.Parent.SelectedInvoiceItems.Insert(this.entity);
                }
            }
            else {
                //this.Parent.SelectedItem = null;
                //this.Parent.preventSelectItem = true;
                if (this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                    this.Parent.SelectedInvoiceItems.Remove(this.entity);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    SupplierInvoiceItemLine.prototype.PreventSelect = function () {
        this.Parent.preventSelectItem = true;
    };
    return SupplierInvoiceItemLine;
}());
exports.SupplierInvoiceItemLine = SupplierInvoiceItemLine;
//# sourceMappingURL=SupplierInvoiceSelectionComponent.js.map