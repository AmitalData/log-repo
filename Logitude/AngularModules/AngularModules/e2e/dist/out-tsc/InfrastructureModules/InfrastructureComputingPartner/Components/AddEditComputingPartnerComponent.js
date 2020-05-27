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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var AddEditComputingPartnerComponent = /** @class */ (function (_super) {
    __extends(AddEditComputingPartnerComponent, _super);
    function AddEditComputingPartnerComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.TargetEntityName = "ComputingPartnerTable";
        _this.IsNewEntity = true;
        _this.HiddenFields = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    AddEditComputingPartnerComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.EntityPM = args.entity;
            this.EntityPM.IsDirty = false;
            this.Clone(this.EntityPM);
            this.myComputingPartnerPM = args.FatherEntity;
            this.IsNewEntity = args.IsNewEntity;
            if (!this.IsNewEntity) {
                this.UIProperties.SetEnabled("ObjectTableId", this.TargetEntityName, false);
            }
            this.HiddenFields = SessionLocator_1.SessionLocator.Tenant == 0 ? true : false;
        }
    };
    AddEditComputingPartnerComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            var errors = [];
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.TargetEntityName, errors);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Name)) {
                if (this.myComputingPartnerPM.PartnerTables.filter(function (d) { return d.Name == _this.Name && d.ObjectTableId != _this.EntityPM.ObjectTableId; })[0]) {
                    errors.push("Partner table with same Name already exists");
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableId) && this.IsNewEntity) {
                if (this.myComputingPartnerPM.PartnerTables.filter(function (d) { return d.ObjectTableId == _this.ObjectTableId && d != _this.EntityPM; })[0]) {
                    errors.push("Partner table with same Object Table already exists");
                }
            }
            this.ValidationErrorsList = errors;
            if (this.ValidationErrorsList.length == 0) {
                this.SubmitChanges();
            }
        }
        else {
            this.CancelButtonClicked();
        }
    };
    AddEditComputingPartnerComponent.prototype.Clone = function (EntityPM) {
        this.myCloner = new Cloner_1.Cloner(EntityPM);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('ObjectTableId');
        this.myCloner.AddField('MustUsePartnerList');
        this.myCloner.AddField('TransalationRequired');
        this.myCloner.AddField('TenantLevelTranslationBlocked');
        this.myCloner.AddField('ObjectTableName');
        this.myCloner.AddField('HasPartnerList');
        this.myCloner.AddField('UpdateDate');
        this.myCloner.AddField('UpdatedByUserId');
        this.myCloner.AddField('UpdatedByUserName');
        this.myCloner.AddField('ComputingPartnerId');
        this.myCloner.AddField('MarkAsDirty');
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.myComputingPartnerPM);
    };
    AddEditComputingPartnerComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditComputingPartnerComponent.prototype.SubmitChanges = function () {
        this.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.UpdatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        if (this.IsNewEntity) {
            this.IsNewEntity = false;
            if (!this.myComputingPartnerPM.PartnerTables.includes(this.EntityPM)) {
                this.myComputingPartnerPM.AddComputingPartnerTablePM(this.EntityPM);
            }
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "IsEditTableAllowed", {
        get: function () {
            var myResult = true;
            if (SessionLocator_1.SessionLocator.Tenant != 0) {
                if (this.EntityPM.Tenant == 0) {
                    myResult = false;
                }
                else if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("ComputingPartner", "ComputingPartner.A.AllowAddEditTables")) {
                    myResult = false;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "IsTranslationAllowed", {
        get: function () {
            return FeatureLocator_1.FeatureLocator.HasFeaturePermession("ComputingPartner", "ComputingPartner.A.AllowTranslation") ? true : false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) { if (this.EntityPM.Name != value)
            this.EntityPM.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "ObjectTableId", {
        get: function () { return this.EntityPM.ObjectTableId; },
        set: function (value) {
            if (this.EntityPM.ObjectTableId != value) {
                this.EntityPM.ObjectTableId = value;
                if (value != null) {
                    var objectTablePM = window.ObjectTables.filter(function (d) { return d.Id == value; })[0];
                    this.ObjectTableName = objectTablePM.Name;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "MustUsePartnerList", {
        get: function () { return this.EntityPM.MustUsePartnerList; },
        set: function (value) { if (this.EntityPM.MustUsePartnerList != value)
            this.EntityPM.MustUsePartnerList = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "TransalationRequired", {
        get: function () { return this.EntityPM.TransalationRequired; },
        set: function (value) { if (this.EntityPM.TransalationRequired != value)
            this.EntityPM.TransalationRequired = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "HasPartnerList", {
        get: function () { return this.EntityPM.HasPartnerList; },
        set: function (value) { if (this.EntityPM.HasPartnerList != value)
            this.EntityPM.HasPartnerList = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "TenantLevelTranslationBlocked", {
        get: function () { return this.EntityPM.TenantLevelTranslationBlocked; },
        set: function (value) { if (this.EntityPM.TenantLevelTranslationBlocked != value)
            this.EntityPM.TenantLevelTranslationBlocked = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "UpdateDate", {
        get: function () { return this.EntityPM.UpdateDate; },
        set: function (value) { if (this.EntityPM.UpdateDate != value)
            this.EntityPM.UpdateDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "UpdatedByUserId", {
        get: function () { return this.EntityPM.UpdatedByUserId; },
        set: function (value) { if (this.EntityPM.UpdatedByUserId != value)
            this.EntityPM.UpdatedByUserId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "UpdatedByUserName", {
        get: function () { return this.EntityPM.UpdatedByUserName; },
        set: function (value) { if (this.EntityPM.UpdatedByUserName != value)
            this.EntityPM.UpdatedByUserName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "ComputingPartnerId", {
        get: function () { return this.EntityPM.ComputingPartnerId; },
        set: function (value) { if (this.EntityPM.ComputingPartnerId != value)
            this.EntityPM.ComputingPartnerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditComputingPartnerComponent.prototype, "ObjectTableName", {
        get: function () { return this.EntityPM.ObjectTableName; },
        set: function (value) { if (this.EntityPM.ObjectTableName != value)
            this.EntityPM.ObjectTableName = value; },
        enumerable: true,
        configurable: true
    });
    AddEditComputingPartnerComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditComputingPartnerComponent = __decorate([
        core_1.Component({
            selector: 'AddEditComputingPartnerComponent',
            moduleId: module.id,
            templateUrl: './AddEditComputingPartnerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditComputingPartnerComponent);
    return AddEditComputingPartnerComponent;
}(BaseComponent_1.BaseComponent));
exports.AddEditComputingPartnerComponent = AddEditComputingPartnerComponent;
//# sourceMappingURL=AddEditComputingPartnerComponent.js.map