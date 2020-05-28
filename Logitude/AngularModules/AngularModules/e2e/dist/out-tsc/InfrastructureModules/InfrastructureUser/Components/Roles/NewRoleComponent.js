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
var Tools_1 = require("../../../../Infrastructure/Tools");
var RolePMService_1 = require("../../../../Common/Services/StandardPMs/RolePMService");
var RoleListService_1 = require("../../../../Common/Services/StandardLists/RoleListService");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var NewRoleComponent = /** @class */ (function (_super) {
    __extends(NewRoleComponent, _super);
    function NewRoleComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.DataContext = _this;
        _this.ObjectTableName = "Role";
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.IsNewEntity = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.InitializeServices();
        return _this;
    }
    NewRoleComponent.prototype.InitializeServices = function () {
        this.myRolePMService = new RolePMService_1.RolePMService();
        this.myRoleListService = new RoleListService_1.RoleListService();
    };
    NewRoleComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.EntityPM = args['RolePM'];
        this.IsNewEntity = args['IsNew'];
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.SetUIProperties();
        });
    };
    NewRoleComponent.prototype.SetUIProperties = function () {
        if (this.IsNewEntity) {
            var isFieldEnabled = false;
            var isFieldRequired = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ParentRoleId)) {
                isFieldEnabled = true;
                isFieldRequired = false;
            }
            this.UIProperties.SetRequired("ParentRoleId", this.ObjectTableName, isFieldRequired);
            this.UIProperties.SetEnabled("Name", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("RoleTypeCode", this.ObjectTableName, isFieldEnabled);
            this.UIProperties.SetEnabled("Description", this.ObjectTableName, isFieldEnabled);
        }
        else {
            this.UIProperties.SetEnabled("ParentRoleId", this.ObjectTableName, false);
        }
    };
    Object.defineProperty(NewRoleComponent.prototype, "ParentRoleId", {
        get: function () { return this.EntityPM.ParentRoleId; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.ParentRoleId != value) {
                this.EntityPM.ParentRoleId = value;
                this.SetUIProperties();
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.Name = null;
                    this.RoleTypeCode = null;
                }
                else {
                    this.myRoleListService.getSingle(value).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var list = myResponse.Result;
                            if (list != null) {
                                _this.Name = list.Name;
                                _this.RoleTypeCode = list.RoleTypeCode;
                            }
                        }
                    });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRoleComponent.prototype, "RoleTypeCode", {
        get: function () { return this.EntityPM.RoleTypeCode; },
        set: function (value) {
            if (this.EntityPM.RoleTypeCode != value) {
                this.EntityPM.RoleTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRoleComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value) {
                this.EntityPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewRoleComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewRoleComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewRoleComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.ParentRoleId)) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            errors.push(msg.replace("%FieldName", "Parent Role"));
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            if (this.IsNewEntity) {
                var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
                confirmWindow.Show("The user who will be assigned this role will need to logout and login so the changes will take place");
                confirmWindow.WindowClosed.subscribe(function (event) {
                    if (confirmWindow.Yes) {
                        _this.CurrentSession.StartBusyIndicatorSaving();
                        _this.myRolePMService.insert(_this.EntityPM).subscribe(function (myResponse) {
                            _this.CurrentSession.StopBusyIndicator();
                            if (!myResponse.HasError) {
                                _this.CurrentSession.CloseCurrentWindowEmit("OK");
                            }
                        });
                    }
                });
            }
            else {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.myRolePMService.update(this.EntityPM).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        _this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }
                });
            }
        }
    };
    NewRoleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewRoleComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NewRoleComponent);
    return NewRoleComponent;
}(BaseComponent_1.BaseComponent));
exports.NewRoleComponent = NewRoleComponent;
//# sourceMappingURL=NewRoleComponent.js.map