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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var DocumentTypeListService_1 = require("../../../../Common/Services/StandardLists/DocumentTypeListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ShipmentDocsOutTabComponent = /** @class */ (function () {
    function ShipmentDocsOutTabComponent(entityArgs, _documentTypeListService, _elementRef) {
        this.entityArgs = entityArgs;
        this._documentTypeListService = _documentTypeListService;
        this._elementRef = _elementRef;
        this.InitializeDocsOutForAnotherObjectTable = new core_1.EventEmitter();
    }
    ShipmentDocsOutTabComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
            if (table)
                this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.ShipmentlevelCode = this.EntityPM.ShipmentLevelCode;
            var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
            apiQueryFilters.GetAll = true;
            apiQueryFilters.Tenant = this.EntityPM.Tenant;
            this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    var childrenDocTypes_invoices = myResult.filter(function (d) { return d.Code == "999S" || d.Code == "999M" || d.Code == "999CI"; });
                    if (childrenDocTypes_invoices.length > 0) {
                        childrenDocTypes_invoices.forEach(function (typeList) {
                            _this.ChildrenObjectTableIds = _this.ChildrenObjectTableIds + "," + typeList.ObjectTableId;
                        });
                        var x = _this.ChildrenObjectTableIds.charAt(0);
                        if (x == ',') {
                            _this.ChildrenObjectTableIds = _this.ChildrenObjectTableIds.substr(1);
                        }
                    }
                }
            });
        }
    };
    ShipmentDocsOutTabComponent.prototype.LoadFirstObjectTablesDocsCompleted = function () {
        if (this.EntityPM.ShipmentLevelCode == "C") {
            var masterObject = window.ObjectTables.filter(function (d) { return d.Name == "Master"; })[0];
            if (masterObject) {
                this.InitializeDocsOutForAnotherObjectTable.emit(masterObject.Id);
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ShipmentDocsOutTabComponent.prototype, "InitializeDocsOutForAnotherObjectTable", void 0);
    ShipmentDocsOutTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmentDocsOutTabComponent.html',
            providers: [DocumentTypeListService_1.DocumentTypeListService]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, DocumentTypeListService_1.DocumentTypeListService,
            core_1.ElementRef])
    ], ShipmentDocsOutTabComponent);
    return ShipmentDocsOutTabComponent;
}());
exports.ShipmentDocsOutTabComponent = ShipmentDocsOutTabComponent;
//# sourceMappingURL=ShipmentDocsOutTabComponent.js.map