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
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var CourierMasterService_1 = require("../../../../Customs/Services/Others/CourierMasterService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var CMConnectedDeclarationTabComponent = /** @class */ (function (_super) {
    __extends(CMConnectedDeclarationTabComponent, _super);
    function CMConnectedDeclarationTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CourierMasterService = new CourierMasterService_1.CourierMasterService();
        _this.ObjectTableName = "Customs.CourierMaster";
        _this.DataContext = _this;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.MenuHeaderchangeevent1 = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._TotalConnected = 0;
        _this.columns = null;
        _this.columns1 = null;
        _this.DataSource = {
            pageSize: 10,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.DataSource1 = {
            pageSize: 10,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows1(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.entityPM = entityArgs.EntityPM;
        _this.connectedListIds = new ObservableCollection_1.ObservableCollection([]);
        _this.notConnectedListIds = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Consignment").subscribe(function (response) {
                _this.IsVisibile = true;
                _this.BuildColumns();
                _this.BuildColumns1();
                _this.LoadConnectedItems();
                _this.Listen();
            });
        });
        return _this;
    }
    Object.defineProperty(CMConnectedDeclarationTabComponent.prototype, "ConnectedSearch", {
        get: function () { return this._ConnectedSearch; },
        set: function (value) {
            if (this._ConnectedSearch != value) {
                this._ConnectedSearch = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CMConnectedDeclarationTabComponent.prototype, "NotConnectedSearch", {
        get: function () { return this._NotConnectedSearch; },
        set: function (value) {
            if (this._NotConnectedSearch != value) {
                this._NotConnectedSearch = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CMConnectedDeclarationTabComponent.prototype, "TotalConnected", {
        get: function () { return this.DataSource != null ? this.DataSource.rowCount : 0; },
        set: function (value) {
            if (this._TotalConnected != value) {
                this._TotalConnected = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CMConnectedDeclarationTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.LoadConnectedDeclarationGrid();
                    _this.LoadNotConnectedDeclarationGrid();
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.BuildColumns();
                    _this.LoadConnectedItems();
                }
            });
            this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "COCD") {
                    }
                }
            });
        }
    };
    CMConnectedDeclarationTabComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "MyConnectedCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',
        });
        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.CustomFileNo"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',
        });
        this.columns.push({
            FieldName: 'CourierHAWB',
            DataTypeCode: 'String',
            Display: "שטר מטען בלדר",
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DeclarationNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationNumber"),
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'CustomerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.CustomerName"),
            Styles: { width: '250px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationStatusTypeCode"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });
    };
    CMConnectedDeclarationTabComponent.prototype.BuildColumns1 = function () {
        this.columns1 = [];
        this.columns1.push({
            FieldName: "MyNotConnectedCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '35px' },
            //IsCheckBox: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',
        });
        this.columns1.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.CustomFileNo"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierConnectedDeclarationListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierConnectedDeclarationListTemplate',
        });
        this.columns1.push({
            FieldName: 'CourierHAWB',
            DataTypeCode: 'String',
            Display: "שטר מטען בלדר",
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });
        this.columns1.push({
            FieldName: 'DeclarationNumber',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationNumber"),
            Styles: { width: '120px' },
            IsCustomTemplate: true
        });
        this.columns1.push({
            FieldName: 'CustomerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.CustomerName"),
            Styles: { width: '250px' },
            IsCustomTemplate: true
        });
        this.columns1.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.F.DeclarationStatusTypeCode"),
            Styles: { width: '200px' },
            IsCustomTemplate: true
        });
    };
    CMConnectedDeclarationTabComponent.prototype.LoadConnectedItems = function () {
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    CMConnectedDeclarationTabComponent.prototype.ViewInitCompleted = function ($event) {
        this.LoadConnectedDeclarationGrid();
    };
    CMConnectedDeclarationTabComponent.prototype.ConnectedSearchCompleted = function (searchText) {
        this.ConnectedSearch = searchText;
        this.LoadConnectedDeclarationGrid();
    };
    CMConnectedDeclarationTabComponent.prototype.NotConnectedSearchCompleted = function (searchText) {
        this.NotConnectedSearch = searchText;
        this.LoadNotConnectedDeclarationGrid();
    };
    CMConnectedDeclarationTabComponent.prototype.ViewInitCompleted1 = function ($event) {
        this.LoadNotConnectedDeclarationGrid();
    };
    CMConnectedDeclarationTabComponent.prototype.LoadConnectedDeclarationGrid = function () {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    CMConnectedDeclarationTabComponent.prototype.LoadNotConnectedDeclarationGrid = function () {
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.MenuHeaderchangeevent1.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
    };
    CMConnectedDeclarationTabComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConnectedSearch)) {
            filters.addAdditionalFilter("CourierSearchFields", this.ConnectedSearch, null, null, "Contains", false, false, false, "string");
        }
        return this.CourierMasterService.getPromiseByFilters(filters);
    };
    CMConnectedDeclarationTabComponent.prototype.getRows1 = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NotConnectedSearch)) {
            filters.addAdditionalFilter("CourierSearchFields", this.NotConnectedSearch, null, null, "Contains", false, false, false, "string");
        }
        return this.CourierMasterService.getPromiseByFilters1(filters);
    };
    CMConnectedDeclarationTabComponent.prototype.onCheckBoxChecked = function ($event) {
        if (!this.entityPM.NotConnectedDeclarations) {
            this.entityPM.NotConnectedDeclarations = "";
        }
        if (!$event.IsChecked) {
            if (!this.entityPM.NotConnectedDeclarations.includes($event.rowData.Id)) {
                this.notConnectedListIds.Collection.push($event.rowData.Id);
                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations + $event.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.NotConnectedDeclarations.includes($event.rowData.Id)) {
                this.entityPM.NotConnectedDeclarations = this.entityPM.NotConnectedDeclarations.replace($event.rowData.Id + ",", "");
            }
        }
    };
    CMConnectedDeclarationTabComponent.prototype.onCheckBoxChecked1 = function ($event) {
        if (!this.entityPM.ConnectedDeclarations) {
            this.entityPM.ConnectedDeclarations = "";
        }
        if ($event.IsChecked) {
            if (!this.entityPM.ConnectedDeclarations.includes($event.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations + $event.rowData.Id + ",";
            }
        }
        else {
            if (this.entityPM.ConnectedDeclarations.includes($event.rowData.Id)) {
                this.entityPM.ConnectedDeclarations = this.entityPM.ConnectedDeclarations.replace($event.rowData.Id + ",", "");
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CMConnectedDeclarationTabComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CMConnectedDeclarationTabComponent.prototype, "MenuHeaderchangeevent1", void 0);
    CMConnectedDeclarationTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CMConnectedDeclarationTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CMConnectedDeclarationTabComponent);
    return CMConnectedDeclarationTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CMConnectedDeclarationTabComponent = CMConnectedDeclarationTabComponent;
//# sourceMappingURL=CMConnectedDeclarationTabComponent.js.map