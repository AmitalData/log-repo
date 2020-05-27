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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var Tools_2 = require("../../../../../Shipment/Tools");
var OtherPartnersTabComponent = /** @class */ (function (_super) {
    __extends(OtherPartnersTabComponent, _super);
    function OtherPartnersTabComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsEditingEnabled = false;
        // Validate
        _this.ShowWarning_NominatedHandlingPartyId = false;
        _this.ShowWarning_Participant1 = false;
        _this.ShowWarning_Participant2 = false;
        _this.ShowWarning_Participant3 = false;
        return _this;
    }
    OtherPartnersTabComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
        this.EntityPM = this.Wizard.EntityPM;
        this.ObjectTableName = this.Wizard.ObjectTableName;
        this.Listen();
        this.Validate();
        this.SetUIProperties();
    };
    OtherPartnersTabComponent.prototype.RefreshTab = function () {
        this.Validate();
    };
    OtherPartnersTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.Wizard != null) {
            this.Wizard.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
            this.Wizard.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.Wizard.EntityPM;
                    _this.SetUIProperties();
                }
            });
        }
    };
    OtherPartnersTabComponent.prototype.SetUIProperties = function () {
        this.IsEditingEnabled = Tools_2.ShipmentTool.IsEditingEnabled(this.EntityPM);
        this.UIProperties.SetEnabled('NominatedHandlingPartyId', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantIdCode1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationCode1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationName1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationPortCode1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationReference1', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantIdCode2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationCode2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationName2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationPortCode2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationReference2', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantIdCode3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationCode3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationName3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationPortCode3', this.ObjectTableName, this.IsEditingEnabled);
        this.UIProperties.SetEnabled('OtherParticipantInformationReference3', this.ObjectTableName, this.IsEditingEnabled);
    };
    OtherPartnersTabComponent.prototype.FireWizardEvent = function () {
        this.Wizard.ValidateScreen_OTP();
    };
    OtherPartnersTabComponent.prototype.Validate = function () {
        this.Validate_Nominated();
        this.Validate_Participant1();
        this.Validate_Participant2();
        this.Validate_Participant3();
    };
    OtherPartnersTabComponent.prototype.Validate_Nominated = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "NominatedHandlingPartyId"; })[0];
            if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.NominatedHandlingPartyId)) {
                isValid = false;
            }
            this.ShowWarning_NominatedHandlingPartyId = !isValid;
        }
    };
    OtherPartnersTabComponent.prototype.Validate_Participant1 = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            var isFieldFilled = Tools_2.ShipmentTool.IsParticipant1Filled(this.EntityPM);
            if (isFieldFilled) {
                var isMissingData = Tools_2.ShipmentTool.IsParticipant1MissingData(this.EntityPM);
                if (isMissingData) {
                    isValid = false;
                }
                else {
                    if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode1)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode1)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode1)) {
                        isValid = false;
                    }
                    else if (this.EntityPM.OtherParticipantInformationPortCode1.length != 3) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationName1)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference1)) {
                        isValid = false;
                    }
                }
            }
            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "OtherParticipantIdCode1"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.OtherParticipantIdCode1)) {
                    isValid = false;
                }
            }
            this.ShowWarning_Participant1 = !isValid;
        }
    };
    OtherPartnersTabComponent.prototype.Validate_Participant2 = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            var isFieldFilled = Tools_2.ShipmentTool.IsParticipant2Filled(this.EntityPM);
            if (isFieldFilled) {
                var isMissingData = Tools_2.ShipmentTool.IsParticipant2MissingData(this.EntityPM);
                if (isMissingData) {
                    isValid = false;
                }
                else {
                    if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode2)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode2)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode2)) {
                        isValid = false;
                    }
                    else if (this.EntityPM.OtherParticipantInformationPortCode2.length != 3) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationName2)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference2)) {
                        isValid = false;
                    }
                }
            }
            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "OtherParticipantIdCode2"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.OtherParticipantIdCode2)) {
                    isValid = false;
                }
            }
            this.ShowWarning_Participant2 = !isValid;
        }
    };
    OtherPartnersTabComponent.prototype.Validate_Participant3 = function () {
        if (!this.Wizard.IsImportWizard) {
            var isValid = true;
            var isFieldFilled = Tools_2.ShipmentTool.IsParticipant3Filled(this.EntityPM);
            if (isFieldFilled) {
                var isMissingData = Tools_2.ShipmentTool.IsParticipant3MissingData(this.EntityPM);
                if (isMissingData) {
                    isValid = false;
                }
                else {
                    if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantIdCode3)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsAlphaNumeric(this.EntityPM.OtherParticipantInformationCode3)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsAlpha(this.EntityPM.OtherParticipantInformationPortCode3)) {
                        isValid = false;
                    }
                    else if (this.EntityPM.OtherParticipantInformationPortCode3.length != 3) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationName3)) {
                        isValid = false;
                    }
                    else if (!Tools_1.FormatTool.IsText(this.EntityPM.OtherParticipantInformationReference3)) {
                        isValid = false;
                    }
                }
            }
            if (isValid) {
                var myFieldRule = this.Wizard.AirlineRulesList.filter(function (d) { return d.RuleFieldName == "OtherParticipantIdCode3"; })[0];
                if (!Tools_1.AppTool.IsAirlineRuleFieldValid(myFieldRule, this.EntityPM.OtherParticipantIdCode3)) {
                    isValid = false;
                }
            }
            this.ShowWarning_Participant3 = !isValid;
        }
    };
    Object.defineProperty(OtherPartnersTabComponent.prototype, "NominatedHandlingPartyId", {
        // [Properties]
        get: function () { return this.EntityPM.NominatedHandlingPartyId; },
        set: function (newValue) {
            if (this.EntityPM.NominatedHandlingPartyId != newValue) {
                this.EntityPM.NominatedHandlingPartyId = newValue;
                this.FireWizardEvent();
                this.Validate_Nominated();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantIdCode1", {
        get: function () { return this.EntityPM.OtherParticipantIdCode1; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantIdCode1 != newValue) {
                this.EntityPM.OtherParticipantIdCode1 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantIdCode2", {
        get: function () { return this.EntityPM.OtherParticipantIdCode2; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantIdCode2 != newValue) {
                this.EntityPM.OtherParticipantIdCode2 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantIdCode3", {
        get: function () { return this.EntityPM.OtherParticipantIdCode3; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantIdCode3 != newValue) {
                this.EntityPM.OtherParticipantIdCode3 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant3();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationCode1", {
        get: function () { return this.EntityPM.OtherParticipantInformationCode1; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationCode1 != newValue) {
                this.EntityPM.OtherParticipantInformationCode1 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationCode2", {
        get: function () { return this.EntityPM.OtherParticipantInformationCode2; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationCode2 != newValue) {
                this.EntityPM.OtherParticipantInformationCode2 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationCode3", {
        get: function () { return this.EntityPM.OtherParticipantInformationCode3; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationCode3 != newValue) {
                this.EntityPM.OtherParticipantInformationCode3 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant3();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationName1", {
        get: function () { return this.EntityPM.OtherParticipantInformationName1; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationName1 != newValue) {
                this.EntityPM.OtherParticipantInformationName1 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationName2", {
        get: function () { return this.EntityPM.OtherParticipantInformationName2; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationName2 != newValue) {
                this.EntityPM.OtherParticipantInformationName2 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationName3", {
        get: function () { return this.EntityPM.OtherParticipantInformationName3; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationName3 != newValue) {
                this.EntityPM.OtherParticipantInformationName3 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant3();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationPortCode1", {
        get: function () { return this.EntityPM.OtherParticipantInformationPortCode1; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationPortCode1 != newValue) {
                this.EntityPM.OtherParticipantInformationPortCode1 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationPortCode2", {
        get: function () { return this.EntityPM.OtherParticipantInformationPortCode2; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationPortCode2 != newValue) {
                this.EntityPM.OtherParticipantInformationPortCode2 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationPortCode3", {
        get: function () { return this.EntityPM.OtherParticipantInformationPortCode3; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationPortCode3 != newValue) {
                this.EntityPM.OtherParticipantInformationPortCode3 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant3();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationReference1", {
        get: function () { return this.EntityPM.OtherParticipantInformationReference1; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationReference1 != newValue) {
                this.EntityPM.OtherParticipantInformationReference1 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant1();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationReference2", {
        get: function () { return this.EntityPM.OtherParticipantInformationReference2; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationReference2 != newValue) {
                this.EntityPM.OtherParticipantInformationReference2 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant2();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OtherPartnersTabComponent.prototype, "OtherParticipantInformationReference3", {
        get: function () { return this.EntityPM.OtherParticipantInformationReference3; },
        set: function (newValue) {
            if (this.EntityPM.OtherParticipantInformationReference3 != newValue) {
                this.EntityPM.OtherParticipantInformationReference3 = newValue;
                this.FireWizardEvent();
                this.Validate_Participant3();
            }
        },
        enumerable: true,
        configurable: true
    });
    OtherPartnersTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'OtherPartnersTabComponent',
            templateUrl: './OtherPartnersTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], OtherPartnersTabComponent);
    return OtherPartnersTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OtherPartnersTabComponent = OtherPartnersTabComponent;
//# sourceMappingURL=OtherPartnersTabComponent.js.map