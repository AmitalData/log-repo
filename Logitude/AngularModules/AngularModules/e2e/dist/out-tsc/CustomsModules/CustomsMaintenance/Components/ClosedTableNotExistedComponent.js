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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var SystemTableRequestParams_1 = require("../../../Customs/DataContract/RequestParams/SystemTableRequestParams");
var RequestParamsBase_1 = require("../../../Customs/DataContract/RequestParams/RequestParamsBase");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var ClosedTableNotExistedComponent = /** @class */ (function () {
    function ClosedTableNotExistedComponent() {
        this.DataContext = this;
        this.ObjectTableName = "Customs.CustomsClosedTable";
        this.columns = null;
        this._Rows = [];
        this._ObservableList = new ObservableCollection_1.ObservableCollection([]);
    }
    ClosedTableNotExistedComponent.prototype.ngOnInit = function () {
        //this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
        //this._entityListService = new EntityListService();
        var _this = this;
        //this.RefreshBtnClick()
        //});
        var systemTableRequestParams = new SystemTableRequestParams_1.SystemTableRequestParams();
        systemTableRequestParams.TableId = this._CustomsClosedTable.Id;
        systemTableRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        systemTableRequestParams.RequestVIA == RequestParamsBase_1.SendRequestVIA.WebServiceBatch; //all the time 
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        myIIGGeneralMessagesService.PostFillNotExistedClosedTables(systemTableRequestParams).subscribe(function (res) {
            _this._Rows = res.Result;
            //this._ObservableList.InsertCollection(res.Result);
            _this.rowCount = _this._Rows.length;
            _this.filterRows();
        });
    };
    ClosedTableNotExistedComponent.prototype.SetWindowArgs = function (customsClosedTable) {
        this._CustomsClosedTable = customsClosedTable;
    };
    ClosedTableNotExistedComponent.prototype.onSearchTextChangeEvent = function (text) {
        this._SearchText = text;
        this.filterRows();
    };
    ClosedTableNotExistedComponent.prototype.filterRows = function () {
        var _this = this;
        var itemsSource = this._Rows;
        if (Tools_1.AppTool.IsNullOrEmpty(this._SearchText)) {
            //this.ItemsSource = this._CustomsRequestMenuService.CustomsRequestMenuItems;
        }
        else {
            itemsSource = itemsSource.filter(function (f) { return !Tools_1.AppTool.IsNullOrEmpty(f.nameField); });
            itemsSource = itemsSource.filter(function (f) { return f.nameField.toUpperCase().includes(_this._SearchText.toUpperCase()) || f.idField.toUpperCase().includes(_this._SearchText.toUpperCase()); });
        }
        this._ObservableList.Clear();
        this._ObservableList.InsertCollection(itemsSource);
    };
    ClosedTableNotExistedComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ClosedTableNotExistedComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ClosedTableNotExistedComponent);
    return ClosedTableNotExistedComponent;
}());
exports.ClosedTableNotExistedComponent = ClosedTableNotExistedComponent;
var SYSTBL_NG_9001_MSG_SystemTablesResponseTableData = /** @class */ (function () {
    function SYSTBL_NG_9001_MSG_SystemTablesResponseTableData() {
    }
    return SYSTBL_NG_9001_MSG_SystemTablesResponseTableData;
}());
//# sourceMappingURL=ClosedTableNotExistedComponent.js.map