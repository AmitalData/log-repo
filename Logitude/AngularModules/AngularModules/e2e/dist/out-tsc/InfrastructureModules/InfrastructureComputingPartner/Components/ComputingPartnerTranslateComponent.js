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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var ComputingPartnerTranslateComponent = /** @class */ (function (_super) {
    __extends(ComputingPartnerTranslateComponent, _super);
    function ComputingPartnerTranslateComponent(_entityListService) {
        var _this = _super.call(this) || this;
        _this._entityListService = _entityListService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SearchFieldchangeevent = new core_1.EventEmitter();
        _this.CustomBackFromEditevent = new core_1.EventEmitter();
        _this.SearchText = "Search";
        _this.DataSource = {
            pageSize: 20,
            rowCount: null,
            sortingDir: "Ascending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.columns = [];
        _this.items = [];
        _this.CurrentSession.PseventRowSelectEvent.subscribe(function (res) {
            if (res.Name == "btnComponentComputingPartnerEdit") {
                var filters;
                if (filters == null) {
                    filters = new CommonDomainService_1.CustomApiQueryFilters();
                }
                var table = window.ObjectTables.filter(function (d) { return d.Name === res.Value.ObjectTableName; })[0];
                if (table)
                    filters.ClinetName = table.ClientModuleName;
                filters.computingPartnerId = res.Value.ComputingPartnerId;
                filters.objectTableId = res.Value.ObjectTableId;
                filters.objectTableName = res.Value.ObjectTableName;
                filters.ComputingPartnerName = res.Value.ComputingPartnerName;
                var domainService = new CommonDomainService_1.CommonDomainService();
                var items = [];
                domainService.getNoneZeroTenantTranslation(res.Value.ComputingPartnerId, res.Value.ObjectTableId, res.Value.OurCode).subscribe(function (p) {
                    var item = {};
                    item.ComputingPartnerId = p.Result.ComputingPartnerId != null ? p.Result.ComputingPartnerId : res.Value.ComputingPartnerId;
                    item.OurCode = p.Result.OurCode != null ? p.Result.OurCode : res.Value.OurCode;
                    item.PartnerCode = p.Result.PartnerCode != null ? p.Result.PartnerCode : res.Value.PartnerCode;
                    item.Id = p.Result.Id != null ? p.Result.Id : res.Value.Id;
                    item.ObjectTableId = p.Result.ObjectTableId != null ? p.Result.ObjectTableId : res.Value.ObjectTableId;
                    item.CreatedByUserId = p.Result.CreatedByUserId != null ? p.Result.CreatedByUserId : res.Value.CreatedByUserId;
                    item.UpdatedByUserId = p.Result.UpdatedByUserId != null ? p.Result.UpdatedByUserId : res.Value.UpdatedByUserId;
                    item.UpdateDate = p.Result.UpdateDate != null ? p.Result.UpdateDate : res.Value.UpdateDate;
                    item.CreateDate = p.Result.CreateDate != null ? p.Result.CreateDate : res.Value.CreateDate;
                    item.Name = p.Result.Name != null ? p.Result.Name : res.Value.Name;
                    if (SessionLocator_1.SessionLocator.Tenant != 0)
                        item.DefaultTranslationPartnerCode = p.Result.DefaultTranslationCode != null ? p.Result.DefaultTranslationCode : res.Value.DefaultTranslationCode;
                    items.push({ rowData: item, rowIndex: res.RowIndex });
                    _this.CustomBackFromEditevent.emit(items);
                });
            }
        });
        return _this;
    }
    ComputingPartnerTranslateComponent.prototype.TextChanged = function (searchtext) {
        if (searchtext == null)
            searchtext = "";
        this.searchFields = searchtext;
        this.SearchFieldchangeevent.emit(this.searchFields);
    };
    ComputingPartnerTranslateComponent.prototype.SetWindowArgs = function (entity) {
        this.EntityPM = entity;
        this.ObjectTableName = this.EntityPM.ObjectTableName;
        this.BuildColumns();
    };
    ComputingPartnerTranslateComponent.prototype.BuildColumns = function () {
        this.columns.push({
            FieldName: "OurCode",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Our Code',
            Styles: { width: '80px' },
        });
        this.columns.push({
            FieldName: "Name",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Name',
            Styles: { width: '100px' },
        });
        if (SessionLocator_1.SessionLocator.Tenant != 0) {
            this.columns.push({
                FieldName: "DefaultTranslationPartnerCode",
                DataTypeCode: 'String',
                IsCustomTemplate: true,
                Display: 'Default Translation',
                Styles: { width: '180px' },
            });
        }
        this.columns.push({
            FieldName: "PartnerCode",
            DataTypeCode: 'String',
            IsCustomTemplate: true,
            Display: 'Partner Code',
            Styles: { width: '180px' },
        });
        this.columns.push({
            FieldName: "MoreDetails",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '80px' },
            HtmlListComponentName: 'btnComponent',
            HtmlListComponentUrl: './InfrastructureModules/InfrastructureComputingPartner/Components/QueryColumnsComponents/btnComponentComputingPartner',
        });
        this.columns.push({
            FieldName: "Edit",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '30px' },
            HtmlListComponentName: 'btnComponentComputingPartnerEdit',
            HtmlListComponentUrl: './InfrastructureModules/InfrastructureComputingPartner/Components/QueryColumnsComponents/btnComponentComputingPartnerEdit',
        });
    };
    ComputingPartnerTranslateComponent.prototype.onRowSelected = function ($event) {
        this.$event = $event;
    };
    ComputingPartnerTranslateComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        var _this = this;
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new CommonDomainService_1.CustomApiQueryFilters();
        }
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.EntityPM.ObjectTableName; })[0];
        if (table)
            filters.ClinetName = table.ClientModuleName;
        filters.GetAll = true;
        filters.GetCount = getCount;
        filters.PageIndex = skip;
        filters.PageSize = take;
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        filters.tenant2 = SessionLocator_1.SessionLocator.Tenant;
        filters.computingPartnerId = this.EntityPM.ComputingPartnerId;
        filters.objectTableId = this.EntityPM.ObjectTableId;
        filters.objectTableName = this.EntityPM.ObjectTableName;
        filters.ComputingPartnerName = this.EntityPM.ComputingPartnerName;
        filters.SearchingFields = this.searchFields;
        filters.IsClosedTable = table.IsClosed;
        var service = new CommonDomainService_1.CommonDomainService();
        return new Promise(function (resolve, reject) {
            resolve(service.getByFilters(filters));
        }).then(function (result) {
            return result;
        });
    };
    ComputingPartnerTranslateComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ComputingPartnerTranslateComponent.prototype, "SearchFieldchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ComputingPartnerTranslateComponent.prototype, "CustomBackFromEditevent", void 0);
    ComputingPartnerTranslateComponent = __decorate([
        core_1.Component({
            selector: 'ComputingPartnerTranslateComponent',
            moduleId: module.id,
            templateUrl: './ComputingPartnerTranslateComponent.html',
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService])
    ], ComputingPartnerTranslateComponent);
    return ComputingPartnerTranslateComponent;
}(BaseComponent_1.BaseComponent));
exports.ComputingPartnerTranslateComponent = ComputingPartnerTranslateComponent;
//# sourceMappingURL=ComputingPartnerTranslateComponent.js.map