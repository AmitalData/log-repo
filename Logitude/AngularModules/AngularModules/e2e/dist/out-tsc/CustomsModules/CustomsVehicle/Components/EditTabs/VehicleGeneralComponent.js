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
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var VehicleExtendedPMService_1 = require("../../../../Customs/Services/ExtendedPMs/VehicleExtendedPMService");
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var VehicleGeneralComponent = /** @class */ (function (_super) {
    __extends(VehicleGeneralComponent, _super);
    function VehicleGeneralComponent(entityArgs, cd, EntityResourceService) {
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
        _this._VehicleExtendedPMService = new VehicleExtendedPMService_1.VehicleExtendedPMService();
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
                _this.SetFieldsEditability();
            });
        });
        return _this;
    }
    Object.defineProperty(VehicleGeneralComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    VehicleGeneralComponent.prototype.Listen = function () {
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
                    _this.RefreshEntity();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DEGC") {
                        _this.RefreshEntity();
                    }
                }
            }));
        }
    };
    //public SetTabArgs(args: any, ValidationErrorsList: any[]) {
    VehicleGeneralComponent.prototype.SetTabArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        console.log("EntityPM", this.EntityPM);
        this.SetFieldsEditability();
    };
    VehicleGeneralComponent.prototype.RefreshEntity = function () {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
        this.SetFieldsEditability();
    };
    VehicleGeneralComponent.prototype.SetFieldsEditability = function () {
        this.SetImporterIdentityIdFieldsEditability();
        this.SetImporterPassportNumberFieldsEditability();
    };
    VehicleGeneralComponent.prototype.SetImporterIdentityIdFieldsEditability = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ImporterIdentityId)) {
            this.ImporterPassportNumber = null;
            this.ImporterPassCountryCode = null;
            this.ImporterPassportTypeCode = null;
            this.PassportName = null;
            this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ImporterPassportTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PassportName", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ImporterIdentityId", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassportTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PassportName", this.ObjectTableName, true);
        }
    };
    VehicleGeneralComponent.prototype.SetImporterPassportNumberFieldsEditability = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ImporterPassportNumber)) {
            this.EntityPM.ImporterIdentityId = null;
            this.ImporterIdentityId = null;
            this.UIProperties.SetEnabled("ImporterIdentityId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ImporterPassportNumber", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassCountryCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ImporterPassportTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PassportName", this.ObjectTableName, true);
        }
        else {
            this.UIProperties.SetEnabled("ImporterIdentityId", this.ObjectTableName, true);
        }
    };
    //#endregion
    VehicleGeneralComponent.prototype.checkForChassisNumberOp_Completed = function (exists) {
        if (exists) {
            this.UIProperties.SetValidity("InternalCode", "Customs.CustomBank", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist"));
            this.InvalidChassisNumber = true;
        }
        else {
            this.InvalidChassisNumber = false;
        }
    };
    Object.defineProperty(VehicleGeneralComponent.prototype, "VehicleChassisNumber", {
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleChassisNumber : null; },
        set: function (value) {
            if (this.EntityPM.VehicleChassisNumber != value) {
                this.EntityPM.VehicleChassisNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "InvalidChassisNumber", {
        get: function () { return this.EntityPM != null ? this.EntityPM.InvalidChassisNumber : false; },
        set: function (value) { this.EntityPM.InvalidChassisNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "VehiclePoolTypeCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.VehiclePoolTypeCode : null; },
        set: function (value) { this.EntityPM.VehiclePoolTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "ModelCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.ModelCode : null; },
        set: function (value) { this.EntityPM.ModelCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "ModelDescription", {
        get: function () { return this.EntityPM != null ? this.EntityPM.ModelDescription : null; },
        set: function (value) { this.EntityPM.ModelDescription = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "NumberOfWheels", {
        get: function () { return this.EntityPM != null ? this.EntityPM.NumberOfWheels : null; },
        set: function (value) { this.EntityPM.NumberOfWheels = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "RichbitFileNumber", {
        get: function () { return this.EntityPM != null ? this.EntityPM.RichbitFileNumber : null; },
        set: function (value) { this.EntityPM.RichbitFileNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "EngineCapacity", {
        get: function () { return this.EntityPM != null ? this.EntityPM.EngineCapacity : null; },
        set: function (value) { this.EntityPM.EngineCapacity = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "CommercialNickname", {
        get: function () { return this.EntityPM != null ? this.EntityPM.CommercialNickname : null; },
        set: function (value) { this.EntityPM.CommercialNickname = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "VehicleManufacturerCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleManufacturerCode : null; },
        set: function (value) { this.EntityPM.VehicleManufacturerCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "VehicleManufactureDate", {
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleManufactureDate : null; },
        set: function (value) { this.EntityPM.VehicleManufactureDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "FuelTypeCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.FuelTypeCode : null; },
        set: function (value) { this.EntityPM.FuelTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "ManufactureCountryCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.ManufactureCountryCode : null; },
        set: function (value) { this.EntityPM.ManufactureCountryCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "TotalVehicleWeight", {
        get: function () { return this.EntityPM != null ? this.EntityPM.TotalVehicleWeight : null; },
        set: function (value) { this.EntityPM.TotalVehicleWeight = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "VehicleWindowNumber", {
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleWindowNumber : null; },
        set: function (value) { this.EntityPM.VehicleWindowNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "VehicleTypeCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleTypeCode : null; },
        set: function (value) { this.EntityPM.VehicleTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "ImporterPassportNumber", {
        get: function () { return this.EntityPM != null ? this.EntityPM.ImporterPassportNumber : null; },
        set: function (value) {
            this.EntityPM.ImporterPassportNumber = value;
            this.SetImporterPassportNumberFieldsEditability();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "ImporterPassCountryCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.ImporterPassCountryCode : null; },
        set: function (value) { this.EntityPM.ImporterPassCountryCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "ImporterPassportTypeCode", {
        get: function () { return this.EntityPM != null ? this.EntityPM.ImporterPassportTypeCode : null; },
        set: function (value) { this.EntityPM.ImporterPassportTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "ImporterIdentityId", {
        get: function () {
            if (this.EntityPM == null)
                return null;
            return this.EntityPM.ImporterIdentityId;
        },
        set: function (value) {
            this.EntityPM.ImporterIdentityId = value;
            this.SetImporterIdentityIdFieldsEditability();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleGeneralComponent.prototype, "PassportName", {
        get: function () { return this.EntityPM != null ? this.EntityPM.PassportName : null; },
        set: function (value) { this.EntityPM.PassportName = value; },
        enumerable: true,
        configurable: true
    });
    //#region Send + Delete
    VehicleGeneralComponent.prototype.SendButtonClicked = function () {
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
            this.ValidationErrorsList = errors;
            this.FillValidationErrorList.emit(errors);
        }
        else {
            // send request
            //this.SendRequest(false);
        }
    };
    VehicleGeneralComponent.prototype.OnSendCompleted = function () {
        if (this.IsDelete) {
            this.ApplyDeleteVehicle();
        }
    };
    VehicleGeneralComponent.prototype.ApplyDeleteVehicle = function () {
        this.IsDelete = false;
        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vehicle", DateTime.UtcNow, true);
        //this.Dispose();
    };
    VehicleGeneralComponent.prototype.OnVehicleChassisNumberLostFocus = function (vehicleChassisNumberTextBox) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.VehicleChassisNumber)) {
            this._VehicleExtendedPMService.CheckIfVehicleExistByChassisNumber(this.VehicleChassisNumber).subscribe(function (response) {
                if (response != null) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(response.Result)) {
                        if (_this.EntityPM.Id != response.Result) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Warning");
                            messageWindow.Width = 250;
                            messageWindow.Height = 150;
                            messageWindow.OkButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                            messageWindow.WindowClosed.subscribe(function ($event) { return _this.OnCheckIfVehicleExistWindowClosed($event, vehicleChassisNumberTextBox); });
                            messageWindow.Show("קיים כבר רכב עם אותו מספר שלדה");
                            return;
                        }
                    }
                }
            });
        }
    };
    VehicleGeneralComponent.prototype.OnCheckIfVehicleExistWindowClosed = function (arg, vehicleChassisNumberTextBox) {
        if (!Tools_1.AppTool.IsNullOrEmpty(vehicleChassisNumberTextBox)) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            console.log(vehicleChassisNumberTextBox.InputId);
            var element = document.getElementById(vehicleChassisNumberTextBox.InputId);
            if (element) {
                element.focus();
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], VehicleGeneralComponent.prototype, "FillValidationErrorList", void 0);
    VehicleGeneralComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VehicleGeneralComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], VehicleGeneralComponent);
    return VehicleGeneralComponent;
}(BaseComponent_1.BaseComponent));
exports.VehicleGeneralComponent = VehicleGeneralComponent;
//# sourceMappingURL=VehicleGeneralComponent.js.map