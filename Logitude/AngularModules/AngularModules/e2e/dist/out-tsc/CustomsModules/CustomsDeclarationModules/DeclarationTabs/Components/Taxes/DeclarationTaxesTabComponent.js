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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
//import { PhysicalCheckList } from '../../../../../Customs/EntityLists/PhysicalCheckList';
//import { PhysicalCheckPMService } from '../../../../../Customs/Services/StandardPMs/PhysicalCheckPMService';
var DeclarationExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var ApiQueryFilters_1 = require("../../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../../../Infrastructure/Services/EntityListService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DeclarationTaxesTabComponent = /** @class */ (function () {
    function DeclarationTaxesTabComponent(entityArgs, CD, EntityResourceService) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.CD = CD;
        this.EntityResourceService = EntityResourceService;
        this.EntityPM = null;
        this.ObjectTableName = "Customs.Declaration";
        this.MenuHeaderchangeevent = new core_1.EventEmitter();
        //public physicalCheckList: PhysicalCheckList[] = [];
        this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService;
        //private physicalCheckPMService: PhysicalCheckPMService = new PhysicalCheckPMService;
        this.IsVisible = false;
        this.TaxBaseAmountTotal = 0;
        this.TaxToPayTotal = 0;
        this.FooterMethods = 0;
        this.TotalAmountTotal = 0;
        this.DeferredTaxAmountTotal = 0;
        this.IsCloseButtonVisibile = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.columns = null;
        this.ShowErrorMessage = false;
        this._IsReloading = false;
        this.ErrorMessage = null;
        this.SelectedRow2 = null;
        this.DataSource = {
            pageSize: 10,
            rowCount: null,
            sortingCol: "ClassificationCode",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        this.isOk = true;
        ///public SelectedRow: //SupplierInvoicePM 
        this.any = null;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsTax").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsExchangeRate").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationTax").subscribe(function (response) {
                            _this.EntityPM = _this.entityArgs.EntityPM;
                            _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                            _this._entityListService = new EntityListService_1.EntityListService();
                            _this.Listen();
                            _this.CurrencyRatesView = new ObservableCollection_1.ObservableCollection([]);
                            _this.TaxesObslist = new ObservableCollection_1.ObservableCollection([]);
                            _this.OnEditTabSelected();
                            _this.BuildColumns();
                        });
                    });
                });
            });
        });
    }
    DeclarationTaxesTabComponent.prototype.ngOnDestroy = function () {
        console.log("DeclarationTaxesTabComponent:ngOnDestroy");
        this.entityArgs = null;
        this.CD = null;
    };
    DeclarationTaxesTabComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItem").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemsTax").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsExchangeRate").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationTax").subscribe(function (response) {
                            _this.EntityPM = args.EntityPM;
                            _this.EntityId = _this.EntityPM.Id;
                            _this.CIFValue = _this.EntityPM.CIFValue;
                            _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                            _this._entityListService = new EntityListService_1.EntityListService();
                            _this.Listen();
                            _this.CurrencyRatesView = new ObservableCollection_1.ObservableCollection([]);
                            _this.TaxesObslist = new ObservableCollection_1.ObservableCollection([]);
                            _this.IsCloseButtonVisibile = true;
                            _this.OnEditTabSelected();
                            _this.BuildColumns();
                            _this.CD.detectChanges();
                        });
                    });
                });
            });
        });
    };
    DeclarationTaxesTabComponent.prototype.ngOnInit = function () {
        if (this.entityArgs.EntityPM) {
            this.EntityPM = this.entityArgs.EntityPM;
        }
    };
    DeclarationTaxesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.OnEditTabSelected();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DETX") {
                        _this.OnEditTabSelected();
                    }
                }
            }));
        }
    };
    DeclarationTaxesTabComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'ClassificationCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItem.F.ClassificationCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'InvoiceNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoice.F.InvoiceNumber"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TaxTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxTypeCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TradeAgreementTypeCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TradeAgreementTypeCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TaxBaseAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxBaseAmount"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            //<TextBlock VerticalAlignment= "Center" Text= "{Binding TaxBaseAmount,StringFormat=\{0:N2\}}" />
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
        });
        this.columns.push({
            FieldName: 'TaxRate',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxRate"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TaxToPay',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.TaxAmount"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TaxAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationTax.F.TaxToPay"),
            Styles: { width: '90px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DeferedTaxAmount',
            DataTypeCode: 'Number',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.SupplierInvoiceItemsTax.F.DeferedTaxAmount"),
            Styles: { width: '100px' },
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'EditButton',
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '30px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'SupplierInvoiceItemsTaxListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/SupplierInvoiceItemsTaxListTemplate',
        });
    };
    DeclarationTaxesTabComponent.prototype.GetSupplierInvoiceCurrencyAsync = function () {
        var _this = this;
        this.CurrencyRatesView.Clear();
        this._DeclarationExtendedListService.GetCurrenciesCodesForDeclaration(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe(function (res) {
            var aSupplierInvoiceCurrency = res.Result;
            _this.CurrencyRatesView.InsertCollection(aSupplierInvoiceCurrency);
            _this.IsVisible = true;
        });
    };
    DeclarationTaxesTabComponent.prototype.OnEditTabSelected = function () {
        //if (tabCode == "DETX") {
        if (this.EntityPM != null) {
            this.ViewInitCompleted({});
            this.TaxesObslist.Clear();
            this.TaxesObslist.InsertCollection(this.EntityPM.DeclarationTaxes);
            this.GetSupplierInvoiceCurrencyAsync();
            this.RefreshProperties();
            this.ReloadTaxes();
            //return;
            //}
            this.BuildTaxesObsList();
            this.RefreshProperties();
        }
        //this.loaded = true;
        //}
    };
    DeclarationTaxesTabComponent.prototype.BuildTaxesObsList = function () {
        var _this = this;
        this.DeferredTaxAmountTotal = this.TotalAmountTotal = this.TaxToPayTotal = this.TaxBaseAmountTotal = 0;
        this.TaxesObslist.Collection.forEach(function (item) {
            _this.TaxBaseAmountTotal += item.TaxBaseAmount;
            _this.TaxToPayTotal += item.TaxToPay;
            _this.TotalAmountTotal += item.TotalAmount;
            _this.DeferredTaxAmountTotal += item.DeferredTaxAmount;
        });
        var headerH = 27;
        var rowH = 26;
        //if ((this.TaxesObslist.Length * 30) + 30 < 123) {
        //    this.FooterMethods = (this.TaxesObslist.Length * 30) + 30;
        //}
        //else {
        //    this.FooterMethods = 123;
        //}
        //this.FooterMethods += 10;
        var top = headerH + (this.TaxesObslist.Length * 26) + 2;
        this.FooterMethods = top;
        console.log(this.FooterMethods);
    };
    DeclarationTaxesTabComponent.prototype.RefreshProperties = function () {
        this.ShowErrorMessage = false;
        if (this.EntityPM.IsChanged && this.EntityPM.DeclarationTaxes.length > 0) {
            console.log("ShowErrorMessage");
            this.ShowErrorMessage = true;
            this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationTaxChanged");
        }
        else {
            //MessageBorderVisibility = Visibility.Collapsed;
            this.ErrorMessage = null;
        }
        if (this.EntityPM.DeclarationTaxes.length > 0) {
            this.ShowColumnFooter = true;
        }
    };
    DeclarationTaxesTabComponent.prototype.ReloadTaxes = function () {
        if (!this._IsReloading) {
            //this._DeclarationExtendedListService.      
            //GetSingleDeclarationByNumber
            //.GetDeclarationTaxesByDeclarationId
            //LoadOperation op = context.Load(context.GetDeclarationTaxesByDeclarationIdQuery(entityPM.Id, TenantContext.Current.Id));
            //op.Completed += op_Completed;
            this._IsReloading = true;
        }
    };
    DeclarationTaxesTabComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    Object.defineProperty(DeclarationTaxesTabComponent.prototype, "ErrorMessageNotUsed", {
        get: function () {
            if (this.EntityPM.IsChanged && this.EntityPM.DeclarationTaxes.length > 0) {
                return TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeclarationTaxChanged");
            }
            else {
                //MessageBorderVisibility = Visibility.Collapsed;
                return null;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationTaxesTabComponent.prototype, "CIFValue", {
        get: function () {
            return this.EntityPM == null ? null : this.EntityPM.CIFValue;
        },
        set: function (value) {
            this.EntityPM.CIFValue = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationTaxesTabComponent.prototype, "DealValueWithoutFactor", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.DealValueWithoutFactor; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationTaxesTabComponent.prototype, "TotalTax", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.TotalTax; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationTaxesTabComponent.prototype, "PlatformFee", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.PlatformFee; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationTaxesTabComponent.prototype, "DealValue", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.DealValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationTaxesTabComponent.prototype, "LoadingFactor", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.LoadingFactor; },
        enumerable: true,
        configurable: true
    });
    DeclarationTaxesTabComponent.prototype.OnRowSelected2 = function (CurrentRow) {
        this.SelectedRow2 = CurrentRow.rowData;
    };
    DeclarationTaxesTabComponent.prototype.ViewInitCompleted = function ($event) {
        //this.SelectedRow = this.ItemsSource.Collection[0];
        //this.OnRowSelected(this if (this.EntityPM) {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    DeclarationTaxesTabComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "ClassificationCode";
        filters.SortDirection = "Ascending";
        if (this.EntityPM) {
            filters.addAdditionalFilter("DeclarationId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        }
        else {
            filters.addAdditionalFilter("DeclarationId", this.EntityId, null, null, "Equals", false, false, false, "string");
        }
        return this._entityListService.getExtendedByFilters("Customs.SupplierInvoiceItemsTax", filters); //this.ledgerTransactionListExtendedService.getByFilters(filters);
    };
    DeclarationTaxesTabComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DeclarationTaxesTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    DeclarationTaxesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationTaxesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], DeclarationTaxesTabComponent);
    return DeclarationTaxesTabComponent;
}());
exports.DeclarationTaxesTabComponent = DeclarationTaxesTabComponent;
var SupplierInvoiceCurrency = /** @class */ (function () {
    function SupplierInvoiceCurrency() {
    }
    return SupplierInvoiceCurrency;
}());
exports.SupplierInvoiceCurrency = SupplierInvoiceCurrency;
//# sourceMappingURL=DeclarationTaxesTabComponent.js.map