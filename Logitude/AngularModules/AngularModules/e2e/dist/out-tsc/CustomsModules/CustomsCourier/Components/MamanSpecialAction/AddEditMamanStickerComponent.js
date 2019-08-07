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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DeclarationMamanSpecialActionPM_1 = require("../../../../Customs/EntityPMs/DeclarationMamanSpecialActionPM");
var DeclarationMamanSpecialActionPMService_1 = require("../../../../Customs/Services/StandardPMs/DeclarationMamanSpecialActionPMService");
var DeclarationWebService_1 = require("../../../../Customs/Services/WebServices/DeclarationWebService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var AddEditMamanStickerComponent = /** @class */ (function (_super) {
    __extends(AddEditMamanStickerComponent, _super);
    function AddEditMamanStickerComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationMamanSpecialAction";
        _this.isWindowMode = true;
        _this.ValidationErrorsList = [];
        _this.IsLoaded = false;
        _this._EntityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._DeclarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this._DeclarationMamanSpecialActionPMService = new DeclarationMamanSpecialActionPMService_1.DeclarationMamanSpecialActionPMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrentSession.StartBusyIndicator("");
        _this._EntityResourceService.getEntityResourceByTableName(_this.ObjectTableName).subscribe(function (response) {
            _this.CurrentSession.StopBusyIndicator();
            _this.IsLoaded = true;
        });
        return _this;
    }
    AddEditMamanStickerComponent.prototype.SetWindowArgs = function (entityArgs) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entityArgs)) {
            this.isWindowMode = true;
            if (Tools_1.AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
                this.EntityPM = new DeclarationMamanSpecialActionPM_1.DeclarationMamanSpecialActionPM();
                this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                this.EntityPM.DeclarationId = entityArgs.DeclarationId;
                this.EntityPM.MamanSpecialActionCode = "4";
            }
            else {
                this.EntityPM = entityArgs.EntityPM;
            }
        }
    };
    Object.defineProperty(AddEditMamanStickerComponent.prototype, "MamanLabelText1", {
        //#region Properties
        get: function () { return this.EntityPM.MamanLabelText1; },
        set: function (newValue) {
            this.EntityPM.MamanLabelText1 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMamanStickerComponent.prototype, "MamanLabelText2", {
        get: function () { return this.EntityPM.MamanLabelText2; },
        set: function (newValue) {
            this.EntityPM.MamanLabelText2 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMamanStickerComponent.prototype, "MamanLabelText3", {
        get: function () { return this.EntityPM.MamanLabelText3; },
        set: function (newValue) {
            this.EntityPM.MamanLabelText3 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMamanStickerComponent.prototype, "MamanLabelText4", {
        get: function () { return this.EntityPM.MamanLabelText4; },
        set: function (newValue) {
            this.EntityPM.MamanLabelText4 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditMamanStickerComponent.prototype, "MamanLabelText5", {
        get: function () { return this.EntityPM.MamanLabelText5; },
        set: function (newValue) {
            this.EntityPM.MamanLabelText5 = newValue;
        },
        enumerable: true,
        configurable: true
    });
    //#endregion\
    AddEditMamanStickerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        this._DeclarationMamanSpecialActionPMService.insert(this.EntityPM).subscribe(function (res) {
            _this._DeclarationWebService.GetDeclarationMamanSpecialAction(_this.EntityPM.DeclarationId, _this.EntityPM.Tenant, "U", "4").subscribe(function (myResult) {
                if (myResult.HasError) {
                    _this.ValidationErrorsList = [];
                    _this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                    return;
                }
                else {
                    _this.CurrentSession.StopBusyIndicator();
                    var myMessageWindow = new MessageWindow_1.MessageWindow();
                    myMessageWindow.Show(myResult.Result);
                }
                _this.CancelButtonClicked();
            });
        });
    };
    AddEditMamanStickerComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditMamanStickerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditMamanStickerComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AddEditMamanStickerComponent);
    return AddEditMamanStickerComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditMamanStickerComponent = AddEditMamanStickerComponent;
//# sourceMappingURL=AddEditMamanStickerComponent.js.map