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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var AirlineListService_1 = require("../../../../Common/Services/StandardLists/AirlineListService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FlightsSchedulesRequestPM_1 = require("../../../../Booking/EntityPMs/FlightsSchedulesRequestPM");
var MessageSimulatingService_1 = require("../../../../Infrastructure/Services/WebServices/MessageSimulatingService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var FVASimulatorComponent = /** @class */ (function (_super) {
    __extends(FVASimulatorComponent, _super);
    function FVASimulatorComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "FlightsSchedulesRequest";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationText = null;
        _this.mySimulatingService = new MessageSimulatingService_1.MessageSimulatingService();
        return _this;
    }
    FVASimulatorComponent.prototype.SetWindowArgs = function (args) {
        this.EntityId = args.EntityPM.Id;
        this.Recipient = args.Recipient;
        this.Airline = args.AirlineList;
        var myEntity = args.EntityPM;
        this.EntityPM = new FlightsSchedulesRequestPM_1.FlightsSchedulesRequestPM();
        this.EntityPM.Tenant = myEntity.Tenant;
        this.EntityPM.CreateDate = myEntity.CreateDate;
        this.EntityPM.CreatedByUserId = myEntity.CreatedByUserId;
        this.EntityPM.AirlineId = myEntity.AirlineId;
        this.EntityPM.BookingId = myEntity.BookingId;
        this.EntityPM.ShipmentId = myEntity.ShipmentId;
        this.EntityPM.AnswerOSI = myEntity.AnswerOSI;
        this.EntityPM.AnswerReasonForNoReply = myEntity.AnswerReasonForNoReply;
        this.EntityPM.ETA = myEntity.ETA;
        this.EntityPM.ETD = myEntity.ETD;
        this.EntityPM.FromPortId = myEntity.FromPortId;
        this.EntityPM.ToPortId = myEntity.ToPortId;
        this.EntityPM.GrossWeight = myEntity.GrossWeight;
        this.EntityPM.GrossWeightUnitCode = myEntity.GrossWeightUnitCode;
        this.EntityPM.Volume = myEntity.Volume;
        this.EntityPM.VolumeUnitCode = myEntity.VolumeUnitCode;
        this.EntityPM.Tenant = myEntity.Tenant;
        this.EntityPM.RequestDetails = myEntity.RequestDetails;
        this.EntityPM.ResponseDate = myEntity.ResponseDate;
        this.EntityPM.StatusCode = myEntity.StatusCode;
        this.SetUIProperties();
    };
    FVASimulatorComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("AirlineId", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("FromPortId", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ToPortId", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ETD", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ETA", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("FromDate", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("ToDate", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("AnswerOSI", this.ObjectTableName, !this.isReasonForNoReplayChecked);
        this.UIProperties.SetEnabled("AnswerReasonForNoReply", this.ObjectTableName, this.isReasonForNoReplayChecked);
        if (this.isReasonForNoReplayChecked) {
            this.UIProperties.SetRequired("AnswerReasonForNoReply", this.ObjectTableName, (Tools_1.AppTool.IsNullOrEmpty(this.AnswerReasonForNoReply) ? true : false));
        }
        else {
            this.UIProperties.SetRequired("ETD", this.ObjectTableName, (this.ETD == null ? true : false));
            var isWeightRequired = false;
            if (this.GrossWeight > 0) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
                    isWeightRequired = true;
                }
            }
            var isVolumeRequired = false;
            if (this.Volume > 0) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
                    isVolumeRequired = true;
                }
            }
            this.UIProperties.SetRequired("Volume", this.ObjectTableName, isVolumeRequired);
            this.UIProperties.SetRequired("GrossWeight", this.ObjectTableName, isWeightRequired);
        }
        this.SetUIProperties_Airline();
    };
    FVASimulatorComponent.prototype.SetUIProperties_Airline = function () {
        var isFieldEnabled = true;
        if (this.isReasonForNoReplayChecked) {
            isFieldEnabled = false;
        }
        else {
            if (this.Airline != null) {
                if (this.Airline.NoAvailabilityInFVAMessages) {
                    isFieldEnabled = false;
                }
            }
        }
        this.UIProperties.SetEnabled("Volume", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeight", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("VolumeUnitCode", this.ObjectTableName, isFieldEnabled);
        this.UIProperties.SetEnabled("GrossWeightUnitCode", this.ObjectTableName, isFieldEnabled);
    };
    Object.defineProperty(FVASimulatorComponent.prototype, "AirlineId", {
        get: function () { return this.EntityPM.AirlineId; },
        set: function (newValue) {
            var _this = this;
            if (this.EntityPM.AirlineId != newValue) {
                this.EntityPM.AirlineId = newValue;
                if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                    this.SetUIProperties_Airline();
                }
                else {
                    if (this.myAirlineService == null) {
                        this.myAirlineService = new AirlineListService_1.AirlineListService();
                    }
                    this.myAirlineService.getSingleFromCache(newValue).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            _this.Airline = myResponse.Result;
                            _this.SetUIProperties_Airline();
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "FromPortId", {
        get: function () { return this.EntityPM.FromPortId; },
        set: function (newValue) {
            if (this.EntityPM.FromPortId != newValue) {
                this.EntityPM.FromPortId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "ToPortId", {
        get: function () { return this.EntityPM.ToPortId; },
        set: function (newValue) {
            if (this.EntityPM.ToPortId != newValue) {
                this.EntityPM.ToPortId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "ETD", {
        get: function () { return this.EntityPM.ETD; },
        set: function (newValue) {
            if (this.EntityPM.ETD != newValue) {
                this.EntityPM.ETD = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "ETA", {
        get: function () { return this.EntityPM.ETA; },
        set: function (newValue) {
            if (this.EntityPM.ETA != newValue) {
                this.EntityPM.ETA = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "Volume", {
        get: function () { return this.EntityPM.Volume; },
        set: function (newValue) {
            if (this.EntityPM.Volume != newValue) {
                this.EntityPM.Volume = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "GrossWeight", {
        get: function () { return this.EntityPM.GrossWeight; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeight != newValue) {
                this.EntityPM.GrossWeight = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "VolumeUnitCode", {
        get: function () { return this.EntityPM.VolumeUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.VolumeUnitCode != newValue) {
                this.EntityPM.VolumeUnitCode = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "GrossWeightUnitCode", {
        get: function () { return this.EntityPM.GrossWeightUnitCode; },
        set: function (newValue) {
            if (this.EntityPM.GrossWeightUnitCode != newValue) {
                this.EntityPM.GrossWeightUnitCode = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "AnswerOSI", {
        get: function () { return this.EntityPM.AnswerOSI; },
        set: function (newValue) {
            if (this.EntityPM.AnswerOSI != newValue) {
                this.EntityPM.AnswerOSI = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "AnswerReasonForNoReply", {
        get: function () { return this.EntityPM.AnswerReasonForNoReply; },
        set: function (newValue) {
            if (this.EntityPM.AnswerReasonForNoReply != newValue) {
                this.EntityPM.AnswerReasonForNoReply = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(FVASimulatorComponent.prototype, "IsReasonForNoReplayChecked", {
        get: function () { return this.isReasonForNoReplayChecked; },
        set: function (newValue) {
            if (this.isReasonForNoReplayChecked != newValue) {
                this.isReasonForNoReplayChecked = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    FVASimulatorComponent.prototype.CancelClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    FVASimulatorComponent.prototype.SendClicked = function () {
        var _this = this;
        var isValid = this.Validate();
        if (isValid) {
            this.CurrentSession.StartBusyIndicator("Sending...");
            var simulatorArgs = new MessageSimulatingService_1.SimulatorArgs();
            simulatorArgs.Id = SessionLocator_1.SessionLocator.Tenant;
            simulatorArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
            simulatorArgs.MessageIdentifier = "FVA";
            simulatorArgs.AirlineId = this.AirlineId;
            simulatorArgs.EntityId = this.EntityId;
            simulatorArgs.EntityName = "FlightsSchedulesRequest";
            simulatorArgs.FVA = new MessageSimulatingService_1.SimulatorFVA();
            simulatorArgs.FVA.ETA = this.ETA;
            simulatorArgs.FVA.ETD = this.ETD;
            simulatorArgs.FVA.FromPortId = this.FromPortId;
            simulatorArgs.FVA.ToPortId = this.ToPortId;
            simulatorArgs.FVA.Volume = this.Volume;
            simulatorArgs.FVA.VolumeUnitCode = this.VolumeUnitCode;
            simulatorArgs.FVA.GrossWeight = this.GrossWeight;
            simulatorArgs.FVA.GrossWeightUnitCode = this.GrossWeightUnitCode;
            simulatorArgs.FVA.Recipient = this.Recipient;
            simulatorArgs.FVA.AnswerOSI = this.AnswerOSI;
            simulatorArgs.FVA.AnswerReasonForNoReply = this.AnswerReasonForNoReply;
            this.mySimulatingService.Simulate(simulatorArgs).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse != null) {
                    if (myResponse.HasError) {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    else {
                        var myResult = myResponse.Result;
                        if (myResult.IsValid) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Show("FVA message sent successfully");
                        }
                        else {
                            _this.ValidationErrorsList = myResult.Errors;
                        }
                    }
                }
            });
        }
    };
    FVASimulatorComponent.prototype.Validate = function () {
        var isValid = true;
        var errors = [];
        if (this.ValidationText == null) {
            this.ValidationText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        }
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.ETD == null) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.ETD");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }
        if (this.Volume != null && Tools_1.AppTool.IsNullOrEmpty(this.VolumeUnitCode)) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.VolumeUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }
        if (this.GrossWeight != null && Tools_1.AppTool.IsNullOrEmpty(this.GrossWeightUnitCode)) {
            var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.GrossWeightUnitCode");
            errors.push(this.ValidationText.replace("%FieldName", field));
        }
        if (this.IsReasonForNoReplayChecked) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.AnswerReasonForNoReply)) {
                var field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectTableName + ".F.AnswerReasonForNoReply");
                errors.push(this.ValidationText.replace("%FieldName", field));
            }
        }
        this.ValidationErrorsList = errors;
        isValid = errors.length == 0 ? true : false;
        return isValid;
    };
    FVASimulatorComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './FVASimulatorComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], FVASimulatorComponent);
    return FVASimulatorComponent;
}(BaseComponent_1.BaseComponent));
exports.FVASimulatorComponent = FVASimulatorComponent;
//# sourceMappingURL=FVASimulatorComponent.js.map