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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AgentGeneralTabComponent = /** @class */ (function (_super) {
    __extends(AgentGeneralTabComponent, _super);
    function AgentGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Agent";
        _this.LabelColumnWidth = 100;
        _this.ControlColumnWidth = 200;
        _this.DataContext = _this;
        _this.ScreenCode = "Agent.AdditionalFields";
        _this.Retries = 0;
        _this.isRAFieldsVisibile = false;
        _this.EntityPM = entityArgs.EntityPM;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        _this.RunComponent();
        return _this;
    }
    AgentGeneralTabComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    AgentGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    AgentGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AgentGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.entityArgs.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode);
        });
    };
    AgentGeneralTabComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("Code", this.ObjectTableName, false);
        //var isRAFieldsVisibile: boolean = false;
        if (this.TenantPM.RegulatedAgentRegimeActivated) {
            this.isRAFieldsVisibile = true;
        }
        this.UIProperties.SetVisibility("RegulatedAgentCode", this.ObjectTableName, this.isRAFieldsVisibile);
    };
    Object.defineProperty(AgentGeneralTabComponent.prototype, "Code", {
        // Properties 
        get: function () { return this.EntityPM.Code; },
        set: function (newValue) {
            if (this.EntityPM.Code != newValue) {
                this.EntityPM.Code = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "EnglishName", {
        get: function () { return this.EntityPM.EnglishName; },
        set: function (newValue) {
            if (this.EntityPM.EnglishName != newValue) {
                this.EntityPM.EnglishName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (newValue) {
            if (this.EntityPM.LocalName != newValue) {
                this.EntityPM.LocalName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "IATACode", {
        get: function () { return this.EntityPM.IATACode; },
        set: function (newValue) {
            if (this.EntityPM.IATACode != newValue) {
                this.EntityPM.IATACode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "CASSCode", {
        get: function () { return this.EntityPM.CASSCode; },
        set: function (newValue) {
            if (this.EntityPM.CASSCode != newValue) {
                this.EntityPM.CASSCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "RegulatedAgentCode", {
        get: function () { return this.EntityPM.RegulatedAgentCode; },
        set: function (newValue) {
            if (this.EntityPM.RegulatedAgentCode != newValue) {
                this.EntityPM.RegulatedAgentCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "Website", {
        get: function () { return this.EntityPM.Website; },
        set: function (newValue) {
            if (this.EntityPM.Website != newValue) {
                this.EntityPM.Website = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (newValue) {
            if (this.EntityPM.Notes != newValue) {
                this.EntityPM.Notes = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgentGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (newValue) {
            if (this.EntityPM.InActive != newValue) {
                this.EntityPM.InActive = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AgentGeneralTabComponent.prototype, "viewContainerRef", void 0);
    AgentGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AgentGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], AgentGeneralTabComponent);
    return AgentGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.AgentGeneralTabComponent = AgentGeneralTabComponent;
//# sourceMappingURL=AgentGeneralTabComponent.js.map