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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';
var VehiclesSelectionComponent = /** @class */ (function (_super) {
    __extends(VehiclesSelectionComponent, _super);
    function VehiclesSelectionComponent(entityArgs, cd, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.EntityResourceService = EntityResourceService;
        _this.FillValidationErrorList = new core_1.EventEmitter();
        _this.ObjectTableName = "Customs.Vehicle";
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.SubCountryCodeEnabled = false;
        _this.IsDelete = false;
        //VehicleMessagesService: VehicleMessagesService = new VehicleMessagesService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        ///#region Properties
        _this.SendButtonsVisibility = false;
        //#endregion
        _this.line = 0;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.EntityPM = _this.entityArgs.EntityPM;
                _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                _this.Listen();
            });
        });
        return _this;
    }
    Object.defineProperty(VehiclesSelectionComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    VehiclesSelectionComponent.prototype.Listen = function () {
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
                    //this.DisplayOnlyCheck();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DEGC") {
                        //this.DisplayOnlyCheck();
                    }
                }
            }));
        }
    };
    VehiclesSelectionComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        console.log("EntityPM", this.EntityPM);
        this.SetFieldsEditability();
    };
    VehiclesSelectionComponent.prototype.SetFieldsEditability = function () {
        //this.UIProperties.SetEnabled("VehicleTypeCode", this.ObjectTableName, this.IsNewEntity);
        //this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityPM.CountryCode));
    };
    //#endregion
    VehiclesSelectionComponent.prototype.checkForChassisNumberOp_Completed = function (exists) {
        if (exists) {
            this.UIProperties.SetValidity("InternalCode", "Customs.CustomBank", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist"));
            //this.InvalidChassisNumber = true;
        }
        else {
            //this.InvalidChassisNumber = false;
        }
    };
    //#region Send + Delete
    VehiclesSelectionComponent.prototype.SendButtonClicked = function () {
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs
        // validate Vehicle
        Validator_1.Validator.TryValidateObject(this.EntityPM, "Customs.Vehicle", errors);
        //if (this.EntityPM.VehicleCommunications.length == 0) {
        //    errors.push(TextCodeTranslator.Translate("Customs.Vehicle.O.RequierdCommunication"));
        //} else {
        //    // validate Vehicle communication items
        //    this.EntityPM.VehicleCommunications.forEach((item) => {
        //        Validator.TryValidateObject(item, "Customs.VehicleCommunication", errors);
        //    });
        //}
        if (errors.length > 0) {
            this.ValdationErrorList = errors;
            this.FillValidationErrorList.emit(errors);
        }
        else {
            // send request
            //this.SendRequest(false);
        }
    };
    VehiclesSelectionComponent.prototype.OnSendCompleted = function () {
        if (this.IsDelete) {
            this.ApplyDeleteVehicle();
        }
    };
    VehiclesSelectionComponent.prototype.ApplyDeleteVehicle = function () {
        this.IsDelete = false;
        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vehicle", DateTime.UtcNow, true);
        //this.Dispose();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], VehiclesSelectionComponent.prototype, "FillValidationErrorList", void 0);
    VehiclesSelectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VehiclesSelectionComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], VehiclesSelectionComponent);
    return VehiclesSelectionComponent;
}(BaseComponent_1.BaseComponent));
exports.VehiclesSelectionComponent = VehiclesSelectionComponent;
//# sourceMappingURL=VehiclesSelectionComponent.js.map