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
var Tools_1 = require("../../../../Infrastructure/Tools");
var LBPTeamMemberPM_1 = require("../../../../Infrastructure/EntityPMs/LBPTeamMemberPM");
var TeamMemberBusinessRolePM_1 = require("../../../../Infrastructure/EntityPMs/TeamMemberBusinessRolePM");
var BusinessRoleListService_1 = require("../../../../Infrastructure/Services/StandardLists/BusinessRoleListService");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var BusinessRoleExtendedListService_1 = require("../../../../Infrastructure/Services/ExtendedLists/BusinessRoleExtendedListService");
var TeamGeneralTabComponent = /** @class */ (function (_super) {
    __extends(TeamGeneralTabComponent, _super);
    function TeamGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.Session = SessionLocator_1.SessionLocator.Tenant;
        _this.DataContext = _this;
        _this.ObjectTableName = "Team";
        _this.BusinessRolesList = [];
        _this.EntityPM = entityArgs.EntityPM;
        _this.TeamsFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
        _this.TeamsFilterItems.addAdditionalFilter("Id", _this.EntityPM.Id, null, null, "NotEqual", false, false, false, "String");
        return _this;
    }
    TeamGeneralTabComponent.prototype.ngOnInit = function () {
        this.GetBusinessRoles();
    };
    TeamGeneralTabComponent.prototype.GetBusinessRoles = function () {
        var _this = this;
        var service = new BusinessRoleListService_1.BusinessRoleListService();
        service.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.BusinessRolesList = myResponse.Result;
                _this.FillMemberLines();
            }
        });
    };
    TeamGeneralTabComponent.prototype.FillMemberLines = function () {
        var _this = this;
        this.MembersLines = [];
        if (this.EntityPM.MemberLines.length > 0) {
            this.EntityPM.MemberLines.forEach(function (item) {
                _this.MembersLines.push(new MembersLineData(item, _this));
            });
        }
    };
    Object.defineProperty(TeamGeneralTabComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value)
                this.EntityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamGeneralTabComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value)
                this.EntityPM.LocalName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamGeneralTabComponent.prototype, "ManagerUserId", {
        get: function () { return this.EntityPM.ManagerUserId; },
        set: function (value) {
            if (this.EntityPM.ManagerUserId != value)
                this.EntityPM.ManagerUserId = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamGeneralTabComponent.prototype, "Notify", {
        get: function () { return this.EntityPM.Notify; },
        set: function (value) {
            if (this.EntityPM.Notify != value)
                this.EntityPM.Notify = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamGeneralTabComponent.prototype, "InActive", {
        get: function () { return this.EntityPM.InActive; },
        set: function (value) {
            if (this.EntityPM.InActive != value)
                this.EntityPM.InActive = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TeamGeneralTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value)
                this.EntityPM.Notes = value;
        },
        enumerable: true,
        configurable: true
    });
    TeamGeneralTabComponent.prototype.NewMemberLine = function () {
        var newLine = new LBPTeamMemberPM_1.LBPTeamMemberPM(null);
        newLine.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newLine.TeamId = this.EntityPM.Id;
        newLine.AddedByUserId = this.EntityPM.CreatedByUserId;
        newLine.ChangeSetOp = "Insert";
        this.MembersLines.push(new MembersLineData(newLine, this));
    };
    TeamGeneralTabComponent.prototype.DeleteTeamMemberLine = function (deletedItem) {
        var selectedLinePM = deletedItem.MemberPM;
        if (this.EntityPM.MemberLines.indexOf(selectedLinePM) != -1) {
            selectedLinePM.ChangeSetOp = "Delete";
            this.EntityPM.RemoveLBPTeamMember(selectedLinePM);
        }
        var index = this.MembersLines.indexOf(deletedItem);
        if (index > -1) {
            this.MembersLines.splice(index, 1);
        }
    };
    TeamGeneralTabComponent = __decorate([
        core_1.Component({
            selector: 'TeamGeneralTabComponent',
            moduleId: module.id,
            templateUrl: './TeamGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TeamGeneralTabComponent);
    return TeamGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.TeamGeneralTabComponent = TeamGeneralTabComponent;
var MembersLineData = /** @class */ (function (_super) {
    __extends(MembersLineData, _super);
    function MembersLineData(entity, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        _this.ObjectTableName = "LBPTeamMember";
        _this.DataContext = _this;
        _this.MembersTypes = [];
        _this.BusinessRolesToggleList = [];
        _this.BusinessRolesList = [];
        _this.selectedFilter = null;
        _this.MemberPM = entity;
        _this.TeamPM = father.EntityPM;
        _this.FillBusinessRolesList();
        _this.FillToggleBusinessRoles();
        _this.CreateTypes();
        return _this;
    }
    MembersLineData.prototype.FillToggleBusinessRoles = function () {
        var _this = this;
        this.BusinessRolesToggleList = [];
        if (this.father.BusinessRolesList != null) {
            this.father.BusinessRolesList.forEach(function (item) {
                _this.BusinessRolesToggleList.push(new ToggleBusinessRoleData(item, _this));
            });
        }
    };
    MembersLineData.prototype.FillBusinessRolesList = function () {
        var _this = this;
        this.BusinessRolesList = [];
        if (this.MemberPM.BusinessRolesList != null) {
            this.MemberPM.BusinessRolesList.forEach(function (item) {
                _this.BusinessRolesList.push(new BusinessRoleData(item, _this));
            });
        }
    };
    Object.defineProperty(MembersLineData.prototype, "MemberTeamId", {
        get: function () { return this.MemberPM.MemberTeamId; },
        set: function (value) {
            if (this.MemberPM.MemberTeamId != value) {
                this.MemberPM.MemberTeamId = value;
                this.OnMemberChanged(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MembersLineData.prototype, "MemberUserId", {
        get: function () { return this.MemberPM.MemberUserId; },
        set: function (value) {
            if (this.MemberPM.MemberUserId != value)
                this.MemberPM.MemberUserId = value;
            {
                this.OnMemberChanged(value);
                //this.Load();
            }
        },
        enumerable: true,
        configurable: true
    });
    MembersLineData.prototype.Load = function () {
        var _this = this;
        var service = new BusinessRoleExtendedListService_1.BusinessRoleExtendedListService();
        service.getToggleBusinessRoles(this.MemberPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.FillToggleBusinessRoles();
            }
        });
    };
    Object.defineProperty(MembersLineData.prototype, "SelectedFilter", {
        get: function () {
            return this.selectedFilter;
        },
        set: function (value) {
            if (this.selectedFilter != value) {
                this.selectedFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    MembersLineData.prototype.CreateTypes = function () {
        this.MembersTypes = [];
        var s_entity = new TypeClass();
        s_entity.Code = "1";
        s_entity.Name = "User";
        this.MembersTypes.push(s_entity);
        var q_entity = new TypeClass();
        q_entity.Code = "2";
        q_entity.Name = "Team";
        this.MembersTypes.push(q_entity);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MemberTeamId)) {
            this.selectedFilter = q_entity;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MemberUserId)) {
            this.selectedFilter = s_entity;
        }
    };
    MembersLineData.prototype.OnMemberChanged = function (Id) {
        if (Tools_1.AppTool.IsNullOrEmpty(Id)) {
            if (this.TeamPM.MemberLines.indexOf(this.MemberPM) != -1) {
                this.MemberPM.ChangeSetOp = "Delete";
                this.TeamPM.RemoveLBPTeamMember(this.MemberPM);
            }
        }
        else {
            if (this.TeamPM.MemberLines.indexOf(this.MemberPM) == -1) {
                this.MemberPM.ChangeSetOp = "Insert";
                this.TeamPM.AddLBPTeamMember(this.MemberPM);
            }
        }
    };
    MembersLineData.prototype.DeleteBuseinssRole = function (role) {
        var item = this.BusinessRolesList.filter(function (d) { return d.TeamMemberId == role.TeamMemberId && d.BusinessRoleId == role.BusinessRoleId; })[0];
        if (item != null) {
            this.BusinessRolesList = this.BusinessRolesList.filter(function (obj) { return obj !== item; });
            if (this.MemberPM.BusinessRolesList.indexOf(item.EntityPM) != -1) {
                item.EntityPM.ChangeSetOp = "Delete";
                this.MemberPM.RemoveTeamMemberBusinessRole(item.EntityPM);
            }
        }
    };
    return MembersLineData;
}(BaseComponent_1.BaseComponent));
exports.MembersLineData = MembersLineData;
var ToggleBusinessRoleData = /** @class */ (function (_super) {
    __extends(ToggleBusinessRoleData, _super);
    function ToggleBusinessRoleData(entity, trigger) {
        var _this = _super.call(this) || this;
        _this.trigger = trigger;
        _this.ObjectTableName = "BusinessRole";
        _this.DataContext = _this;
        _this.EntityPM = entity;
        _this.isChecked = trigger.MemberPM.BusinessRolesList.filter(function (d) { return d.TeamMemberId == _this.trigger.MemberPM.Id && d.BusinessRoleId == _this.EntityPM.Id; })[0] != null ? true : false;
        return _this;
    }
    Object.defineProperty(ToggleBusinessRoleData.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value)
                this.EntityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ToggleBusinessRoleData.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value)
                this.EntityPM.LocalName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ToggleBusinessRoleData.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var newItemPM = new TeamMemberBusinessRolePM_1.TeamMemberBusinessRolePM(null);
                    newItemPM.Tenant = this.EntityPM.Tenant;
                    newItemPM.ChangeSetOp = "Insert";
                    newItemPM.TeamMemberId = this.trigger.MemberPM.Id;
                    newItemPM.BusinessRoleId = this.EntityPM.Id;
                    newItemPM.AddedByUserId = this.EntityPM.CreatedByUserId;
                    newItemPM.RoleName = this.Name;
                    if (newItemPM != null) {
                        if (this.trigger.MemberPM.BusinessRolesList.indexOf(newItemPM) == -1) {
                            newItemPM.ChangeSetOp = "Insert";
                            this.trigger.MemberPM.AddTeamMemberBusinessRole(newItemPM);
                        }
                    }
                }
                else {
                    var itemPM = this.trigger.MemberPM.BusinessRolesList.filter(function (d) { return d.TeamMemberId == _this.trigger.MemberPM.Id && d.BusinessRoleId == _this.EntityPM.Id; })[0];
                    if (itemPM != null) {
                        if (this.trigger.MemberPM.BusinessRolesList.indexOf(itemPM) != -1) {
                            itemPM.ChangeSetOp = "Delete";
                            this.trigger.MemberPM.RemoveTeamMemberBusinessRole(itemPM);
                        }
                    }
                }
                this.trigger.FillBusinessRolesList();
            }
        },
        enumerable: true,
        configurable: true
    });
    return ToggleBusinessRoleData;
}(BaseComponent_1.BaseComponent));
exports.ToggleBusinessRoleData = ToggleBusinessRoleData;
var BusinessRoleData = /** @class */ (function (_super) {
    __extends(BusinessRoleData, _super);
    function BusinessRoleData(entity, trigger) {
        var _this = _super.call(this) || this;
        _this.trigger = trigger;
        _this.ObjectTableName = "TeamMemberBusinessRole";
        _this.DataContext = _this;
        _this.EntityPM = entity;
        return _this;
    }
    Object.defineProperty(BusinessRoleData.prototype, "Id", {
        get: function () { return this.EntityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessRoleData.prototype, "TeamMemberId", {
        get: function () { return this.EntityPM.TeamMemberId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessRoleData.prototype, "BusinessRoleId", {
        get: function () { return this.EntityPM.BusinessRoleId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(BusinessRoleData.prototype, "RoleName", {
        get: function () { return this.EntityPM.RoleName; },
        set: function (value) {
            if (this.EntityPM.RoleName != value) {
                this.EntityPM.RoleName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return BusinessRoleData;
}(BaseComponent_1.BaseComponent));
exports.BusinessRoleData = BusinessRoleData;
var TypeClass = /** @class */ (function () {
    function TypeClass() {
    }
    return TypeClass;
}());
exports.TypeClass = TypeClass;
//# sourceMappingURL=TeamGeneralTabComponent.js.map