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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var DocumentFilingInboxTabComponent = /** @class */ (function (_super) {
    __extends(DocumentFilingInboxTabComponent, _super);
    function DocumentFilingInboxTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.DataContext = _this;
        _this.ObjectTableName = "User";
        _this.ValidationErrorsList = [];
        _this.SettingsDomain = "domain.com";
        var domain = ObjectsLocator_1.ObjectsLocator.GlobalSetting.DocumentFilingEmailDomain;
        if (SessionLocator_1.SessionLocator.PrivateLableSettings) {
            domain = "inbox.dsv.co.il";
        }
        _this.SettingsDomain = domain;
        _this.EntityPM = entityArgs.EntityPM;
        return _this;
    }
    DocumentFilingInboxTabComponent.prototype.ngOnInit = function () {
    };
    Object.defineProperty(DocumentFilingInboxTabComponent.prototype, "DocumentFilingInbox", {
        get: function () {
            return this.EntityPM.DocumentFilingInbox;
        },
        set: function (value) {
            if (this.EntityPM.DocumentFilingInbox != value) {
                this.EntityPM.DocumentFilingInbox = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    DocumentFilingInboxTabComponent = __decorate([
        core_1.Component({
            selector: 'DocumentFilingInboxTabComponent',
            moduleId: module.id,
            templateUrl: './DocumentFilingInboxTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], DocumentFilingInboxTabComponent);
    return DocumentFilingInboxTabComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentFilingInboxTabComponent = DocumentFilingInboxTabComponent;
//# sourceMappingURL=DocumentFilingInboxTabComponent.js.map