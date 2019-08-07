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
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var ConstraintAgentObjectionRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/ConstraintAgentObjectionRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var DeclarationMessagesService_1 = require("../../../../../Customs/Services/WebServices/DeclarationMessagesService");
var AgentObjectionComponent = /** @class */ (function (_super) {
    __extends(AgentObjectionComponent, _super);
    function AgentObjectionComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.DeclarationConstraint";
        _this.IsDisplayOnly = false;
        //Services
        _this.declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService;
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AgentObjectionComponent.prototype.SetWindowArgs = function (args) {
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.ConstraintPM = args.ConstraintPM;
            this.DeclarationError = args.DeclarationError;
        }
    };
    Object.defineProperty(AgentObjectionComponent.prototype, "AgentObjection", {
        get: function () {
            return this.ConstraintPM.AgentObjection;
        },
        set: function (value) {
            this.ConstraintPM.AgentObjection = value;
        },
        enumerable: true,
        configurable: true
    });
    AgentObjectionComponent.prototype.SendButtonClicked = function () {
        //1. save changes
        //this.declarationPMService.update(this.ConstraintPM).subscribe((myRespone: ServiceResponse) => {
        this.CurrentSession.CurrentEditComponent.SaveChanges();
        //2. send
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConstraintPM.AgentObjection)) {
            this.SendConstraintAgentObjectionMethod();
        }
        else {
            var message = new MessageWindow_1.MessageWindow();
            message.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.FillAgentObjection"));
        }
    };
    AgentObjectionComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AgentObjectionComponent.prototype.SendConstraintAgentObjectionMethod = function () {
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        var requestParams = new ConstraintAgentObjectionRequestParams_1.ConstraintAgentObjectionRequestParams();
        requestParams.LoggingEnabled = true;
        requestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        requestParams.ConstraintNumber = this.ConstraintPM.ConstraintNumber;
        requestParams.AgentObjection = this.ConstraintPM.AgentObjection;
        requestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        requestParams.RequestName = "Declaration Constriant";
        requestParams.ResponseName = "Declaration Constriant";
        //requestParams.TestCase = SelectedTest;
        requestParams.DeclarationId = this.ConstraintPM.DeclarationID;
        requestParams.LoggingEntityId = this.ConstraintPM.DeclarationID;
        //requestParams.//LoggingEntityReference = declarationPM.DeclarationNumber;
        requestParams.LoggingObjectTableId = ObjectTable.Id;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(requestParams.PBId, "שליחת בקשת ערעור", false).then(function (res) {
            console.log("[Send] Response/ShowProgressBar : ", res);
        }).catch(function (err) {
            console.error("[Send Service Error] ", err);
        });
        this.declarationMessagesService.PostSendDeclarationConstraintAgentObjection(requestParams).subscribe(function (myServiceResponse) {
            console.log("[Send] Response/PostSendDeclarationConstraintAgentObjection : ", myServiceResponse.Result);
            var response = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
            }
            else {
                var message = "Service returned a null response!";
            }
            //this.OnSendCompleted();
        });
    };
    AgentObjectionComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AgentObjectionComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AgentObjectionComponent);
    return AgentObjectionComponent;
}(BaseComponent_1.BaseComponent));
exports.AgentObjectionComponent = AgentObjectionComponent;
//# sourceMappingURL=AgentObjectionComponent.js.map