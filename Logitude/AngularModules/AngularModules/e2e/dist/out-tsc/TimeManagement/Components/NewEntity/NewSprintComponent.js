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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SprintPM_1 = require("../../EntityPMs/SprintPM");
var SprintPMService_1 = require("../../Services/StandardPMs/SprintPMService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var NewSprintComponent = /** @class */ (function (_super) {
    __extends(NewSprintComponent, _super);
    function NewSprintComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Sprint";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.EntityPM = new SprintPM_1.SprintPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        return _this;
    }
    Object.defineProperty(NewSprintComponent.prototype, "Name", {
        get: function () {
            return this.EntityPM.Name;
        },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewSprintComponent.prototype, "FromDate", {
        get: function () {
            return this.EntityPM.FromDate;
        },
        set: function (value) {
            if (this.EntityPM.FromDate != value) {
                this.EntityPM.FromDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewSprintComponent.prototype, "ToDate", {
        get: function () {
            return this.EntityPM.ToDate;
        },
        set: function (value) {
            if (this.EntityPM.ToDate != value) {
                this.EntityPM.ToDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewSprintComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewSprintComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating...");
        var myService = new SprintPMService_1.SprintPMService();
        myService.insert(this.EntityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    NewSprintComponent = __decorate([
        core_1.Component({
            selector: 'NewSprintComponent',
            moduleId: module.id,
            templateUrl: './NewSprintComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewSprintComponent);
    return NewSprintComponent;
}(BaseComponent_1.BaseComponent));
exports.NewSprintComponent = NewSprintComponent;
//# sourceMappingURL=NewSprintComponent.js.map