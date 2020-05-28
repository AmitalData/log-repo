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
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var PointersFromInvoicesSelectionComponent = /** @class */ (function () {
    function PointersFromInvoicesSelectionComponent() {
        var _this = this;
        this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.supplierInvoiceExtendedListService = new SupplierInvoiceExtendedListService_1.SupplierInvoiceExtendedListService;
        this.deletedInvItems = "";
        this.initialSelectedItems = "";
        this.MenuHeaderchangeevent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsVisibile = false;
        this.DataSource = {
            pageSize: 10,
            rowCount: null,
            sortingCol: "SequenceNumeric",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        this.SupplierInvoicesList = new ObservableCollection_1.ObservableCollection([SupplierInvoiceLine]);
        this.InvoiceItemsList = new ObservableCollection_1.ObservableCollection([]);
        this.SelectedInvoiceItems = new ObservableCollection_1.ObservableCollection([]);
        this.StaticSelectedInvoiceItems = new ObservableCollection_1.ObservableCollection([]);
        this.SelectedInvoices = new ObservableCollection_1.ObservableCollection([]);
        this.BuildColumns();
    }
    PointersFromInvoicesSelectionComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.DeclarationPM = args.DeclarationPM;
        this.CustomsDocumentsTicket = args.CustomsDocumentsTicket;
        this.SelectInvoicesOnly = args.selectInvoicesOnly;
        //this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe((response: any) => {
        this.initialSelectedItems = this.CustomsDocumentsTicket.ConnectedInvoiceItemsSequences;
        var connectedCounterKeys = "";
        var connectedLineNumbers = "";
        this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach(function (pointer) {
            if (pointer.Child1EntityId != null && pointer.Child2EntityId) {
                connectedCounterKeys = connectedCounterKeys + "," + pointer.Child1EntityId;
                if (_this.CustomsDocumentsTicket.ConnectedInvoiceItemsSequences.indexOf(pointer.Child2EntityId) > -1) {
                    connectedLineNumbers = connectedLineNumbers + "," + pointer.Child2EntityId;
                }
            }
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(connectedCounterKeys) && !Tools_1.AppTool.IsNullOrEmpty(connectedCounterKeys)) {
            this.supplierInvoiceExtendedListService.GetSelectedSupplierInvoiceItemLists(this.DeclarationPM.Id, connectedCounterKeys, connectedLineNumbers).subscribe(function (response) {
                var resp = response.Result;
                resp.forEach(function (item) {
                    _this.StaticSelectedInvoiceItems.Insert(item);
                });
                _this.BuildInvoicesList();
            });
        }
        else {
            this.BuildInvoicesList();
        }
        //this.BuildInvoicesList();
        //this.SelectedItemChangedEvt = this.SelectedInvoiceItems.Changed.subscribe((response) => {
        //    if (response.Operation == "insert") {
        //        //this.SelectedInvoiceItems.Collection.forEach((item) => {
        //        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == response.Item.rowData.CounterKey && d.LineNumber == response.Item.rowData.LineNumber)[0];
        //        if (!exists) {
        //            this.StaticSelectedInvoiceItems.Insert(response.Item.rowData);
        //        }
        //        // });
        //    }
        //    else if (response.Operation == "remove") {
        //        var exists = this.StaticSelectedInvoiceItems.Collection.filter(d => d.CounterKey == response.Item.rowData.CounterKey && d.LineNumber == response.Item.rowData.LineNumber)[0];
        //        this.StaticSelectedInvoiceItems.Remove(exists);
        //    }
        //    //var previousSelected = this.StaticSelectedInvoiceItems;
        //    //previousSelected.Collection.forEach((item) => {
        //    //    var exists = this.SelectedInvoiceItems.Collection.filter(d => d.rowData.CounterKey == item.CounterKey && d.rowData.LineNumber == item.LineNumber)[0];
        //    //    if (!exists) {
        //    //        this.StaticSelectedInvoiceItems.Remove(item);
        //    //    }
        //    //});
        //});
        this.IsEntityDisplayOnly = args.IsEntityDisplayOnly;
        if (this.CustomsDocumentsTicket.RequestedCustomsDocId || this.IsEntityDisplayOnly) {
            this.IsDisplayOnly = true;
            this.DisplayOnlyMessage = "מסמך לתצוגה בלבד";
        }
        else {
            this.IsDisplayOnly = false;
            this.DisplayOnlyMessage = "";
        }
        //});
    };
    PointersFromInvoicesSelectionComponent.prototype.BuildInvoicesList = function () {
        var _this = this;
        this.SupplierInvoicesList.Clear();
        this.Invoices = this.DeclarationPM.SupplierInvoices;
        var temp = [];
        this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach(function (pointer) {
            var invoice = _this.Invoices.filter(function (d) { return d.InvoiceCounterKey + "" == pointer.Child1EntityId; })[0];
            if (invoice) {
                var exists = _this.SelectedInvoices.Collection.filter(function (d) { return d.InvoiceCounterKey == invoice.InvoiceCounterKey; })[0];
                if (!exists) {
                    var line = new SupplierInvoiceLine(invoice, _this);
                    var exist = temp.filter(function (d) { return d.InvoiceCounterKey == line.InvoiceCounterKey; })[0];
                    if (!exist) {
                        _this.SupplierInvoicesList.Insert(line);
                        //this.SelectedInvoices.Collection.push(line);
                        temp.push(line);
                    }
                }
            }
        });
        this.SelectedInvoices.Collection = temp;
        this.SelectedRows = this.SelectedInvoices.Collection;
        for (var _i = 0, _a = this.Invoices; _i < _a.length; _i++) {
            var item = _a[_i];
            var invoice = this.SupplierInvoicesList.Collection.filter(function (d) { return d.InvoiceCounterKey == item.InvoiceCounterKey; })[0];
            if (!invoice) {
                var line = new SupplierInvoiceLine(item, this);
                if (!this.SupplierInvoicesList.Collection.includes(line)) {
                    this.SupplierInvoicesList.Insert(line);
                }
            }
        }
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        //this.LoadSupplierInvoiceItems(this.SelectedInvoices.Collection);
    };
    PointersFromInvoicesSelectionComponent.prototype.OnRowSelected = function (items) {
        var _this = this;
        this.SelectedRows = items;
        this.SelectedInvoices.Collection.forEach(function (invoiceLine) {
            var item = items.filter(function (d) { return d.InvoiceCounterKey == invoiceLine.InvoiceCounterKey; })[0];
            if (!item) {
                _this.SelectedInvoices.Remove(invoiceLine);
                var tempItems = [];
                _this.SelectedInvoiceItems.Collection.forEach(function (invoiceItem) {
                    tempItems.push(invoiceItem);
                });
                tempItems.forEach(function (invoiceItemtemp) {
                    if (invoiceItemtemp.rowData.CounterKey == invoiceLine.InvoiceCounterKey) {
                        _this.SelectedInvoiceItems.Remove(invoiceItemtemp);
                        _this.deletedInvItems += "," + invoiceItemtemp.rowData.SequenceNumeric;
                    }
                });
            }
        });
        //this.LoadSupplierInvoiceItems(items);
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    PointersFromInvoicesSelectionComponent.prototype.OnRowUnselected = function (item) {
        //console.log(item);
        var exists = this.StaticSelectedInvoiceItems.Collection.filter(function (d) { return d.CounterKey == item.CounterKey && d.LineNumber == item.LineNumber; })[0];
        this.StaticSelectedInvoiceItems.Remove(exists);
    };
    PointersFromInvoicesSelectionComponent.prototype.OnItemRowSelected = function (item) {
        var exists = this.StaticSelectedInvoiceItems.Collection.filter(function (d) { return d.CounterKey == item.rowData.CounterKey && d.LineNumber == item.rowData.LineNumber; })[0];
        if (!exists) {
            this.StaticSelectedInvoiceItems.Insert(item.rowData);
        }
        //var exists = this.SelectedInvoiceItems.Collection.filter(d => d.rowData.CounterKey == item.rowData.CounterKey && d.rowData.LineNumber==item.rowData.LineNumber)[0];
        //if (!exists) {
        //    this.SelectedInvoiceItems.Insert(item);
        //    //this.StaticSelectedInvoiceItems.Insert(item);
        //}
        //this.SelectedInvoiceItems.Collection.forEach((invoiceItemLine) => {
        //    var item = items.filter(d => d.CounterKey == invoiceItemLine.CounterKey && d.LineNumber == invoiceItemLine.LineNumber)[0];
        //    if (!item) {
        //        this.SelectedInvoiceItems.Remove(invoiceItemLine);
        //    }
        //});
    };
    PointersFromInvoicesSelectionComponent.prototype.OnDataLoaded = function (items) {
        var _this = this;
        var temp = [];
        //this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((pointer) => {
        //    var invItem: any = items.filter(d => d.rowData.CounterKey + "" == pointer.Child1EntityId && d.rowData.LineNumber + "" == pointer.Child2EntityId)[0];
        //    if (invItem) {
        //        temp.push(invItem);
        //    }
        //});
        this.StaticSelectedInvoiceItems.Collection.forEach(function (item) {
            //var exists = this.SelectedInvoiceItems.Collection.filter(d => d.rowData.CounterKey == item.CounterKey && d.rowData.LineNumber == item.LineNumber)[0];
            _this.SelectedInvoiceItems.Clear();
            var invItem = items.filter(function (d) { return d.rowData.CounterKey + "" == item.CounterKey && d.rowData.LineNumber + "" == item.LineNumber; })[0];
            //if (this.SelectedInvoiceItems.Collection.indexOf(item) == -1) {exists == null &&
            if (invItem != null) {
                _this.SelectedInvoiceItems.Insert(invItem);
            }
        });
        //if (!this.SelectedInvoiceItems.Collection.includes(invItem)) {
        //this.SelectedInvoiceItems.AppendCollection(temp);//.push(invItem);
        //}
        //temp.forEach((invoiceItemLine) => {
        //    this.SelectedInvoiceItems.Insert(invoiceItemLine);
        //});
    };
    PointersFromInvoicesSelectionComponent.prototype.LoadSupplierInvoiceItems = function (invoices) {
        return;
        //this.InvoiceItemsList.Clear();
        //var keys = "";
        //invoices.forEach((invoice) => {
        //    keys = keys + "," + invoice.InvoiceCounterKey;
        //});
        //keys = keys.substr(1, keys.length - 1);
        //if (keys.length > 0) {
        //    this.supplierInvoiceExtendedListService.GetSupplierInvoiceItemsForInvoices(this.DeclarationPM.Id, keys, null, null).subscribe((response: any) => {
        //        if (response) {
        //            var invoiceItems: SupplierInvoiceItemList[] = response.Result;
        //            if (invoiceItems) {
        //                for (let item of invoiceItems) {
        //                    var invoiceItem: SupplierInvoiceItemLine = new SupplierInvoiceItemLine(item, this);
        //                    this.InvoiceItemsList.Insert(invoiceItem);
        //                }
        //                var temp = [];
        //                this.CustomsDocumentsTicket.CustomsDocumentPointers.forEach((pointer) => {
        //                    var invItem: SupplierInvoiceItemLine = this.InvoiceItemsList.Collection.filter(d => d.CounterKey + "" == pointer.Child1EntityId && d.LineNumber + "" == pointer.Child2EntityId)[0];
        //                    if (invItem) {
        //                        temp.push(invItem);
        //                    }
        //                    //if (!this.SelectedInvoiceItems.Collection.includes(invItem)) {
        //                    this.SelectedInvoiceItems.Collection = temp;//.push(invItem);
        //                    //}
        //                });
        //            }
        //        }
        //    });
        //}
    };
    PointersFromInvoicesSelectionComponent.prototype.CancelButtonClicked = function () {
        this.Dispose();
        this.CurrentSession.CloseCurrentWindow();
    };
    PointersFromInvoicesSelectionComponent.prototype.OkButtonClicked = function () {
        //this.parent.SelectedInvoiceItems = this.SelectedInvoiceItems;
        //this.parent.SelectedInvoices = this.SelectedInvoices;
        //this.CustomsDocumentsTicket.ConnectedInvoicesSequences
        var _this = this;
        this.ConnectedInvoiceItems = "";
        //var initallySelected: string[] = this.initialSelectedItems ? this.initialSelectedItems.split(',') : [];
        //var deleted: string[] = this.deletedInvItems.substr(1, this.deletedInvItems.length - 1).split(',');
        //for (var i = 0; i < initallySelected.length; i++) {
        //    if (deleted.indexOf(initallySelected[i]) == -1) {
        //        this.ConnectedInvoiceItems = this.ConnectedInvoiceItems + "," + initallySelected[i];
        //    }
        //}
        this.StaticSelectedInvoiceItems.Collection.forEach(function (item) {
            //if (initallySelected.indexOf(item.rowData.SequenceNumeric.toString()) == -1) {
            _this.ConnectedInvoiceItems = _this.ConnectedInvoiceItems + "," + item.SequenceNumeric;
            // }
        });
        this.ConnectedInvoiceItems = this.ConnectedInvoiceItems.substr(1, this.ConnectedInvoiceItems.length - 1);
        this.ConnectedInvoices = "";
        this.SelectedInvoices.Collection.forEach(function (item) {
            _this.ConnectedInvoices = _this.ConnectedInvoices + "," + item.SequenceNumeric;
        });
        this.ConnectedInvoices = this.ConnectedInvoices.substr(1, this.ConnectedInvoices.length - 1);
        this.Dispose();
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    };
    PointersFromInvoicesSelectionComponent.prototype.ViewInitCompleted = function ($event) {
        //this.SelectedRow = this.ItemsSource.Collection[0];
        //this.OnRowSelected(this.SelectedRow);
        this.SelectedRows = this.SelectedInvoices.Collection;
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        //this.SelectedInvoiceItems.Changed.subscribe((isCollection) => {
        //});
    };
    PointersFromInvoicesSelectionComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        //if (filters == null) {
        //    filters = new ApiQueryFilters();
        //}
        var _this = this;
        if (filters === void 0) { filters = null; }
        //filters.PageSize = take;
        //filters.PageIndex = skip;
        //filters.GetAll = false;
        //filters.GetCount = true;
        //filters.SortBy = "SequenceNumeric";
        //filters.SortDirection = "Ascending";
        //if (this.SelectedRow) {
        //    filters.addAdditionalFilter("DeclarationId", this.SelectedRow.DeclarationId, null, null, "Equals", false, false, false, "string");
        //    filters.addAdditionalFilter("CounterKey", this.SelectedRow.InvoiceCounterKey, null, null, "Equals", false, false, false, "number");
        //}
        //else {
        //    filters.addAdditionalFilter("DeclarationId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        //    //          filters.addAdditionalFilter("CounterKey", null, null, null, "Equals", false, false, false, "number");
        //}
        //return this._entityListService.getExtendedByFilters("Customs.SupplierInvoiceItem", filters);//this.ledgerTransactionListExtendedService.getByFilters(filters);
        var keys = "";
        this.SelectedRows.forEach(function (invoice) {
            keys = keys + "," + invoice.InvoiceCounterKey;
        });
        keys = keys.substr(1, keys.length - 1);
        return new Promise(function (resolve, reject) {
            resolve(_this.supplierInvoiceExtendedListService.GetSupplierInvoiceItemsForInvoices(_this.DeclarationPM.Id, keys, skip, take, getCount));
        });
    };
    PointersFromInvoicesSelectionComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'SequenceNumeric',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.SequenceNumeric"),
            Styles: { width: '50px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TradeAgreementName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.TradeAgreementName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'InvoiceQuantity',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.InvoiceQuantity"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'DeclarationSupplierInvoiceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationSupplierInvoiceListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ItemPrice',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ItemPrice"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'DeclarationSupplierInvoiceListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationSupplierInvoiceListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'OriginCountryName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.OriginCountryName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
    };
    PointersFromInvoicesSelectionComponent.prototype.Dispose = function () {
        //this.SelectedItemChangedEvt.unsubscribe();
        //this.SelectedItemChangedEvt = null;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], PointersFromInvoicesSelectionComponent.prototype, "MenuHeaderchangeevent", void 0);
    PointersFromInvoicesSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PointersFromInvoicesSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], PointersFromInvoicesSelectionComponent);
    return PointersFromInvoicesSelectionComponent;
}());
exports.PointersFromInvoicesSelectionComponent = PointersFromInvoicesSelectionComponent;
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
    Object.defineProperty(SupplierInvoiceLine.prototype, "DeclarationId", {
        get: function () { return this.entity.DeclarationId; },
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
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "OriginCountryName", {
        get: function () { return this.entity.OriginCountryName; },
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
    Object.defineProperty(SupplierInvoiceItemLine.prototype, "CounterKey", {
        get: function () { return this.entity.CounterKey; },
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
                for (var _i = 0, _a = this.Parent.InvoiceItemsList.Collection; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (item != this && item.IsSelected) {
                        item.IsSelected = false;
                    }
                }
                if (!this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                    this.Parent.SelectedInvoiceItems.Insert(this.entity);
                }
            }
            else {
                if (this.Parent.SelectedInvoiceItems.Collection.includes(this.entity)) {
                    this.Parent.SelectedInvoiceItems.Remove(this.entity);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    return SupplierInvoiceItemLine;
}());
exports.SupplierInvoiceItemLine = SupplierInvoiceItemLine;
//# sourceMappingURL=PointersFromInvoicesSelectionComponent.js.map