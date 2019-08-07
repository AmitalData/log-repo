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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
//import { VehicleMessagesService } from '../../../Services/WebServices/VehicleMessagesService';
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var VehicleMoreDetailsTabComponent = /** @class */ (function (_super) {
    __extends(VehicleMoreDetailsTabComponent, _super);
    function VehicleMoreDetailsTabComponent(entityArgs, cd, EntityResourceService) {
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
        //#endregion
        _this.SendButtonsVisibility = false;
        //#endregion
        _this.line = 0;
        //#endregion
        _this.valid = true;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.EntityPM = _this.entityArgs.EntityPM;
                _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                _this.Listen();
            });
        });
        return _this;
    }
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    VehicleMoreDetailsTabComponent.prototype.Listen = function () {
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
    VehicleMoreDetailsTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        this.IsNewEntity = args.IsNewEntity;
        console.log("EntityPM", this.EntityPM);
        this.SetFieldsEditability();
    };
    VehicleMoreDetailsTabComponent.prototype.SetFieldsEditability = function () {
        //this.UIProperties.SetEnabled("VehicleTypeCode", this.ObjectTableName, this.IsNewEntity);
        //this.UIProperties.SetEnabled("SubCountryCode", this.ObjectTableName, !AppTool.IsNullOrEmpty(this.EntityPM.CountryCode));
    };
    //#endregion
    VehicleMoreDetailsTabComponent.prototype.checkForChassisNumberOp_Completed = function (exists) {
        if (exists) {
            this.UIProperties.SetValidity("InternalCode", "Customs.CustomBank", false, TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomBank.O.InternalCodekAlreadyExist"));
            //this.InvalidChassisNumber = true;
        }
        else {
            //this.InvalidChassisNumber = false;
        }
    };
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsABS", {
        /// #region Properties
        get: function () { return this.EntityPM != null ? this.EntityPM.IsABS : false; },
        set: function (value) { this.EntityPM.IsABS = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "AirBagsNumber", {
        //public int ? AirBagsNumber        {
        get: function () { return this.EntityPM != null ? this.EntityPM.AirBagsNumber : null; },
        set: function (value) { this.EntityPM.AirBagsNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "ConverterTypeCode", {
        //        public string ConverterTypeCode
        get: function () { return this.EntityPM != null ? this.EntityPM.ConverterTypeCode : null; },
        set: function (value) { this.EntityPM.ConverterTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "GreenIndex", {
        //public decimal ? GreenIndex
        get: function () { return this.EntityPM != null ? this.EntityPM.GreenIndex : null; },
        set: function (value) { this.EntityPM.GreenIndex = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "GreenIndexGroup", {
        //public int ? GreenIndexGroup
        get: function () { return this.EntityPM != null ? this.EntityPM.GreenIndexGroup : null; },
        set: function (value) { this.EntityPM.GreenIndexGroup = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsStabilityControl", {
        //public bool IsStabilityControl
        get: function () { return this.EntityPM != null ? this.EntityPM.IsStabilityControl : false; },
        set: function (value) { this.EntityPM.IsStabilityControl = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "VehicleTecnologyTypeCode", {
        //public string VehicleTecnologyTypeCode
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleTecnologyTypeCode : null; },
        set: function (value) { this.EntityPM.VehicleTecnologyTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "VehicleSafetyAccessoryPoints", {
        //public decimal ? VehicleSafetyAccessoryPoints
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleSafetyAccessoryPoints : null; },
        set: function (value) { this.EntityPM.VehicleSafetyAccessoryPoints = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "VehiclePriceListTypeCode", {
        //public string VehiclePriceListTypeCode
        get: function () { return this.EntityPM != null ? this.EntityPM.VehiclePriceListTypeCode : null; },
        set: function (value) { this.EntityPM.VehiclePriceListTypeCode = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsArmoredVehicle", {
        //        public bool IsArmoredVehicle
        get: function () { return this.EntityPM != null ? this.EntityPM.IsArmoredVehicle : false; },
        set: function (value) { this.EntityPM.IsArmoredVehicle = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsLoweringVehicleForInvalid", {
        ///public bool IsLoweringVehicleForInvalid
        get: function () { return this.EntityPM != null ? this.EntityPM.IsLoweringVehicleForInvalid : false; },
        set: function (value) { this.EntityPM.IsLoweringVehicleForInvalid = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "SelfVehicleWeight", {
        //public int ? SelfVehicleWeight
        get: function () { return this.EntityPM != null ? this.EntityPM.SelfVehicleWeight : null; },
        set: function (value) { this.EntityPM.SelfVehicleWeight = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "VehiclePowerKW", {
        //public int ? VehiclePowerKW
        get: function () { return this.EntityPM != null ? this.EntityPM.VehiclePowerKW : null; },
        set: function (value) {
            this.EntityPM.VehiclePowerKW = value;
            this.OnVehiclePowerKWLostFocus(value);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "MedalNumber", {
        //        public string MedalNumber
        get: function () { return this.EntityPM != null ? this.EntityPM.MedalNumber : null; },
        set: function (value) { this.EntityPM.MedalNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "DateOnRoadAbroad", {
        //        public DateTime ? DateOnRoadAbroad
        get: function () { return this.EntityPM != null ? this.EntityPM.DateOnRoadAbroad : null; },
        set: function (value) { this.EntityPM.DateOnRoadAbroad = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsraelEnterDate", {
        //public DateTime ? IsraelEnterDate
        get: function () { return this.EntityPM != null ? this.EntityPM.IsraelEnterDate : null; },
        set: function (value) { this.EntityPM.IsraelEnterDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "TransmissionDateWithoutTax", {
        //        public DateTime ? TransmissionDateWithoutTax
        get: function () { return this.EntityPM != null ? this.EntityPM.TransmissionDateWithoutTax : null; },
        set: function (value) { this.EntityPM.TransmissionDateWithoutTax = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "NumberOfSeats", {
        //    public int ? NumberOfSeats
        get: function () { return this.EntityPM != null ? this.EntityPM.NumberOfSeats : null; },
        set: function (value) { this.EntityPM.NumberOfSeats = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsThreeWheeledForReduction", {
        //    public bool IsThreeWheeledForReduction
        get: function () { return this.EntityPM != null ? this.EntityPM.IsThreeWheeledForReduction : false; },
        set: function (value) { this.EntityPM.IsThreeWheeledForReduction = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsCBS", {
        //    public bool IsCBS
        get: function () { return this.EntityPM != null ? this.EntityPM.IsCBS : false; },
        set: function (value) { this.EntityPM.IsCBS = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsSlipperClutch", {
        //    public bool IsSlipperClutch
        get: function () { return this.EntityPM != null ? this.EntityPM.IsSlipperClutch : false; },
        set: function (value) { this.EntityPM.IsSlipperClutch = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsSteeringDamper", {
        //    public bool IsSteeringDamper
        get: function () { return this.EntityPM != null ? this.EntityPM.IsSteeringDamper : false; },
        set: function (value) { this.EntityPM.IsSteeringDamper = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsTCS", {
        //    public bool IsTCS
        get: function () { return this.EntityPM != null ? this.EntityPM.IsTCS : false; },
        set: function (value) { this.EntityPM.IsTCS = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "IsTPS", {
        //    public bool IsTPS
        get: function () { return this.EntityPM != null ? this.EntityPM.IsTPS : false; },
        set: function (value) { this.EntityPM.IsTPS = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "VehicleCategory", {
        //    public string VehicleCategory
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleCategory : null; },
        set: function (value) { this.EntityPM.VehicleCategory = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehicleMoreDetailsTabComponent.prototype, "VehicleMaxPowerKW", {
        //    public decimal ? VehicleMaxPowerKW
        get: function () { return this.EntityPM != null ? this.EntityPM.VehicleMaxPowerKW : null; },
        set: function (value) { this.EntityPM.VehicleMaxPowerKW = value; },
        enumerable: true,
        configurable: true
    });
    //#region Send + Delete
    VehicleMoreDetailsTabComponent.prototype.SendButtonClicked = function () {
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
    VehicleMoreDetailsTabComponent.prototype.OnSendCompleted = function () {
        if (this.IsDelete) {
            this.ApplyDeleteVehicle();
        }
    };
    VehicleMoreDetailsTabComponent.prototype.ApplyDeleteVehicle = function () {
        this.IsDelete = false;
        //RefreshDataEvent refreshDataEvent = eventAggregator.GetEvent<RefreshDataEvent>();
        //refreshDataEvent.Publish(new RefreshDataEventArgs() { });
        this.CurrentSession.CloseCurrentWindow(); //currentAssemlyLocator.CurrentSimplogWindow.Close();
        //TenantContext.Current.RefreshTableData("Customs.Vehicle", DateTime.UtcNow, true);
        //this.Dispose();
    };
    VehicleMoreDetailsTabComponent.prototype.VehiclePowerKWKeyUp = function (event, VehiclePowerKWTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnVehiclePowerKWLostFocus(VehiclePowerKWTextBox);
        }
    };
    VehicleMoreDetailsTabComponent.prototype.OnVehiclePowerKWLostFocus = function (VehiclePowerKWTextBox) {
        var newValue = this.VehiclePowerKW;
        this.valid = true;
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
        }
        else {
            this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1)
                strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (Tools_1.AppTool.IsNullOrEmpty(strValue)) {
                this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
            }
            else if (strValue.length > 5) {
                this.valid = false;
                var str1 = strValue.substring(0, 5);
                var str2 = newValue.toString();
                var str3 = str2.replace(strValue, str1);
                newValue = +str3;
                this.VehiclePowerKW = newValue;
                this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", false, "הספק לא יכול להיות ארוך מחמישה תווים");
            }
            else {
                this.valid = true;
                this.UIProperties.SetValidity("VehiclePowerKW", "Customs.Vehicle", true, "");
            }
        }
        if (this.valid != true) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: "", LogTextBoxId: VehiclePowerKWTextBox.InputId });
        }
    };
    VehicleMoreDetailsTabComponent.prototype.VehicleMaxPowerKWKeyUp = function (event, VehicleMaxPowerKWTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnVehicleMaxPowerKWLostFocus(VehicleMaxPowerKWTextBox);
        }
    };
    VehicleMoreDetailsTabComponent.prototype.OnVehicleMaxPowerKWLostFocus = function (VehicleMaxPowerKWTextBox) {
        var newValue = this.VehicleMaxPowerKW;
        this.valid = true;
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
        }
        else {
            this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1)
                strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (Tools_1.AppTool.IsNullOrEmpty(strValue)) {
                this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
            }
            else if (strValue.length > 5) {
                this.valid = false;
                var str1 = strValue.substring(0, 5);
                var str2 = newValue.toString();
                var str3 = str2.replace(strValue, str1);
                newValue = +str3;
                this.VehiclePowerKW = newValue;
                this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", false, "הספק לא יכול להיות ארוך מחמישה תווים");
            }
            else {
                this.valid = true;
                this.UIProperties.SetValidity("VehicleMaxPowerKW", "Customs.Vehicle", true, "");
            }
        }
        if (this.valid != true) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: "", LogTextBoxId: VehicleMaxPowerKWTextBox.InputId });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], VehicleMoreDetailsTabComponent.prototype, "FillValidationErrorList", void 0);
    VehicleMoreDetailsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './VehicleMoreDetailsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], VehicleMoreDetailsTabComponent);
    return VehicleMoreDetailsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.VehicleMoreDetailsTabComponent = VehicleMoreDetailsTabComponent;
//# sourceMappingURL=VehicleMoreDetailsTabComponent.js.map