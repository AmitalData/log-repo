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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var SignStationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/SignStationExtendedListService");
var SignStationsComponent = /** @class */ (function (_super) {
    __extends(SignStationsComponent, _super);
    function SignStationsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CustomsSetting";
        _this.columns = null;
        _this._TranslationLoaded = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        //entityPM: SignStationPM;
        _this._SignStationExtendedListService = new SignStationExtendedListService_1.SignStationExtendedListService();
        _this.ValidationErrorsList = [];
        _this._SelectedStatusValue = '';
        _this._StatusList = ["Start", "תקין", "כשלון", "ממתין להזנת סיסמא", "כרטיס שגוי", "ללא הגדרה"];
        _this._AllNum = 0;
        _this._PinCodeNum = 0;
        _this._BadCardSelectedNum = 0;
        _this._OKNum = 0;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        ///#region Properties
        //IsUnifreightCertificateActivatedEnabled: boolean = true;
        //get UnifreightCertificateActivated() { return this.entityPM != null ? this.entityPM.UnifreightCertificateActivated : false; }
        //set UnifreightCertificateActivated(value: boolean) { this.entityPM.UnifreightCertificateActivated = value; }
        _this.IsSearchButtonEnabled = true;
        //#endregion
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.SignStationGroupList = [];
        _this._SearchText = "";
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            //SortData("RequestCreateDate", "Descending", false, false);
            sortingCol: "SignerName",
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this._entityListService = new EntityListService_1.EntityListService();
        _this.BuildColumns();
        return _this;
    }
    SignStationsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._TranslationLoaded = true;
            console.log("SignStationsC1111ompon");
            console.log("testX12333  dfgdsfg 55");
            ///console.log("te1tsssss");
            console.log("te111t");
            _this.RefreshBtnClick();
        });
    };
    SignStationsComponent.prototype.RefreshBtnClick = function () {
        var _this = this;
        this.IsSearchButtonEnabled = false;
        //this.CurrentSession.StartBusyIndicator("");
        setTimeout(function () {
            _this._SignStationExtendedListService
                .GetSignStationGroupByStatus(_this._SearchText)
                .subscribe(function (response) {
                _this.SignStationGroupList = response;
                //this.RebuildTotals();
                _this._All = _this.GetTotalOf("");
                _this._Waitingtoenterapassword = _this.GetTotalOf("Waitingtoenterapassword");
                _this._Incorrectcard = _this.GetTotalOf("IncorrectCard");
                _this._OK = _this.GetTotalOf("OK");
            });
        }, 1);
        setTimeout(function () {
            _this.MenuHeaderchangeevent.emit({ Filters: _this.filterAgrs, IgnoreFilter: false });
        }, 10);
        //this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); 
    };
    SignStationsComponent.prototype.GetTotalOf = function (mystatus) {
        //_StatusList = ["Start", "תקין", "כישלון", "ממתין להזנת סיסמא", "כרטיס שגוי", "ללא הגדרה"];
        //_StatusList = ["Start", "OK", "Failure", "Waitingtoenterapassword", "Incorrectcard", "NoDefinition"];
        var tot = 0;
        this.SignStationGroupList.forEach(function (r) {
            if (Tools_1.AppTool.IsNullOrEmpty(mystatus)) {
                tot = tot + r.Total;
            }
            else {
                if (r.Key.toString().localeCompare(mystatus.toString()) == 0) {
                    //return " (" + r.Total.toString() + ") ";
                    tot = r.Total;
                }
                if (r.Key.toString() == mystatus.toString()) {
                    //return " (" + r.Total.toString() + ") ";
                    tot = r.Total;
                }
            }
        });
        return " (" + tot + ") ";
    };
    SignStationsComponent.prototype.TextChanged = function ($event) {
        this._SearchText = $event;
        this.RefreshBtnClick();
    };
    SignStationsComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'SignerName',
            DataTypeCode: 'string',
            Display: 'שם החותם',
            Styles: { width: '130px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'SignerName'
        });
        this.columns.push({
            FieldName: 'PersonId',
            DataTypeCode: 'String',
            Display: 'מספר ת.ז',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'PersonId'
        });
        this.columns.push({
            FieldName: 'CustomsAgentId',
            DataTypeCode: 'string',
            Display: 'מספר ח.פ',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CustomsAgentId'
        });
        this.columns.push({
            FieldName: 'MachineName',
            DataTypeCode: 'string',
            Display: 'תחנה',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'MachineName'
        });
        this.columns.push({
            FieldName: 'MachineUser',
            DataTypeCode: 'string',
            Display: 'משתמש (תחנה)',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'MachineUser'
        });
        this.columns.push({
            FieldName: 'IsPersonalSignOn',
            DataTypeCode: 'string',
            Display: 'ח. אישית',
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'IsPersonalSignOn',
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './Customs/Components/ListTemplates/SignStationListTemplate',
        });
        this.columns.push({
            FieldName: 'IsCompanySignOn',
            DataTypeCode: 'string',
            Display: 'ח. חברתית',
            Styles: { width: '65px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'IsCompanySignOn',
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './Customs/Components/ListTemplates/SignStationListTemplate',
        });
        this.columns.push({
            FieldName: 'VersionByFeatures',
            DataTypeCode: 'string',
            Display: 'גרסה',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'VersionByFeatures',
        });
        this.columns.push({
            FieldName: 'LastSignAt',
            DataTypeCode: 'string',
            Display: 'בוצעה חתימה',
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'LastSignAt',
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './Customs/Components/ListTemplates/SignStationListTemplate',
        });
        this.columns.push({
            FieldName: 'Status',
            DataTypeCode: 'string',
            Display: 'סטטוס',
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'Status',
            HtmlListComponentName: 'SignStationListTemplate',
            HtmlListComponentUrl: './Customs/Components/ListTemplates/SignStationListTemplate',
        });
        //this.columns.push({
        //    FieldName: 'TestSign',
        //    DataTypeCode: 'String',
        //    Display: '',//TextCodeTranslator.Translate("CommunicationLogSteps.O.Log"),
        //    Styles: { width: '100px' },
        //    IsCustomTemplate: true,
        //    HtmlListComponentName: 'SignStationListTemplate',
        //    HtmlListComponentUrl: './Customs/Components/ListTemplates/SignStationListTemplate',
        //});
    };
    SignStationsComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol; //"RequestCreateDate";
        filters.SortDirection = sortingDir; //"Descending";
        searchfields = this._SearchText || "";
        var myout = this.getExtendedByFilters(skip, take, sortingCol, sortingDir, getCount, searchfields);
        return myout;
    };
    SignStationsComponent.prototype.getExtendedByFilters //(objectTableName: string, filters: ApiQueryFilters, MethodName: string = null) {
     = function (skip, take, sortingCol, sortingDir, getCount, searchfields) {
        var _this = this;
        var servicelink = './Customs/Services/ExtendedLists/SignStationExtendedListService';
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                resolve(service.getByFilters(skip, take, sortingCol, sortingDir, getCount, searchfields, _this._SelectedStatusValue));
            });
        });
    };
    SignStationsComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SignStationsComponent.prototype.OkButtonClicked = function () {
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SignStationsComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SignStationsComponent.prototype, "onQueryChangeEvent", void 0);
    SignStationsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SignStationsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SignStationsComponent);
    return SignStationsComponent;
}(BaseComponent_1.BaseComponent));
exports.SignStationsComponent = SignStationsComponent;
//# sourceMappingURL=SignStationsComponent.js.map