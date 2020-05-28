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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var StageListService_1 = require("../../Services/StandardLists/StageListService");
var ReOpen_StageComponent = /** @class */ (function (_super) {
    __extends(ReOpen_StageComponent, _super);
    function ReOpen_StageComponent() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.isSelectable = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    Object.defineProperty(ReOpen_StageComponent.prototype, "IsSelectable", {
        get: function () { return this.isSelectable; },
        set: function (value) { this.isSelectable = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ReOpen_StageComponent.prototype, "StageId", {
        get: function () { return this.entityPM.StageId; },
        set: function (value) {
            this.entityPM.StageId = value;
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("StageId", "Opportunity", false);
            }
            else {
                this.UIProperties.SetRequired("StageId", "Opportunity", true);
            }
        },
        enumerable: true,
        configurable: true
    });
    ReOpen_StageComponent.prototype.SetWindowArgs = function (args) {
        this.entityPM = args;
    };
    ReOpen_StageComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancle");
    };
    ReOpen_StageComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.StageId)) {
            this.ValidationErrorsList.push("Stage Field is required !");
        }
        if (this.ValidationErrorsList.length == 0) {
            var listService = new StageListService_1.StageListService();
            listService.getAllFromCache().subscribe(function (result) {
                var stage = result.Result.filter(function (d) { return d.Id == _this.StageId; })[0];
                if (stage != null) {
                    _this.entityPM.Probability = stage.Probability;
                    _this.entityPM.StageName = stage.Name;
                }
                _this.CurrentSession.CurrentEditComponent.SaveChanges("ReOpening Opportunity...");
                _this.CurrentSession.CloseCurrentWindowEmit("OK");
            });
        }
    };
    ReOpen_StageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ReOpen_StageComponent.html',
        })
    ], ReOpen_StageComponent);
    return ReOpen_StageComponent;
}(BaseComponent_1.BaseComponent));
exports.ReOpen_StageComponent = ReOpen_StageComponent;
//# sourceMappingURL=ReOpen_StageComponent.js.map