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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
;
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var CustomsCollateralPMService_1 = require("../../../../../Customs/Services/StandardPMs/CustomsCollateralPMService");
var DeclarationCollateralsComponent = /** @class */ (function (_super) {
    __extends(DeclarationCollateralsComponent, _super);
    function DeclarationCollateralsComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DataContext = _this;
        _this._DeclarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this._CustomsCollateralPMService = new CustomsCollateralPMService_1.CustomsCollateralPMService;
        _this.IsLoaded = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.collateralObslist = new ObservableCollection_1.ObservableCollection([]);
        _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe(function (response) {
                    _this.EntityPM = _this.entityArgs.EntityPM;
                    _this.LoadDeclarationCollateralsList();
                    _this.Listen();
                    _this.IsLoaded = true;
                });
            });
        });
        return _this;
    }
    DeclarationCollateralsComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
    };
    DeclarationCollateralsComponent.prototype.ngOnDestroy = function () {
        console.log("DeclarationCollateralsComponent:ngOnDestroy");
        this.entityArgs = null;
    };
    DeclarationCollateralsComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.LoadDeclarationCollateralsList();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCCL") {
                        _this.LoadDeclarationCollateralsList();
                    }
                }
            }));
        }
    };
    DeclarationCollateralsComponent.prototype.LoadDeclarationCollateralsList = function () {
        var _this = this;
        this.collateralObslist = new ObservableCollection_1.ObservableCollection([]);
        this._DeclarationWebService.GetDeclarationCollateralsList(this.EntityPM.Id, this.EntityPM.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.GetDeclarationCollateralsListsOp_Completed(myResponse, false);
        });
    };
    DeclarationCollateralsComponent.prototype.GetDeclarationCollateralsListsOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        if (myResponse.Result != null) {
            myResponse.Result.forEach(function (item) {
                _this.collateralObslist.Insert(item);
            });
        }
    };
    DeclarationCollateralsComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    DeclarationCollateralsComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this._CustomsCollateralPMService.get(item.Id).subscribe(function (response) {
                var windowArgs = {};
                windowArgs.CurrentEntity = response.Result;
                windowArgs.declarationPM = _this.EntityPM;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Width = 600;
                logWindow.Height = 700;
                logWindow.ShowCloseButton = false;
                logWindow.WindowArgs = windowArgs;
                logWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    DeclarationCollateralsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationCollateralsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], DeclarationCollateralsComponent);
    return DeclarationCollateralsComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationCollateralsComponent = DeclarationCollateralsComponent;
//# sourceMappingURL=DeclarationCollateralsComponent.js.map