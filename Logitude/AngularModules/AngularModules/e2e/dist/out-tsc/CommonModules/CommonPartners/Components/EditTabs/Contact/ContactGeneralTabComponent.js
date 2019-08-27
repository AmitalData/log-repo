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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var ContactInputTemplate_1 = require("../../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate");
var ContactGeneralTabComponent = /** @class */ (function () {
    function ContactGeneralTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ObjectTableName = "Contact";
        this.Retries = 0;
        this.EntityPM = entityArgs.EntityPM;
        this.RunComponent();
    }
    ContactGeneralTabComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ContactGeneralTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ContactGeneralTabComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/ContactInputTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            var myTemplate = cmpRef.instance;
            var args = new ContactInputTemplate_1.ContactInputTemplateArgs();
            args.EntityPM = _this.EntityPM;
            myTemplate.InitTemplate(args);
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ContactGeneralTabComponent.prototype, "viewContainerRef", void 0);
    ContactGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContactGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ContactGeneralTabComponent);
    return ContactGeneralTabComponent;
}());
exports.ContactGeneralTabComponent = ContactGeneralTabComponent;
//# sourceMappingURL=ContactGeneralTabComponent.js.map