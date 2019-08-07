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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CreateTenantValidationScreenComponent = /** @class */ (function () {
    function CreateTenantValidationScreenComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CreateTenantValidationScreenComponent.prototype.ngOnInit = function () {
    };
    CreateTenantValidationScreenComponent.prototype.SetWindowArgs = function (args) {
        var messageError = args.MessageError;
        var contactEmail = args.ContactEmail;
        var contactName = args.ContactName;
        var customerName = args.CustomerName;
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(messageError)) {
            this.ValidationErrorsList.push(messageError);
        }
        else {
            if (Tools_1.AppTool.IsNullOrEmpty(contactEmail))
                this.ValidationErrorsList.push("Contact Email Field is Required");
            if (Tools_1.AppTool.IsNullOrEmpty(contactName))
                this.ValidationErrorsList.push("Contact Name Field is Required");
            if (Tools_1.AppTool.IsNullOrEmpty(customerName))
                this.ValidationErrorsList.push("Customer Name Field is Required");
        }
    };
    CreateTenantValidationScreenComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CreateTenantValidationScreenComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CreateTenantValidationScreenComponent',
            templateUrl: './CreateTenantValidationScreenComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CreateTenantValidationScreenComponent);
    return CreateTenantValidationScreenComponent;
}());
exports.CreateTenantValidationScreenComponent = CreateTenantValidationScreenComponent;
//# sourceMappingURL=CreateTenantValidationScreenComponent.js.map