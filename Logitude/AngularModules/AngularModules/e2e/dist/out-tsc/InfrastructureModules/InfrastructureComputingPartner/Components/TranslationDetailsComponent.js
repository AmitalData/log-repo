"use strict";
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TranslationDetailsComponent = /** @class */ (function () {
    function TranslationDetailsComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    TranslationDetailsComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.entityPM;
    };
    Object.defineProperty(TranslationDetailsComponent.prototype, "OurCode", {
        get: function () { return this.EntityPM.OurCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "PartnerCode", {
        get: function () { return this.EntityPM.PartnerCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "CreateDate", {
        get: function () { return this.EntityPM.CreateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "CreatedByUserName", {
        get: function () {
            return this.EntityPM.CreatedByUserName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "UpdatedByUserName", {
        get: function () {
            return this.EntityPM.UpdatedByUserName;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "DefaultTranslationVisibility", {
        get: function () {
            return SessionLocator_1.SessionLocator.Tenant != 0 ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "DefaultTranslation", {
        get: function () {
            return this.EntityPM.DefaultTranslationPartnerCode;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "CreateDate_Default", {
        get: function () {
            return this.EntityPM.CreatedDateDefault;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "UpdateDate_Default", {
        get: function () {
            return this.EntityPM.UpdatedDateDefault;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "CreatedByUserName_Default", {
        get: function () {
            return this.EntityPM.CreatedByUserNameDefault;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslationDetailsComponent.prototype, "UpdatedByUserName_Default", {
        get: function () {
            return this.EntityPM.UpdatedByUserNameDefault;
        },
        enumerable: true,
        configurable: true
    });
    TranslationDetailsComponent.prototype.CloseButtonClick = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TranslationDetailsComponent = __decorate([
        core_1.Component({
            selector: 'TranslationDetailsComponent',
            moduleId: module.id,
            templateUrl: './TranslationDetailsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TranslationDetailsComponent);
    return TranslationDetailsComponent;
}());
exports.TranslationDetailsComponent = TranslationDetailsComponent;
//# sourceMappingURL=TranslationDetailsComponent.js.map