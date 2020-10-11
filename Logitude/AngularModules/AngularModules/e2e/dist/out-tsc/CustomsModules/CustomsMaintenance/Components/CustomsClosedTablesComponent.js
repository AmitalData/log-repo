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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var SystemTableRequestParams_1 = require("../../../Customs/DataContract/RequestParams/SystemTableRequestParams");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var CustomMessageProgressComponent_1 = require("../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var CustomsClosedTablesComponent = /** @class */ (function () {
    function CustomsClosedTablesComponent() {
        var _this = this;
        this.DataContext = this;
        this.ObjectTableName = "Customs.CustomsClosedTable";
        this.columns = null;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._stratSearch = true;
        this.MenuHeaderchangeevent = new core_1.EventEmitter();
        this.onQueryChangeEvent = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._IsLoaded = false;
        this._Existed = true;
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
            //try {
            _this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(function (response) {
                //this._IsLoaded = true;
            });
            //} catch (err) {
            //    console.warn(err);
            //}
        });
    }
    CustomsClosedTablesComponent.prototype.ngOnInit = function () {
        //this._entityListService = new EntityListService();
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            //try {
            _this._entityResourceService.getEntityResourceByTableName("CommunicationLog").subscribe(function (response) {
                _this._IsLoaded = true;
            });
            //} catch (err) {
            //  console.warn(err);
            //}
            _this.BuildColumns();
            _this.RefreshBtnClick();
        });
    };
    CustomsClosedTablesComponent.prototype.BackButtonClicked = function () {
        if (this.ComponentRef) {
            this.ComponentRef.destroy();
        }
    };
    Object.defineProperty(CustomsClosedTablesComponent.prototype, "Existed", {
        get: function () { return this._Existed; },
        set: function (val) {
            if (this._Existed == val)
                return;
            this._Existed = val;
            this.RefreshBtnClick();
        },
        enumerable: true,
        configurable: true
    });
    CustomsClosedTablesComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
            FieldName: 'Id',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsClosedTable.O.Id"),
            Styles: { width: '75px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
            FieldName: 'CustomsName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.CustomsName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
            FieldName: 'CustomsLocalName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.CustomsLocalName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'LastUpdateDate',
            DataTypeCode: 'Date',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.LastUpdateDate"),
            Styles: { width: '145px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
        });
        this.columns.push({
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
            FieldName: 'StatusName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsClosedTable.F.StatusName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true
        });
        this.columns.push({
            FieldName: 'TableUpdateButton',
            DataTypeCode: 'String',
            Display: "",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
        });
        this.columns.push({
            FieldName: 'ShowDetailsButton',
            DataTypeCode: 'String',
            Display: "",
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsClosedTablesListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsClosedTablesListTemplate',
        });
    };
    CustomsClosedTablesComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        var _this = this;
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = "Id";
        filters.SortDirection = "Descending";
        filters.SortBy = "CustomsName"; //"Id";
        filters.SortDirection = "Descending"; //"Descending";
        if (this._Existed) {
            filters.addAdditionalFilter("Existed", this._Existed, null, null, "Equals", false, false, false, "boolean");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this._SearchText)) {
            filters.addAdditionalFilter("SearchFields", this._SearchText, null, null, "Contains", false, false, false, "string");
        }
        /// filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        var myout = this._entityListService
            .getExtendedByFilters("Customs.CustomsClosedTable", filters);
        myout.then(function (res) {
            _this._stratSearch = false;
            //this.CurrentSession.StopBusyIndicator();
        });
        return myout;
    };
    CustomsClosedTablesComponent.prototype.onSearchTextChangeEvent = function (text) {
        this._SearchText = text;
        this.RefreshBtnClick();
    };
    CustomsClosedTablesComponent.prototype.ItemClicked = function (item) {
    };
    CustomsClosedTablesComponent.prototype.RefreshBtnClick = function () {
        var _this = this;
        this._stratSearch = true;
        //this.CurrentSession.StartBusyIndicator("");
        setTimeout(function () {
            _this.MenuHeaderchangeevent.emit({ Filters: _this.filterAgrs, IgnoreFilter: false });
        }, 10);
    };
    CustomsClosedTablesComponent.prototype.UpdateAll = function () {
        var _this = this;
        var systemTableRequestParams = new SystemTableRequestParams_1.SystemTableRequestParams();
        //systemTableRequestParams.TableId = this._CustomsClosedTable.Id;
        systemTableRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        //systemTableRequestParamsystemTableRequestParams.s.RequestVIA == SendRequestVIA.WebServiceBatch;//all the time 
        systemTableRequestParams.UpdateAllTables = true;
        var myCustomMessageProgressHelper = new CustomMessageProgressComponent_1.CustomMessageProgressHelper();
        myCustomMessageProgressHelper.BasicResponse = true;
        myCustomMessageProgressHelper.StartProgress(systemTableRequestParams.PBId, 5, true);
        //CustomMessageProgressComponent
        //    .ShowProgressBar(systemTableRequestParams.PBId, "שליחת שאילתא להודעות בוקר", true)
        //    .then((res) => {
        //        //this.ResponseData = res;
        //        //this.OnMassageDisplayMethod();
        //    }
        //    ).catch((err) => {
        //        //this.ValidationErrorsList.push(err);
        //    });
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        myIIGGeneralMessagesService.PostUpdateClosedTables(systemTableRequestParams).subscribe(function (res) {
            myCustomMessageProgressHelper.MessageArrived = true;
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CustomsClosedTablesComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CustomsClosedTablesComponent.prototype, "onQueryChangeEvent", void 0);
    CustomsClosedTablesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsClosedTablesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomsClosedTablesComponent);
    return CustomsClosedTablesComponent;
}());
exports.CustomsClosedTablesComponent = CustomsClosedTablesComponent;
//# sourceMappingURL=CustomsClosedTablesComponent.js.map