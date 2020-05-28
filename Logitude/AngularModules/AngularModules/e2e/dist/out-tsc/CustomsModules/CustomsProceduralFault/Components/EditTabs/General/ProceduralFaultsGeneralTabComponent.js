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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var LocationDirective_1 = require("../../../../../Infrastructure/Utilities/LocationDirective");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var ProceduralFaultPMService_1 = require("../../../../../Customs/Services/StandardPMs/ProceduralFaultPMService");
var ProceduralFaultsGeneralTabComponent = /** @class */ (function (_super) {
    __extends(ProceduralFaultsGeneralTabComponent, _super);
    function ProceduralFaultsGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Customs.ProceduralFault";
        _this.DataContext = _this;
        _this.IsNewEntity = false;
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.proceduralFaultPMService = new ProceduralFaultPMService_1.ProceduralFaultPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    ProceduralFaultsGeneralTabComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.EntityPM = args.CurrentEntity;
            this.IsNewEntity = args.IsNewEntity;
        }
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.ProceduralFault";
    };
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "DeclarationNumber", {
        //#region Properties
        get: function () { return this.EntityPM ? this.EntityPM.DeclarationNumber : null; },
        set: function (value) {
            if (this.EntityPM.DeclarationNumber != value) {
                this.EntityPM.DeclarationNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "InspectionTypeName", {
        get: function () { return this.EntityPM ? this.EntityPM.InspectionTypeName : null; },
        set: function (value) {
            if (this.EntityPM.InspectionTypeName != value) {
                this.EntityPM.InspectionTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "InputTypeCode", {
        get: function () { return this.EntityPM ? this.EntityPM.InputTypeCode : null; },
        set: function (value) {
            if (this.EntityPM.InputTypeCode != value) {
                this.EntityPM.InputTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "InputTypeName", {
        get: function () { return this.EntityPM ? this.EntityPM.InputTypeName : null; },
        set: function (value) {
            if (this.EntityPM.InputTypeName != value) {
                this.EntityPM.InputTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "IsCustProceduralFaultCountabl", {
        get: function () { return this.EntityPM ? this.EntityPM.IsCustProceduralFaultCountabl : null; },
        set: function (value) {
            if (this.EntityPM.IsCustProceduralFaultCountabl != value) {
                this.EntityPM.IsCustProceduralFaultCountabl = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "Remarks", {
        get: function () { return this.EntityPM ? this.EntityPM.Remarks : null; },
        set: function (value) {
            if (this.EntityPM.Remarks != value) {
                this.EntityPM.Remarks = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "RansomViolationSum", {
        get: function () { return this.EntityPM ? this.EntityPM.RansomViolationSum : null; },
        set: function (value) {
            if (this.EntityPM.RansomViolationSum != value) {
                this.EntityPM.RansomViolationSum = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "IsAgentResponsibility", {
        get: function () { return this.EntityPM ? this.EntityPM.IsAgentResponsibility : null; },
        set: function (value) {
            if (this.EntityPM.IsAgentResponsibility != value) {
                this.EntityPM.IsAgentResponsibility = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "UpdateDate", {
        get: function () { return this.EntityPM ? this.EntityPM.UpdateDate : null; },
        set: function (value) {
            if (this.EntityPM.UpdateDate != value) {
                this.EntityPM.UpdateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "LeadingDocumentVersion", {
        get: function () { return this.EntityPM ? this.EntityPM.LeadingDocumentVersion : null; },
        set: function (value) {
            if (this.EntityPM.LeadingDocumentVersion != value) {
                this.EntityPM.LeadingDocumentVersion = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProceduralFaultsGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM ? this.EntityPM.Notes : null; },
        set: function (value) {
            if (this.EntityPM.Notes != value) {
                this.EntityPM.Notes = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ProceduralFaultsGeneralTabComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.proceduralFaultPMService.update(this.EntityPM).subscribe(function (response) {
            var result = response.Result;
            _this.CurrentSession.CloseCurrentWindow();
        });
    };
    ProceduralFaultsGeneralTabComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Cancel");
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], ProceduralFaultsGeneralTabComponent.prototype, "AllLocations", void 0);
    ProceduralFaultsGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ProceduralFaultsGeneralTabComponent.html',
            providers: [EntityArgs_1.EntityArgs],
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ProceduralFaultsGeneralTabComponent);
    return ProceduralFaultsGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.ProceduralFaultsGeneralTabComponent = ProceduralFaultsGeneralTabComponent;
//# sourceMappingURL=ProceduralFaultsGeneralTabComponent.js.map