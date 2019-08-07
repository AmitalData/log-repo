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
var SupplierInvoiceExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/SupplierInvoiceExtendedPMService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var SupplierInvoicePM_1 = require("../../../../../Customs/EntityPMs/SupplierInvoicePM");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var SupplierInvoicePMService_1 = require("../../../../../Customs/Services/StandardPMs/SupplierInvoicePMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var EntityListService_1 = require("../../../../../Infrastructure/Services/EntityListService");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var ConfirmWindow_1 = require("../../../../../Controls/Windows/ConfirmWindow");
var CustomsDocumentPointerService_1 = require("../../../../../Customs/Services/Others/CustomsDocumentPointerService");
var DeclarationDisplayOnlyChecks_1 = require("../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var FeatureLocator_1 = require("../../../../../Infrastructure/Utilities/FeatureLocator");
var DeclarationSupplierInvoiceTabComponent = /** @class */ (function (_super) {
    __extends(DeclarationSupplierInvoiceTabComponent, _super);
    function DeclarationSupplierInvoiceTabComponent(entityArgs, CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.ObjectTableName = null;
        _this.DataContext = _this;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService();
        _this.IsVisible = false;
        _this.IsDisplayOnly = false;
        _this.ShowStorageStatusMessage = false;
        _this.DisplayOnlyMessage = "";
        _this.LayoutDirection = 'ltr';
        _this.NumberOfLoadedItems = 500;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.columns = null;
        _this.SelectedRow = null;
        _this.SelectedRowB4Refresh = null;
        _this.DataSource = {
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
        _this.IsAccumulated = false;
        _this.SelectedRow2 = null;
        // this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(response => {
        _this.customsDocumentPointerService = new CustomsDocumentPointerService_1.CustomsDocumentPointerService();
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.InvoiceItems = new ObservableCollection_1.ObservableCollection([]);
        _this.Listen();
        _this._entityListService = new EntityListService_1.EntityListService();
        //this.EntityPM = this.entityArgs.EntityPM;
        _this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        return _this;
        //this.supplierInvoicePMService = new SupplierInvoicePMService();
        //this.ObjectTableName = this.entityArgs.ObjectTableName;
        //this.getSupplierInvoices();
        //});
    }
    DeclarationSupplierInvoiceTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoice").subscribe(function (response) {
                _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(function (response) {
                    _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsMod").subscribe(function (response) {
                        _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemProcesType").subscribe(function (response) {
                            _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsConDeclar").subscribe(function (response) {
                                _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsDescript").subscribe(function (response) {
                                    _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsSerialNum").subscribe(function (response) {
                                        _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsProdIdent").subscribe(function (response) {
                                            _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsLevy").subscribe(function (response) {
                                                _this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvioceItemCertificat").subscribe(function (response) {
                                                    _this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(function (response) {
                                                        _this.IsVisible = true;
                                                        //this.ItemsSource = new ObservableCollection([]);
                                                        //this.InvoiceItems = new ObservableCollection([]);
                                                        _this.BuildColumns();
                                                        _this.supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
                                                        _this.supplierInvoicePMService = new SupplierInvoicePMService_1.SupplierInvoicePMService();
                                                        _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                                                        //this.EntityPM = this.entityArgs.EntityPM;
                                                        //this.getSupplierInvoices();
                                                        //this.DisplayOnlyCheck();
                                                        _this.ReloadMyScreen();
                                                    });
                                                });
                                            });
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        });
    };
    DeclarationSupplierInvoiceTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.ReloadMyScreen();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    setTimeout(function () {
                        _this.ReloadMyScreen();
                    });
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DEIN") {
                        _this.ReloadMyScreen();
                    }
                }
            }));
        }
    };
    DeclarationSupplierInvoiceTabComponent.prototype.ReloadMyScreen = function () {
        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
        this.getSupplierInvoices();
        this.DisplayOnlyCheck();
    };
    DeclarationSupplierInvoiceTabComponent.prototype.BuildColumns = function () {
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
    DeclarationSupplierInvoiceTabComponent.prototype.ViewInitCompleted = function ($event) {
        this.SelectedRow = this.ItemsSource.Collection[0];
        this.OnRowSelected(this.SelectedRow);
    };
    DeclarationSupplierInvoiceTabComponent.prototype.getSupplierInvoices = function () {
        //this.ItemsSource.Clear();
        this.Invoices = this.EntityPM.SupplierInvoices;
        this.ItemsSource.InsertCollection(this.Invoices, true);
        //this.ItemsSource.Collection = this.Invoices;
        //this.ItemsSource.Length = this.Invoices.length;
        //Select last selected row, or first
        if (this.SelectedRowB4Refresh) {
            if (this.SelectedRowB4Refresh == this.lastDeletedItem) { //deleted item
                this.OnRowSelected(this.Invoices[0]);
            }
            else {
                var selectedInvoiceKey = this.SelectedRowB4Refresh.InvoiceCounterKey;
                var selectedInvoice = this.Invoices.filter(function (d) { return d.InvoiceCounterKey == selectedInvoiceKey; })[0];
                this.OnRowSelected(selectedInvoice);
            }
        }
        else
            this.OnRowSelected(this.Invoices[0]);
    };
    DeclarationSupplierInvoiceTabComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
        this.SelectedRowB4Refresh = this.SelectedRow;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
        if (itemComponent) {
            if (itemComponent.IsAccumalated) {
                this.IsAccumulated = true;
                this.IsAccumulatedMessageText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.IsAccumulated");
            }
            else {
                this.IsAccumulated = false;
            }
        }
        //this.DataSource.pageSize = 10;
        //this.DataSource.rowCount = 10;
        //this.DataSource.sortingDir = "Ascending";
        //this.DataSource.getRows(0, 10, "SequenceNumeric", "Ascending", true, null, null);
    };
    DeclarationSupplierInvoiceTabComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "SequenceNumeric";
        filters.SortDirection = "Ascending";
        if (this.SelectedRow) {
            if (this.SelectedRow.IsAccumalated) {
                filters.addAdditionalFilter("IsParent", true, null, null, "Equals", false, false, false, "boolean");
            }
            filters.addAdditionalFilter("DeclarationId", this.SelectedRow.DeclarationId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("CounterKey", this.SelectedRow.InvoiceCounterKey, null, null, "Equals", false, false, false, "number");
        }
        else {
            filters.addAdditionalFilter("DeclarationId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
            //          filters.addAdditionalFilter("CounterKey", null, null, null, "Equals", false, false, false, "number");
        }
        return this._entityListService.getExtendedByFilters("Customs.SupplierInvoiceItem", filters); //this.ledgerTransactionListExtendedService.getByFilters(filters);
    };
    DeclarationSupplierInvoiceTabComponent.prototype.IsPrimarySupplierInvoiceChecked = function (checked, item) {
        var _this = this;
        if (!checked) {
            if (this.EntityPM.PrimaryInvoiceCounterKey == item.InvoiceCounterKey.toString()) {
                this.timerToken = setTimeout(function () {
                    var invoice = _this.EntityPM.SupplierInvoices.filter(function (d) { return d.InvoiceCounterKey == item.InvoiceCounterKey; })[0];
                    invoice.IsPrimarySupplierInvoice = true;
                }, 1);
            }
        }
        else {
            if (this.EntityPM.PrimaryInvoiceCounterKey != item.InvoiceCounterKey.toString()) {
                this.EntityPM.PrimaryInvoiceCounterKey = item.InvoiceCounterKey.toString();
                item.IsPrimarySupplierInvoice = true;
                for (var i = 0; i < this.EntityPM.SupplierInvoices.length; i++) {
                    if (this.EntityPM.SupplierInvoices[i].InvoiceCounterKey.toString() != this.EntityPM.PrimaryInvoiceCounterKey) {
                        this.EntityPM.SupplierInvoices[i].IsPrimarySupplierInvoice = false;
                    }
                    else {
                        this.EntityPM.SupplierInvoices[i].IsPrimarySupplierInvoice = true;
                    }
                }
            }
        }
    };
    DeclarationSupplierInvoiceTabComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);
        for (var _i = 0, _a = this.EntityPM.Consignments; _i < _a.length; _i++) {
            var item_1 = _a[_i];
            for (var _b = 0, _c = item_1.ConsignmentPackages; _b < _c.length; _b++) {
                var line = _c[_b];
                if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                    var errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                    if (!Tools_1.AppTool.IsNullOrEmpty(errorMessage)) {
                        errors.push(errorMessage);
                    }
                }
            }
        }
        if (errors.length > 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }
        else {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicator("");
                this.declarationPMService.update(this.EntityPM).subscribe(function (response) {
                    var declaration = response.Result;
                    _this.CurrentSession.StopBusyIndicator();
                    if (!Tools_1.AppTool.IsNullOrEmpty(declaration)) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
                            _this.EditInvoice(item);
                        }
                    }
                });
            }
            else {
                this.EditInvoice(item);
            }
        }
    };
    DeclarationSupplierInvoiceTabComponent.prototype.EditInvoice = function (item) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        var supplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService_1.SupplierInvoiceExtendedPMService();
        this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.Id, item.InvoiceCounterKey, 0, this.NumberOfLoadedItems, "parent").subscribe(function (response) {
            var windowArgs = {};
            windowArgs.EntityPM = response.Result;
            windowArgs.declarationPM = _this.EntityPM;
            windowArgs.NumberOfLoadedItems = _this.NumberOfLoadedItems;
            var windowTitle = "Supplier Invoice";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 995; // don't change this width!
            logWindow.Height = 600;
            if (!Tools_1.AppTool.IsNullOrEmpty(item.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + item.InvoiceNumber + "-" + _this.EntityPM.DeclarationNumber;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(item.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.DeclarationNumber;
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(item.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + item.InvoiceNumber;
            }
            else if (Tools_1.AppTool.IsNullOrEmpty(item.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
            }
            windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            _this.CD.detach();
            logWindow.WindowClosed.subscribe(function (event) {
                if (event != 'cancel') {
                    _this.RefreshEntity();
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                else {
                    _this.ReloadMyScreen();
                }
                _this.CD.reattach();
            });
            logWindow.IsHideHeader = true;
            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DeclarationSupplierInvoiceTabComponent.prototype.DeleteButtonClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        //confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        //confirmWindow.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        confirmWindow.Width = 300;
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeleteInvoice"));
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.customsDocumentPointerService.GetCheckForPointers(item.DeclarationId, item.InvoiceCounterKey).subscribe(function (myResponse) {
                    var exist = myResponse;
                    if (myResponse.Result) {
                        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        //     confirmWindow.DisplayWariningIconImage();
                        confirmWindow.Width = 400;
                        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                        confirmWindow.Height = 190;
                        confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Cancel");
                        confirmWindow.ShowWarningImage = true;
                        confirmWindow.ShowNoButton;
                        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.InvoiceRelatedPoiner"));
                        confirmWindow.WindowClosed.subscribe(function (event) {
                            if (confirmWindow.Yes) {
                                _this.DeleteSelected(item);
                            }
                            else if (confirmWindow.No) {
                            }
                        });
                    }
                    else {
                        _this.DeleteSelected(item);
                    }
                });
            }
            else if (confirmWindow.No) {
            }
        });
    };
    DeclarationSupplierInvoiceTabComponent.prototype.DeleteSelected = function (item) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("");
        this.lastDeletedItem = item;
        var SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
            SaveCompletedEvent.unsubscribe();
            _this.supplierInvoiceExtendedPMService.delete(item.DeclarationId, item.InvoiceCounterKey).subscribe(function (myResponse) {
                if (!myResponse.HasError) {
                    _this.ItemsSource.Remove(item);
                    //if (Number(this.EntityPM.PrimaryInvoiceCounterKey) == item.InvoiceCounterKey) {
                    //    this.EntityPM.PrimaryInvoiceCounterKey = this.ItemsSource.Collection[0].InvoiceCounterKey;
                    //}
                    //   this.getSupplierInvoices();
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        });
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    DeclarationSupplierInvoiceTabComponent.prototype.Add = function () {
        var _this = this;
        if (this.IsDisplayOnly)
            return;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        if (errors.length > 0) {
            //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }
        else {
            if (this.EntityPM.IsDirty) {
                this.declarationPMService.update(this.EntityPM).subscribe(function (response) {
                    var declaration = response.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(declaration))
                        _this.NewInvoice();
                    else
                        console.log("[!] No response for saving declaration, adding inice aborted.", response);
                });
            }
            else {
                this.NewInvoice();
            }
        }
    };
    DeclarationSupplierInvoiceTabComponent.prototype.NewInvoice = function () {
        var _this = this;
        var itemPM = new SupplierInvoicePM_1.SupplierInvoicePM();
        itemPM.DeclarationId = this.EntityPM.Id;
        itemPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Declaration'; })[0];
        this.accumulationFeature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.Code == "ACCUMULATION") && f.ObjectTableId == table.Id; })[0];
        if (this.accumulationFeature == null) {
            itemPM.AccumalationStateCode = "3";
        }
        else {
            itemPM.AccumalationStateCode = "1";
        }
        itemPM.InvoiceCounterKey = 0;
        // [!] I think its not nessecary  !!!
        //var itemList = new SupplierInvoiceList();
        //itemPM.DeclarationId = this.EntityPM.Id;
        //itemPM.Tenant = SessionLocator.Tenant;
        var windowArgs = {};
        if (this.EntityPM.SupplierInvoices.length > 0) {
            windowArgs.InsuranceAmountEnabled = true;
            windowArgs.InsuranceCurrencyEnabled = true;
            windowArgs.PercentageEnabled = true;
            itemPM.IsPrimarySupplierInvoice = true;
        }
        else {
            windowArgs.InsuranceAmountEnabled = false;
            windowArgs.InsuranceCurrencyEnabled = false;
            windowArgs.PercentageEnabled = false;
        }
        // Show Window
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.NewInvoice");
        windowArgs.EntityPM = itemPM;
        windowArgs.declarationPM = this.EntityPM;
        windowArgs.IsDisplayOnly = this.IsDisplayOnly;
        windowArgs.IsNewEntity = true;
        windowArgs.WindowTitle = windowTitle;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 995; // don't change this width!
        logWindow.Height = 600;
        //  logWindow.Title = windowTitle;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.CD.reattach();
            _this.RefreshEntity();
            _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
            _this.getSupplierInvoices();
        });
        this.CD.detach();
        logWindow.IsHideHeader = true;
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
        //}
    };
    DeclarationSupplierInvoiceTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    DeclarationSupplierInvoiceTabComponent.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            //this.SetScreenFieldsEditability();
            //DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            if (this.EntityPM.SupplierInvoices != null) {
                for (var i = 0; i < this.EntityPM.SupplierInvoices.length; i++) {
                    this.EntityPM.SupplierInvoices[i].UIProperties.SetEnabled("IsPrimarySupplierInvoice", "Customs.SupplierInvoice", !this.IsDisplayOnly);
                }
            }
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
        }
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe(function (response) {
            var displayOnlyCheckResult = response.Result;
            _this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (_this.IsDisplayOnly) {
                _this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (_this.EntityPM.StorageStatusCode) {
                _this.ShowStorageStatusMessage = true;
                _this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + _this.EntityPM.StorageStatusName;
            }
            if (_this.EntityPM.SupplierInvoices != null) {
                for (var i = 0; i < _this.EntityPM.SupplierInvoices.length; i++) {
                    _this.EntityPM.SupplierInvoices[i].UIProperties.SetEnabled("IsPrimarySupplierInvoice", "Customs.SupplierInvoice", !_this.IsDisplayOnly);
                }
            }
        });
    };
    DeclarationSupplierInvoiceTabComponent.prototype.OnRowSelected2 = function (CurrentRow) {
        this.SelectedRow2 = CurrentRow.rowData;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DeclarationSupplierInvoiceTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    DeclarationSupplierInvoiceTabComponent = __decorate([
        core_1.Component({
            selector: 'DeclarationSupplierInvoiceTabComponent',
            moduleId: module.id,
            templateUrl: './DeclarationSupplierInvoiceTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef])
    ], DeclarationSupplierInvoiceTabComponent);
    return DeclarationSupplierInvoiceTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationSupplierInvoiceTabComponent = DeclarationSupplierInvoiceTabComponent;
//export class SupplierInvoiceLine extends BaseComponent {
//    public SupplierInvoicePM: SupplierInvoicePM = null;
//    public ObjectTableName = "Customs.SupplierInvoice";
//    public DataContext = this;
//    constructor(private invoice: SupplierInvoicePM,
//        private parent: DeclarationSupplierInvoiceTabComponent) {
//        super();
//        this.EntityPM = this.parent.EntityPM;
//        this.SupplierInvoicePM = invoice;
//    }
//    get DeclarationId() { return this.invoice.DeclarationId; }
//    set DeclarationId(value: string) {
//        if (this.invoice.DeclarationId != value) {
//            this.invoice.DeclarationId = value;
//        }
//    }
//    get InvoiceCounterKey() { return this.invoice.InvoiceCounterKey; }
//    set InvoiceCounterKey(value: number) {
//        if (this.invoice.InvoiceCounterKey != value) {
//            this.invoice.InvoiceCounterKey = value;
//        }
//    }
//    get SequenceNumeric() { return this.invoice.SequenceNumeric; }
//    set SequenceNumeric(value: number) {
//        if (this.invoice.SequenceNumeric != value) {
//            this.invoice.SequenceNumeric = value;
//        }
//    }
//    get InvoiceNumber() { return this.invoice.InvoiceNumber; }
//    set InvoiceNumber(value: string) {
//        if (this.invoice.InvoiceNumber != value) {
//            this.invoice.InvoiceNumber = value;
//        }
//    }
//    get IssueDate() { return this.invoice.IssueDate; }
//    set IssueDate(value: Date) {
//        if (this.invoice.IssueDate != value) {
//            this.invoice.IssueDate = value;
//        }
//    }
//    get IncotermCode() { return this.invoice.IncotermCode; }
//    set IncotermCode(value: string) {
//        if (this.invoice.IncotermCode != value) {
//            this.invoice.IncotermCode = value;
//        }
//    }
//    get InvoiceCurrencyTypeCode() { return this.invoice.InvoiceCurrencyTypeCode; }
//    set InvoiceCurrencyTypeCode(value: string) {
//        if (this.invoice.InvoiceCurrencyTypeCode != value) {
//            this.invoice.InvoiceCurrencyTypeCode = value;
//        }
//    }
//    get IssueCountryName() { return this.invoice.IssueCountryName; }
//    set IssueCountryName(value: string) {
//        if (this.invoice.IssueCountryName != value) {
//            this.invoice.IssueCountryName = value;
//        }
//    }
//    get VendorName() { return this.invoice.VendorName; }
//    set VendorName(value: string) {
//        if (this.invoice.VendorName != value) {
//            this.invoice.VendorName = value;
//        }
//    }
//    get PreferenceDocumentTypeName() { return this.invoice.PreferenceDocumentTypeName; }
//    set PreferenceDocumentTypeName(value: string) {
//        if (this.invoice.PreferenceDocumentTypeName != value) {
//            this.invoice.PreferenceDocumentTypeName = value;
//        }
//    }
//    get InsruanceCurrencyTypeCode() { return this.invoice.InsruanceCurrencyTypeCode; }
//    set InsruanceCurrencyTypeCode(value: string) {
//        if (this.invoice.InsruanceCurrencyTypeCode != value) {
//            this.invoice.InsruanceCurrencyTypeCode = value;
//        }
//    }
//    get InsuranceAmount() { return this.invoice.InsuranceAmount; }
//    set InsuranceAmount(value: number) {
//        if (this.invoice.InsuranceAmount != value) {
//            this.invoice.InsuranceAmount = value;
//        }
//    }
//    get InvoiceAmount() { return this.invoice.InvoiceAmount; }
//    set InvoiceAmount(value: number) {
//        if (this.invoice.InvoiceAmount != value) {
//            this.invoice.InvoiceAmount = value;
//        }
//    }
//    private isPrimarySupplierInvoice: boolean = false;
//    get IsPrimarySupplierInvoice() { return this.invoice.IsPrimarySupplierInvoice; }
//    set IsPrimarySupplierInvoice(value: boolean) {
//        if (this.invoice.IsPrimarySupplierInvoice != value) {
//            this.invoice.IsPrimarySupplierInvoice = value;
//        }
//    }
//}
//# sourceMappingURL=DeclarationSupplierInvoiceTabComponent.js.map