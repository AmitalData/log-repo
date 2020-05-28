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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var AirlineCCSSettingsTabComponent = /** @class */ (function (_super) {
    __extends(AirlineCCSSettingsTabComponent, _super);
    function AirlineCCSSettingsTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Airline";
        _this.DataContext = _this;
        _this.EntityPM = entityArgs.EntityPM;
        return _this;
    }
    AirlineCCSSettingsTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    };
    AirlineCCSSettingsTabComponent.prototype.SetUIProperties = function () {
        var isRegistrationNotesVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.ObjectTableName, "RegistrationNotes")) {
            isRegistrationNotesVisible = true;
        }
        this.UIProperties.SetVisibility("RegistrationNotes", this.ObjectTableName, isRegistrationNotesVisible);
    };
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "TTY", {
        get: function () { return this.EntityPM.TTY; },
        set: function (newValue) {
            if (this.EntityPM.TTY != newValue) {
                this.EntityPM.TTY = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKPIMA", {
        get: function () { return this.EntityPM.GLSHKPIMA; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKPIMA != newValue) {
                this.EntityPM.GLSHKPIMA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "RegistrationNotes", {
        get: function () { return this.EntityPM.RegistrationNotes; },
        set: function (newValue) {
            if (this.EntityPM.RegistrationNotes != newValue) {
                this.EntityPM.RegistrationNotes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "ChampFSRFSA", {
        get: function () { return this.EntityPM.ChampFSRFSA; },
        set: function (newValue) {
            if (this.EntityPM.ChampFSRFSA != newValue) {
                this.EntityPM.ChampFSRFSA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKFSRFSA", {
        get: function () { return this.EntityPM.GLSHKFSRFSA; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKFSRFSA != newValue) {
                this.EntityPM.GLSHKFSRFSA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "ChampFSU", {
        get: function () { return this.EntityPM.ChampFSU; },
        set: function (newValue) {
            if (this.EntityPM.ChampFSU != newValue) {
                this.EntityPM.ChampFSU = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKFSU", {
        get: function () { return this.EntityPM.GLSHKFSU; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKFSU != newValue) {
                this.EntityPM.GLSHKFSU = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "ChampFWB", {
        get: function () { return this.EntityPM.ChampFWB; },
        set: function (newValue) {
            if (this.EntityPM.ChampFWB != newValue) {
                this.EntityPM.ChampFWB = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKFWB", {
        get: function () { return this.EntityPM.GLSHKFWB; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKFWB != newValue) {
                this.EntityPM.GLSHKFWB = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "ChampFHL", {
        get: function () { return this.EntityPM.ChampFHL; },
        set: function (newValue) {
            if (this.EntityPM.ChampFHL != newValue) {
                this.EntityPM.ChampFHL = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKFHL", {
        get: function () { return this.EntityPM.GLSHKFHL; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKFHL != newValue) {
                this.EntityPM.GLSHKFHL = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "ChampFVRFVA", {
        get: function () { return this.EntityPM.ChampFVRFVA; },
        set: function (newValue) {
            if (this.EntityPM.ChampFVRFVA != newValue) {
                this.EntityPM.ChampFVRFVA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKFVRFVA", {
        get: function () { return this.EntityPM.GLSHKFVRFVA; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKFVRFVA != newValue) {
                this.EntityPM.GLSHKFVRFVA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "ChampFFRFFA", {
        get: function () { return this.EntityPM.ChampFFRFFA; },
        set: function (newValue) {
            if (this.EntityPM.ChampFFRFFA != newValue) {
                this.EntityPM.ChampFFRFFA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKFFRFFA", {
        get: function () { return this.EntityPM.GLSHKFFRFFA; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKFFRFFA != newValue) {
                this.EntityPM.GLSHKFFRFFA = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "ChampNeedsRegistration", {
        get: function () { return this.EntityPM.ChampNeedsRegistration; },
        set: function (newValue) {
            if (this.EntityPM.ChampNeedsRegistration != newValue) {
                this.EntityPM.ChampNeedsRegistration = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AirlineCCSSettingsTabComponent.prototype, "GLSHKNeedsRegistration", {
        get: function () { return this.EntityPM.GLSHKNeedsRegistration; },
        set: function (newValue) {
            if (this.EntityPM.GLSHKNeedsRegistration != newValue) {
                this.EntityPM.GLSHKNeedsRegistration = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AirlineCCSSettingsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AirlineCCSSettingsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AirlineCCSSettingsTabComponent);
    return AirlineCCSSettingsTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AirlineCCSSettingsTabComponent = AirlineCCSSettingsTabComponent;
//# sourceMappingURL=AirlineCCSSettingsTabComponent.js.map