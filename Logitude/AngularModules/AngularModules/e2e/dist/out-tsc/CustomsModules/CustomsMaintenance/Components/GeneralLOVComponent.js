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
//C: \LW\Customs\AngularModules\AngularModules\Customs\Services\StandardPMs\DocumentTypeCustomsDataPMService.ts
var DocumentTypeCustomsDataPMService_1 = require("../../../Customs/Services/StandardPMs/DocumentTypeCustomsDataPMService");
var DocumentTypeCustomsDataListService_1 = require("../../../Customs/Services/StandardLists/DocumentTypeCustomsDataListService");
var GeneralLOVComponent = /** @class */ (function (_super) {
    __extends(GeneralLOVComponent, _super);
    function GeneralLOVComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = ""; //"Customs.DocumentTypeCustomsData";
        _this.columns = null;
        _this._DocumentTypeCustomsDataPMService = new DocumentTypeCustomsDataPMService_1.DocumentTypeCustomsDataPMService();
        _this._DocumentTypeCustomsDataListService = new DocumentTypeCustomsDataListService_1.DocumentTypeCustomsDataListService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        _this.EntityResource = false;
        return _this;
    }
    GeneralLOVComponent.prototype.ngOnInit = function () {
        ///this.CurrentSession.StartBusyIndicatorLoading();
    };
    GeneralLOVComponent.prototype.SetWindowArgs = function (arg) {
        var _this = this;
        this.ObjectTableName = this.LogitudeEntity = arg.LogitudeEntity;
        this.Code = this.LogitudeEntityNumber = arg.LogitudeEntityNumber;
        this.LOVText = arg.LOVText;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            _this.EntityResource = true;
            _this.Loaded = true;
            if (Tools_1.AppTool.IsNullOrEmpty(_this.Code)) {
                if (_this.EntityResource && _this.Loaded) {
                }
            }
        });
    };
    Object.defineProperty(GeneralLOVComponent.prototype, "Code", {
        get: function () { return this._Code; },
        set: function (value) { this._Code = value; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    GeneralLOVComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("ShowGeneralLOVReturnSelectedCancel");
    };
    GeneralLOVComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit(this.Code);
    };
    GeneralLOVComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GeneralLOVComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GeneralLOVComponent);
    return GeneralLOVComponent;
}(BaseComponent_1.BaseComponent));
exports.GeneralLOVComponent = GeneralLOVComponent;
//# sourceMappingURL=GeneralLOVComponent.js.map