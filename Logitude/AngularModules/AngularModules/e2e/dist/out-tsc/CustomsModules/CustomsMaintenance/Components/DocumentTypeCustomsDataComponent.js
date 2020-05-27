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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DocumentTypeCustomsDataPM_1 = require("../../../Customs/EntityPMs/DocumentTypeCustomsDataPM");
//C: \LW\Customs\AngularModules\AngularModules\Customs\Services\StandardPMs\DocumentTypeCustomsDataPMService.ts
var DocumentTypeCustomsDataPMService_1 = require("../../../Customs/Services/StandardPMs/DocumentTypeCustomsDataPMService");
var DocumentTypeCustomsDataListService_1 = require("../../../Customs/Services/StandardLists/DocumentTypeCustomsDataListService");
var DocumentTypeCustomsDataExtendPMService_1 = require("../../../Customs/Services/ExtendedPMs/DocumentTypeCustomsDataExtendPMService");
var DocumentTypeCustomsDataComponent = /** @class */ (function (_super) {
    __extends(DocumentTypeCustomsDataComponent, _super);
    function DocumentTypeCustomsDataComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DocumentTypeCustomsData";
        _this.columns = null;
        _this._DocumentTypeCustomsDataPMService = new DocumentTypeCustomsDataPMService_1.DocumentTypeCustomsDataPMService();
        _this._DocumentTypeCustomsDataListService = new DocumentTypeCustomsDataListService_1.DocumentTypeCustomsDataListService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        _this.EntityResource = false;
        _this.IsNew = false; //itzik : there is a row that come with defualt DB !!!
        return _this;
    }
    DocumentTypeCustomsDataComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this.EntityResource = true;
            if (_this.EntityResource && _this.Loaded) {
                _this.CurrentSession.StopBusyIndicator();
            }
            //this.RefreshBtnClick()
        });
    };
    DocumentTypeCustomsDataComponent.prototype.SetWindowArgs = function (arg) {
        var _this = this;
        this.DocumentTypeId = arg.UnifaceDOC_ID;
        this.UnifaceNAME_HEB = arg.UnifaceNAME_HEB;
        this._DocumentTypeCustomsDataPMService
            .get(this.DocumentTypeId)
            .subscribe(function (res) {
            _this.entityPM = res.Result;
            if (_this.entityPM == null) {
                _this.IsNew = true;
                _this.entityPM = new DocumentTypeCustomsDataPM_1.DocumentTypeCustomsDataPM();
                _this.entityPM.DocumentTypeId = _this.DocumentTypeId;
                _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            }
            _this.CurrentSession.StopBusyIndicator();
            _this.Loaded = true;
            if (_this.EntityResource && _this.Loaded) {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    Object.defineProperty(DocumentTypeCustomsDataComponent.prototype, "CustomsDoucumentTypeCode", {
        ///#region Properties
        //IsUnifreightCertificateActivatedEnabled: boolean = true;
        get: function () { return this.entityPM.CustomsDoucumentTypeCode; },
        set: function (value) { this.entityPM.CustomsDoucumentTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    DocumentTypeCustomsDataComponent.prototype.DeleteRow = function () {
        var _this = this;
        var documentTypeCustomsDataExtendPMService = new DocumentTypeCustomsDataExtendPMService_1.DocumentTypeCustomsDataExtendPMService();
        documentTypeCustomsDataExtendPMService.DeleteRecord(this.entityPM.DocumentTypeId)
            .subscribe(function (resp) {
            if (resp.HasError) {
                _this.ValidationErrorsList = [];
                _this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                return;
            }
            _this.CancelButtonClicked();
        });
    };
    DocumentTypeCustomsDataComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DocumentTypeCustomsDataComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.IsNew) {
            this._DocumentTypeCustomsDataPMService.insert(this.entityPM)
                .subscribe(function (resp) {
                if (resp.HasError) {
                    _this.ValidationErrorsList = [];
                    _this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                    return;
                }
                _this.CancelButtonClicked();
            });
        }
        else {
            this._DocumentTypeCustomsDataPMService.update(this.entityPM)
                .subscribe(function (resp) {
                if (resp.HasError) {
                    _this.ValidationErrorsList = [];
                    _this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                    return;
                }
                _this.CancelButtonClicked();
            });
        }
    };
    DocumentTypeCustomsDataComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DocumentTypeCustomsDataComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DocumentTypeCustomsDataComponent);
    return DocumentTypeCustomsDataComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentTypeCustomsDataComponent = DocumentTypeCustomsDataComponent;
//# sourceMappingURL=DocumentTypeCustomsDataComponent.js.map