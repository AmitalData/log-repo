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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TeamPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/TeamPMService");
var TeamPM_1 = require("../../../../Infrastructure/EntityPMs/TeamPM");
var TeamPMInitService_1 = require("../../../../Infrastructure/EntityPMInitServices/TeamPMInitService");
var TeamNewComponent = /** @class */ (function (_super) {
    __extends(TeamNewComponent, _super);
    function TeamNewComponent() {
        var _this = _super.call(this) || this;
        _this.Session = SessionLocator_1.SessionLocator.Tenant;
        _this.DataContext = _this;
        _this.ObjectTableName = "Team";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new TeamPM_1.TeamPM();
        TeamPMInitService_1.TeamPMInitService.InitValues(_this.EntityPM, true);
        return _this;
    }
    Object.defineProperty(TeamNewComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value)
                this.EntityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamNewComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value)
                this.EntityPM.LocalName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamNewComponent.prototype, "ManagerUserId", {
        get: function () { return this.EntityPM.ManagerUserId; },
        set: function (value) {
            if (this.EntityPM.ManagerUserId != value)
                this.EntityPM.ManagerUserId = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamNewComponent.prototype, "Notify", {
        get: function () { return this.EntityPM.Notify; },
        set: function (value) {
            if (this.EntityPM.Notify != value)
                this.EntityPM.Notify = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamNewComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value)
                this.EntityPM.Notes = value;
        },
        enumerable: true,
        configurable: true
    });
    TeamNewComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TeamNewComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new TeamPMService_1.TeamPMService();
            myService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    TeamNewComponent = __decorate([
        core_1.Component({
            selector: 'TeamNewComponent',
            moduleId: module.id,
            templateUrl: './TeamNewComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TeamNewComponent);
    return TeamNewComponent;
}(BaseComponent_1.BaseComponent));
exports.TeamNewComponent = TeamNewComponent;
//# sourceMappingURL=TeamNewComponent.js.map