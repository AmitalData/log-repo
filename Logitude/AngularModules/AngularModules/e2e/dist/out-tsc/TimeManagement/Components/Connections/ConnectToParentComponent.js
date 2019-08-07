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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TimeManagementDomainService_1 = require("../../Services/TimeManagementDomainService");
var ConnectToParentComponent = /** @class */ (function (_super) {
    __extends(ConnectToParentComponent, _super);
    function ConnectToParentComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "TMProject";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.ValidationErrorsList = [];
        return _this;
    }
    ConnectToParentComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityArgs;
    };
    Object.defineProperty(ConnectToParentComponent.prototype, "Id", {
        get: function () {
            return this.projectId;
        },
        set: function (value) {
            if (this.projectId != value) {
                this.projectId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    ConnectToParentComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ConnectToParentComponent.prototype.ConnectButtonClicked = function () {
        var _this = this;
        if (this.EntityPM.IsInnerProject) {
            this.ValidationErrorsList.push('This is an inner project , cannot be connected to parent project');
        }
        else {
            if (this.Id == null) {
                this.ValidationErrorsList.push('you must select a project');
            }
            else {
                this.CurrentSession.StartBusyIndicator("Updating...");
                var myService = new TimeManagementDomainService_1.TimeManagementDomainService();
                myService.GetNewTMProjectConnect(this.EntityPM.Id, this.projectId).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        _this.EntityPM.ProjectNumber = myResponse.Result;
                        _this.EntityPM.IsDirty = false;
                        _this.CurrentSession.CloseCurrentWindowEmit('OK');
                    }
                    else {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
        }
    };
    ConnectToParentComponent = __decorate([
        core_1.Component({
            selector: 'ConnectToParentComponent',
            moduleId: module.id,
            templateUrl: './ConnectToParentComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ConnectToParentComponent);
    return ConnectToParentComponent;
}(BaseComponent_1.BaseComponent));
exports.ConnectToParentComponent = ConnectToParentComponent;
//# sourceMappingURL=ConnectToParentComponent.js.map