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
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var DeclarationExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DeclarationQueryComponent = /** @class */ (function (_super) {
    __extends(DeclarationQueryComponent, _super);
    function DeclarationQueryComponent() {
        var _this = _super.call(this) || this;
        _this.entityListService = new EntityListService_1.EntityListService();
        _this.DataContext = _this;
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.declarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            //sortingCol: "CreateDateTime",
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.columns = null;
        _this.SelectedRow = null;
        _this.CurrentSession.StartBusyIndicatorLoading();
        _this.BuildColumns();
        return _this;
        //this.onQueryChangeEvent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    }
    DeclarationQueryComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.DeclarationPM;
        this.CustomerId = this.EntityPM.CustomerId;
        var today = new Date();
        var lastmonth = today.setMonth(today.getMonth() - 1);
        this.TaxationDateTime = new Date(lastmonth);
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
    };
    Object.defineProperty(DeclarationQueryComponent.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) {
            if (this.customerId != value) {
                this.customerId = value;
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationQueryComponent.prototype, "DepartmentId", {
        get: function () { return this.departmentId; },
        set: function (value) {
            if (this.departmentId != value) {
                this.departmentId = value;
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationQueryComponent.prototype, "DeclarationOfficeCode", {
        get: function () { return this.declarationOfficeCode; },
        set: function (value) {
            if (this.declarationOfficeCode != value) {
                this.declarationOfficeCode = value;
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationQueryComponent.prototype, "PaymentDate", {
        get: function () { return this.paymentDate; },
        set: function (value) {
            if (this.paymentDate != value) {
                this.paymentDate = value;
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationQueryComponent.prototype, "TaxationDateTime", {
        get: function () { return this.taxationDateTime; },
        set: function (value) {
            if (this.taxationDateTime != value) {
                this.taxationDateTime = value;
                this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
            }
        },
        enumerable: true,
        configurable: true
    });
    DeclarationQueryComponent.prototype.Search = function (value) {
        this.searchValue = value;
        this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters_1.ApiQueryFilters() });
    };
    DeclarationQueryComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.GetAll = false;
        filters.GetCount = true;
        filters.PageSize = 20;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.TaxationDateTime)) {
            var todayDate = new Date();
            var fromDate = new Date(this.TaxationDateTime.getFullYear(), this.TaxationDateTime.getMonth(), this.TaxationDateTime.getDate(), 0, 0, 0);
            var todayDate = new Date(todayDate.setHours(23, 59, 59, 59));
            console.log("TaxationDateTime: ", fromDate, todayDate);
            filters.addAdditionalFilter("TaxationDateTime", fromDate, todayDate, null, "Between", false, false, false, "Date", false);
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentDate)) {
            var fromDate = new Date();
            fromDate.setUTCDate(this.PaymentDate.getUTCDate());
            fromDate.setUTCMonth(this.PaymentDate.getUTCMonth());
            fromDate.setUTCFullYear(this.PaymentDate.getUTCFullYear());
            fromDate.setHours(0);
            fromDate.setMinutes(0);
            fromDate.setSeconds(0);
            fromDate.setMilliseconds(0);
            var filterValue = new Date();
            filterValue.setUTCDate(this.PaymentDate.getUTCDate());
            filterValue.setUTCMonth(this.PaymentDate.getUTCMonth());
            filterValue.setUTCFullYear(this.PaymentDate.getUTCFullYear());
            filterValue.setHours(23);
            filterValue.setMinutes(59);
            filters.addAdditionalFilter("PaymentDate", fromDate, filterValue, null, "Between", false, false, false, "Date", false);
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
            filters.addAdditionalFilter("CustomerId", this.CustomerId, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DepartmentId)) {
            filters.addAdditionalFilter("DepartmentId", this.DepartmentId, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DeclarationOfficeCode)) {
            filters.addAdditionalFilter("DeclarationOfficeCode", this.DeclarationOfficeCode, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.searchValue)) {
            filters.addAdditionalFilter("SearchFields", this.searchValue, null, null, "Contains", false, false, false, "string");
        }
        filters.addAdditionalFilter("CustomFileNo", this.EntityPM.CustomFileNo, null, null, "NotEqual", false, false, false, "string");
        this.CurrentSession.StopBusyIndicator();
        return this.entityListService.getByFilters("Customs.Declaration", filters);
    };
    DeclarationQueryComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'TaxationDateTime',
            DataTypeCode: 'DateTime',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.Declaration.F.TaxationDateTime'),
            Styles: { width: '120px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.Declaration.F.CustomFileNo'),
            Styles: { width: '80px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CustomerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.Declaration.F.CustomerName'),
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DeclarationOfficeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationOfficeName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
        });
        this.columns.push({
            FieldName: 'DeclarationNumber',
            DataTypeCode: 'string',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationNumber'),
            Styles: { width: '105px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'ProcedureCurrentName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.Declaration.F.ProcedureCurrentName'),
            IsCustomTemplate: true,
            Styles: { width: '150px' },
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
        });
        this.columns.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate('Customs.Declaration.F.DeclarationStatusTypeName'),
            Styles: { width: '90px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'DeclarationQueryListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/DeclarationQueryListTemplate',
        });
    };
    DeclarationQueryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    DeclarationQueryComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.SelectedRow) {
            var confirm_1 = new ConfirmWindow_1.ConfirmWindow();
            confirm_1.WindowClosed.subscribe(function (event) {
                if (confirm_1.Yes) {
                    _this.declarationExtendedListService.GetSupplierInvoiceItemsCount(_this.SelectedRow.Id).subscribe(function (response) {
                        if (response) {
                            if (!response.HasError) {
                                if (response.Result) {
                                    var msg = new MessageWindow_1.MessageWindow();
                                    msg.Show("ההצהרה מכילה יותר מ-1000 פריטים, לא ניתן להעתיק אותה");
                                }
                                else {
                                    _this.CurrentSession.StartBusyIndicator("");
                                    _this.declarationExtendedListService
                                        .PutCopyDeclaration(_this.SelectedRow.Id, _this.EntityPM.Id, _this.EntityPM.Tenant)
                                        .subscribe(function (response) {
                                        if (response) {
                                            if (!response.HasError) {
                                                //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                                _this.CurrentSession.StopBusyIndicator();
                                                _this.CurrentSession.CloseCurrentWindowEmit(null);
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    });
                }
            });
            confirm_1.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.CopyData") + " " + this.SelectedRow.CustomFileNo + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ToFile") + " " + this.EntityPM.CustomFileNo + " ?");
        }
        else {
            var msg = new MessageWindow_1.MessageWindow();
            msg.RTL = true;
            msg.Show("נא לבחר הצהרה");
        }
    };
    DeclarationQueryComponent.prototype.OnRowSelected = function (item) {
        this.SelectedRow = item.rowData;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DeclarationQueryComponent.prototype, "onQueryChangeEvent", void 0);
    DeclarationQueryComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationQueryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeclarationQueryComponent);
    return DeclarationQueryComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationQueryComponent = DeclarationQueryComponent;
//# sourceMappingURL=DeclarationQueryComponent.js.map