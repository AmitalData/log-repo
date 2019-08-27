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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var CustomsAirlinePM_1 = require("../../../Customs/EntityPMs/CustomsAirlinePM");
var CustomsAirlinePMService_1 = require("../../../Customs/Services/StandardPMs/CustomsAirlinePMService");
var CustomsAirlineExtendedPMService_1 = require("../../../Customs/Services/ExtendedPMs/CustomsAirlineExtendedPMService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CustomsAirlineListService_1 = require("../../../Customs/Services/StandardLists/CustomsAirlineListService");
var AddEditCustomsAirlineComponent = /** @class */ (function (_super) {
    __extends(AddEditCustomsAirlineComponent, _super);
    function AddEditCustomsAirlineComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CustomsAirline";
        _this.isWindowMode = false;
        _this.ValidationErrorsList = [];
        _this._CustomsAirlinePMService = new CustomsAirlinePMService_1.CustomsAirlinePMService();
        _this._CustomsAirlineListService = new CustomsAirlineListService_1.CustomsAirlineListService();
        _this._CustomsAirlineExtendedPMService = new CustomsAirlineExtendedPMService_1.CustomsAirlineExtendedPMService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (Tools_1.AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            _this.EntityPM = new CustomsAirlinePM_1.CustomsAirlinePM();
            _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            _this.isWindowMode = true;
        }
        else {
            _this.EntityPM = _this.entityArgs.EntityPM;
        }
        return _this;
    }
    AddEditCustomsAirlineComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customs.CustomsAirline").subscribe(function (response) {
            });
            //this.RefreshBtnClick()
        });
    };
    AddEditCustomsAirlineComponent.prototype.SetWindowArgs = function (WinArg) {
        var _this = this;
        ;
        if (!Tools_1.AppTool.IsNullOrEmpty(WinArg)) {
            this.isWindowMode = true;
        }
        this._CustomsAirlineList = WinArg.SelectedItem;
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customs.CustomsAirline").subscribe(function (response) {
                _this._CustomsAirlineExtendedPMService
                    .GetSingleCustomsAirlineByCodeAndPrefix(_this._CustomsAirlineList.AirlineCode, _this._CustomsAirlineList.AirlinePrefix)
                    .subscribe(function (rsp) {
                    _this.EntityPM = rsp.Result;
                    _this.CurrentSession.StopBusyIndicator();
                });
            });
        });
    };
    Object.defineProperty(AddEditCustomsAirlineComponent.prototype, "AirlinePrefix", {
        //#region Properties
        get: function () { return this.EntityPM.AirlinePrefix; },
        set: function (newValue) {
            this.EntityPM.AirlinePrefix = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsAirlineComponent.prototype, "AirlineCode", {
        get: function () { return this.EntityPM.AirlineCode; },
        set: function (newValue) {
            this.EntityPM.AirlineCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsAirlineComponent.prototype, "ICAO", {
        get: function () { return this.EntityPM.ICAO; },
        set: function (newValue) {
            this.EntityPM.ICAO = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsAirlineComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            this.EntityPM.EnglishName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsAirlineComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            this.EntityPM.LocalName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditCustomsAirlineComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            this.EntityPM.InActive = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion\
    AddEditCustomsAirlineComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        }
        else {
            this._CustomsAirlinePMService.insert(this.EntityPM).subscribe(function (myResult) {
                var mm = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    };
    AddEditCustomsAirlineComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditCustomsAirlineComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditCustomsAirlineComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AddEditCustomsAirlineComponent);
    return AddEditCustomsAirlineComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditCustomsAirlineComponent = AddEditCustomsAirlineComponent;
//# sourceMappingURL=AddEditCustomsAirlineComponent.js.map