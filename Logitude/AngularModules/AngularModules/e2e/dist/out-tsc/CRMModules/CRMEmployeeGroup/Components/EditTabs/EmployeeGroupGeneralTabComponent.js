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
var EmployeeGroupLinePM_1 = require("../../../../CRM/EntityPMs/EmployeeGroupLinePM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var EmployeeGroupGeneralTabComponent = /** @class */ (function (_super) {
    __extends(EmployeeGroupGeneralTabComponent, _super);
    function EmployeeGroupGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "EmployeeGroup";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.entityPM = entityArgs.EntityPM;
        _this.FillGroupLines();
        return _this;
    }
    EmployeeGroupGeneralTabComponent.prototype.FillGroupLines = function () {
        var _this = this;
        this.EmployeeGroupLines = [];
        if (this.entityPM.EmployeeGroupLines.length > 0) {
            this.entityPM.EmployeeGroupLines.forEach(function (item) {
                _this.EmployeeGroupLines.push(new EmployeeGroupLineData(item, _this));
            });
        }
    };
    Object.defineProperty(EmployeeGroupGeneralTabComponent.prototype, "Name", {
        get: function () { return this.entityPM.Name; },
        set: function (value) {
            if (this.entityPM.Name != value) {
                this.entityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmployeeGroupGeneralTabComponent.prototype, "Description", {
        get: function () { return this.entityPM.Description; },
        set: function (value) {
            if (this.entityPM.Description != value) {
                this.entityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmployeeGroupGeneralTabComponent.prototype, "ManagerUserId", {
        get: function () { return this.entityPM.ManagerUserId; },
        set: function (value) {
            if (this.entityPM.ManagerUserId != value) {
                this.entityPM.ManagerUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmployeeGroupGeneralTabComponent.prototype, "EscalationNotify", {
        get: function () { return this.entityPM.EscalationNotify; },
        set: function (value) {
            if (this.entityPM.EscalationNotify != value) {
                this.entityPM.EscalationNotify = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EmployeeGroupGeneralTabComponent.prototype, "Inactive", {
        get: function () { return this.entityPM.Inactive; },
        set: function (value) {
            if (this.entityPM.Inactive != value) {
                this.entityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    EmployeeGroupGeneralTabComponent.prototype.NewGroupLine = function () {
        var newLine = new EmployeeGroupLinePM_1.EmployeeGroupLinePM(null);
        newLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newLine.EmployeeGroupId = this.entityPM.Id;
        this.EmployeeGroupLines.push(new EmployeeGroupLineData(newLine, this));
    };
    EmployeeGroupGeneralTabComponent.prototype.DeleteGroupLine = function (deletedItem) {
        var selectedLinePM = deletedItem.linePM;
        if (this.entityPM.EmployeeGroupLines.indexOf(selectedLinePM) != -1) {
            this.entityPM.RemoveEmployeeGroupLine(selectedLinePM);
        }
        var index = this.EmployeeGroupLines.indexOf(deletedItem);
        if (index > -1) {
            this.EmployeeGroupLines.splice(index, 1);
        }
    };
    EmployeeGroupGeneralTabComponent.prototype.RefreshLines = function (args) {
        if (args != null) {
            var myUser = args.toString().split(':')[0];
            this.EmployeeGroupLines.forEach(function (item) {
                item.myCheck = args.toString().split(':')[1];
                if (item.UserId == myUser) {
                    item.isDefaultOwner = true;
                }
                else {
                    item.isDefaultOwner = false;
                }
            });
        }
    };
    EmployeeGroupGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EmployeeGroupGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], EmployeeGroupGeneralTabComponent);
    return EmployeeGroupGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.EmployeeGroupGeneralTabComponent = EmployeeGroupGeneralTabComponent;
var EmployeeGroupLineData = /** @class */ (function (_super) {
    __extends(EmployeeGroupLineData, _super);
    function EmployeeGroupLineData(entity, trigger) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "EmployeeGroupLine";
        _this.DataContext = _this;
        _this.myCheck = "F";
        _this.isDefaultOwner = false;
        _this.groupPM = trigger.entityPM;
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
    Object.defineProperty(EmployeeGroupLineData.prototype, "IsDefaultOwner", {
        get: function () {
            if (this.myCheck == "T") {
                var myValue = this.linePM.IsDefaultOwner;
                this.myCheck = "F";
                myValue = this.isDefaultOwner;
                this.linePM.IsDefaultOwner = myValue;
            }
            return this.linePM.IsDefaultOwner;
        },
        set: function (value) {
            if (this.linePM.IsDefaultOwner != value) {
                this.linePM.IsDefaultOwner = value;
                if (this.myCheck == "F") {
                    this.FireRefreshOwner();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    EmployeeGroupLineData.prototype.FireRefreshOwner = function () {
        var args = this.UserId + ":" + "T";
        if (args != null) {
            this.myCheck = args.toString().split(':')[1];
            this.trigger.RefreshLines(args);
        }
    };
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
//# sourceMappingURL=EmployeeGroupGeneralTabComponent.js.map