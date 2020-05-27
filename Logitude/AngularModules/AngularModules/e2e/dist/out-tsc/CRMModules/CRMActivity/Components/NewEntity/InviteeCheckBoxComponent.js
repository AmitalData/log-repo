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
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var InviteeCheckBoxComponent = /** @class */ (function (_super) {
    __extends(InviteeCheckBoxComponent, _super);
    function InviteeCheckBoxComponent(cd, _entityListService) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this._entityListService = _entityListService;
        _this.DataContext = _this;
        _this.AddButtonEnabled = true;
        _this.isChecked = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isCheckedFlag = false;
        _this.isClickedFlag = false;
        _this.selectedItem = null;
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        return _this;
    }
    Object.defineProperty(InviteeCheckBoxComponent.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            if (this.isChecked != value) {
                this.isChecked = value;
                this.isCheckedFlag = true;
                this.FireCheckedEvent();
            }
        },
        enumerable: true,
        configurable: true
    });
    InviteeCheckBoxComponent.prototype.setVariables = function (rowData, fieldName) {
        var _this = this;
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.Key = Guid_1.Guid.newGuid() + fieldName;
        if (this.rowData.Email) {
            if (fieldName == "Required" && window.RequiredList) {
                var item = window.RequiredList.filter(function (d) { return d.Email.toLowerCase() == _this.rowData.Email.toLowerCase() && d.ContactId == _this.rowData.Id; })[0];
                if (item) {
                    this.IsChecked = true;
                }
                else {
                    this.IsChecked = false;
                }
            }
            else if (fieldName == "Optional" && window.OptionalList) {
                var item = window.OptionalList.filter(function (d) { return d.Email.toLowerCase() == _this.rowData.Email.toLowerCase() && d.ContactId == _this.rowData.Id; })[0];
                if (item) {
                    this.IsChecked = true;
                }
                else {
                    this.IsChecked = false;
                }
            }
            this.Destroyed();
            this.isCheckedFlag = false;
        }
    };
    InviteeCheckBoxComponent.prototype.Checkclick = function (item) {
        if (item.Email) {
            this.cd.detectChanges();
            this.isClickedFlag = true;
            this.selectedItem = item;
            this.FireCheckedEvent();
        }
    };
    InviteeCheckBoxComponent.prototype.FireCheckedEvent = function () {
        if (this.selectedItem != null && this.isCheckedFlag && this.isClickedFlag) {
            var select = new InviteeParameterInput(this.fieldName, this.selectedItem.Email, this.selectedItem.Id, this.selectedItem.EnglishName, this.IsChecked);
            this.Destroyed();
            this.CurrentSession.SessionEvent.emit({ Name: "InviteeCheckBoxComponent", select: select });
            this.selectedItem = null;
            this.isCheckedFlag = false;
            this.isClickedFlag = false;
        }
    };
    InviteeCheckBoxComponent.prototype.Destroyed = function () {
        var isDestroyed = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    };
    InviteeCheckBoxComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'InviteeCheckBoxComponent',
            templateUrl: './InviteeCheckBoxComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], InviteeCheckBoxComponent);
    return InviteeCheckBoxComponent;
}(BaseComponent_1.BaseComponent));
exports.InviteeCheckBoxComponent = InviteeCheckBoxComponent;
var InviteeParameterInput = /** @class */ (function () {
    function InviteeParameterInput(fieldName, email, contactid, contactname, isCheck) {
        this.FieldName = fieldName;
        this.Email = email;
        this.ContactId = contactid;
        this.ContactName = contactname;
        this.IsCheck = isCheck;
    }
    return InviteeParameterInput;
}());
//# sourceMappingURL=InviteeCheckBoxComponent.js.map