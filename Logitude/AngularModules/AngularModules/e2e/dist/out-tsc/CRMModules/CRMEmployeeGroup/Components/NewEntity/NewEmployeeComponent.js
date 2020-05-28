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
var EmployeeGroupPM_1 = require("../../../../CRM/EntityPMs/EmployeeGroupPM");
var EmployeeGroupPMService_1 = require("../../../../CRM/Services/StandardPMs/EmployeeGroupPMService");
var EmployeeGroupPMInitService_1 = require("../../../../CRM/EntityPMInitServices/EmployeeGroupPMInitService");
var EmployeeGroupLinePM_1 = require("../../../../CRM/EntityPMs/EmployeeGroupLinePM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var NewEmployeeComponent = /** @class */ (function (_super) {
    __extends(NewEmployeeComponent, _super);
    function NewEmployeeComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "EmployeeGroup";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityPM = new EmployeeGroupPM_1.EmployeeGroupPM();
        _this.EmployeeGroupLines = [];
        EmployeeGroupPMInitService_1.EmployeeGroupPMInitService.InitValues(_this.entityPM, true);
        return _this;
    }
    Object.defineProperty(NewEmployeeComponent.prototype, "Name", {
        get: function () { return this.entityPM.Name; },
        set: function (value) {
            if (this.entityPM.Name != value) {
                this.entityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewEmployeeComponent.prototype, "Description", {
        get: function () { return this.entityPM.Description; },
        set: function (value) {
            if (this.entityPM.Description != value) {
                this.entityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewEmployeeComponent.prototype, "ManagerUserId", {
        get: function () { return this.entityPM.ManagerUserId; },
        set: function (value) {
            if (this.entityPM.ManagerUserId != value) {
                this.entityPM.ManagerUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewEmployeeComponent.prototype, "EscalationNotify", {
        get: function () { return this.entityPM.EscalationNotify; },
        set: function (value) {
            if (this.entityPM.EscalationNotify != value) {
                this.entityPM.EscalationNotify = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewEmployeeComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewEmployeeComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (this.EmployeeGroupLines.length > 0) {
            this.EmployeeGroupLines.forEach(function (item) {
                if (Tools_1.AppTool.IsNullOrEmpty(item.UserId)) {
                    errors.push("Some lines have empty user field");
                }
            });
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new EmployeeGroupPMService_1.EmployeeGroupPMService();
            service.insert(this.entityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit('ok');
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    NewEmployeeComponent.prototype.NewGroupLine = function () {
        var newLine = new EmployeeGroupLinePM_1.EmployeeGroupLinePM(null);
        newLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newLine.EmployeeGroupId = this.entityPM.Id;
        this.EmployeeGroupLines.push(new EmployeeGroupLineData(newLine, this.entityPM, this));
    };
    NewEmployeeComponent.prototype.DeleteGroupLine = function (deletedItem) {
        var selectedLinePM = deletedItem.linePM;
        if (this.entityPM.EmployeeGroupLines.indexOf(selectedLinePM) != -1) {
            this.entityPM.RemoveEmployeeGroupLine(selectedLinePM);
        }
        var index = this.EmployeeGroupLines.indexOf(deletedItem);
        if (index > -1) {
            this.EmployeeGroupLines.splice(index, 1);
        }
    };
    NewEmployeeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewEmployeeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewEmployeeComponent);
    return NewEmployeeComponent;
}(BaseComponent_1.BaseComponent));
exports.NewEmployeeComponent = NewEmployeeComponent;
var EmployeeGroupLineData = /** @class */ (function (_super) {
    __extends(EmployeeGroupLineData, _super);
    function EmployeeGroupLineData(entity, group, trigger) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "EmployeeGroupLine";
        _this.DataContext = _this;
        _this.groupPM = group;
        _this.linePM = entity;
        _this.trigger = trigger;
        return _this;
    }
    Object.defineProperty(EmployeeGroupLineData.prototype, "UserId", {
        get: function () { return this.linePM.UserId; },
        set: function (value) {
            if (this.linePM.UserId != value) {
                this.linePM.UserId = value;
                this.OnUserChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    EmployeeGroupLineData.prototype.OnUserChanged = function () {
        if (Tools_1.AppTool.IsNullOrEmpty(this.UserId)) {
            if (this.groupPM.EmployeeGroupLines.indexOf(this.linePM) != -1) {
                this.groupPM.RemoveEmployeeGroupLine(this.linePM);
            }
        }
        else {
            if (this.groupPM.EmployeeGroupLines.indexOf(this.linePM) == -1) {
                this.groupPM.AddEmployeeGroupLine(this.linePM);
            }
        }
    };
    return EmployeeGroupLineData;
}(BaseComponent_1.BaseComponent));
exports.EmployeeGroupLineData = EmployeeGroupLineData;
//# sourceMappingURL=NewEmployeeComponent.js.map