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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var InterfaceManagementComponent = /** @class */ (function () {
    function InterfaceManagementComponent() {
        var _this = this;
        this.DataContext = this;
        this.ObjectTableName = "Customs.InterfaceManagement";
        this.columns = null;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._stratSearch = true;
        this.MenuHeaderchangeevent = new core_1.EventEmitter();
        this.onQueryChangeEvent = new core_1.EventEmitter();
        this._IsLoaded = false;
        this.DataSource = {
            pageSize: 10,
            rowCount: null,
            //SortData("RequestCreateDate", "Descending", false, false);
            sortingCol: "",
            sortingDir: "",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        this._entityListService = new EntityListService_1.EntityListService();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
        });
    }
    InterfaceManagementComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._IsLoaded = true;
            _this.BuildColumns();
            _this.RefreshBtnClick();
        });
    };
    InterfaceManagementComponent.prototype.BackButtonClicked = function () {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }
    };
    InterfaceManagementComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'Code',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.InterfaceManagement.F.Code"),
            Styles: { width: '75px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'Description',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.InterfaceManagement.F.Description"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'InOut',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.InterfaceManagement.F.InOut"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'SendOptionsCode',
            DataTypeCode: 'Date',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.InterfaceManagement.F.DefaultSendOptionsCode"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
        });
        this.columns.push({
            FieldName: 'DefaultPriority',
            DataTypeCode: 'Date',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.InterfaceManagement.F.DefaultPriority"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
        });
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'HasDefinition',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.InterfaceManagement.F.HasDefinition"),
            Styles: { width: '80px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'InterfaceManagementsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/InterfaceManagementsListTemplate',
            FieldName: 'SignatureTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.InterfaceManagement.F.SignatureTypeName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true
        });
    };
    InterfaceManagementComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        var _this = this;
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "Code";
        filters.SortDirection = "Ascending"; //"Descending";
        //filters.SortBy = "CustomsName";//"Id";
        //filters.SortDirection = "Descending";//"Descending";
        if (!Tools_1.AppTool.IsNullOrEmpty(this._SearchText)) {
            filters.addAdditionalFilter("SearchFields", this._SearchText, null, null, "Contains", false, false, false, "string");
        }
        /// filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        var myout = this._entityListService
            //.getExtendedByFilters("Customs.InterfaceManagement", filters);
            .getByFilters("Customs.InterfaceManagement", filters);
        myout.then(function (res) {
            _this._stratSearch = false;
            //this.CurrentSession.StopBusyIndicator();
        });
        return myout;
    };
    InterfaceManagementComponent.prototype.onSearchTextChangeEvent = function (text) {
        this._SearchText = text;
        this.RefreshBtnClick();
    };
    InterfaceManagementComponent.prototype.ItemClicked = function (item) {
    };
    InterfaceManagementComponent.prototype.onRowSelected = function (selected) {
        var item = selected.rowData;
        var window = new LogitudeWindow_1.LogitudeWindow();
        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.EditInterfaceManagement");
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = { SelectedItem: item };
        logWindow.Width = 750;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        //logWindow.Show('./Customs/Components/Maintenance/AddEditInterfaceManagementComponent');
        logWindow.Show('./CustomsModules/CustomsMaintenance/Components/AddEditInterfaceManagementComponent');
    };
    InterfaceManagementComponent.prototype.RefreshBtnClick = function () {
        var _this = this;
        this._stratSearch = true;
        //this.CurrentSession.StartBusyIndicator("");
        setTimeout(function () {
            _this.MenuHeaderchangeevent.emit({ Filters: _this.filterAgrs, IgnoreFilter: false });
        }, 10);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], InterfaceManagementComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], InterfaceManagementComponent.prototype, "onQueryChangeEvent", void 0);
    InterfaceManagementComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './InterfaceManagementComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], InterfaceManagementComponent);
    return InterfaceManagementComponent;
}());
exports.InterfaceManagementComponent = InterfaceManagementComponent;
//# sourceMappingURL=InterfaceManagementComponent.js.map