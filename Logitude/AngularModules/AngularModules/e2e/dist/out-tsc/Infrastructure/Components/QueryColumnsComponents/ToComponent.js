"use strict";
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
/// <reference path="../../../controls/pipes/idgeneratorpipe.ts" />
var core_1 = require("@angular/core");
var InfraSettings_1 = require("../../../Infrastructure/Utilities/InfraSettings");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var IdGeneratorPipe_1 = require("../../../Controls/Pipes/IdGeneratorPipe");
var ToComponent = /** @class */ (function () {
    function ToComponent(cd, _entityListService) {
        this.cd = cd;
        this._entityListService = _entityListService;
        this.AddButtonEnabled = true;
        this.IsChecked = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.IdGeneratorPipe = new IdGeneratorPipe_1.IdGeneratorPipe();
    }
    ToComponent.prototype.setVariables = function (rowData, fieldName) {
        var _this = this;
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.Key = this.IdGeneratorPipe.transform("SendMessage" + fieldName + "CheckBox");
        if (this.rowData.Email) {
            if (fieldName == "To" && window.ToEmailLists) {
                var item = window.ToEmailLists.filter(function (d) { return d.toLowerCase() == _this.rowData.Email.toLowerCase(); })[0];
                if (item) {
                    this.IsChecked = true;
                }
                else
                    this.IsChecked = false;
            }
            else if (fieldName == "Cc" && window.CcEmailLists) {
                var item = window.CcEmailLists.filter(function (d) { return d.toLowerCase() == _this.rowData.Email.toLowerCase(); })[0];
                if (item) {
                    this.IsChecked = true;
                }
                else
                    this.IsChecked = false;
            }
            else if (fieldName == "Bcc" && window.BccEmailLists) {
                var item = window.BccEmailLists.filter(function (d) { return d.toLowerCase() == _this.rowData.Email.toLowerCase(); })[0];
                if (item) {
                    this.IsChecked = true;
                }
                else
                    this.IsChecked = false;
            }
            this.Destroyed();
        }
        if (this.rowData.Id) {
            if (fieldName == "SelectedUser" && window.ToEmailLists) {
                var item = window.ToEmailLists.filter(function (d) { return d.toLowerCase() == _this.rowData.Id.toLowerCase(); })[0];
                if (item) {
                    this.IsChecked = true;
                }
                else
                    this.IsChecked = false;
            }
            this.Destroyed();
        }
    };
    ToComponent.prototype.ngOnInit = function () {
    };
    ToComponent.prototype.Checkclick = function (item) {
        //this.IsChecked = !this.IsChecked;
        if (item.Email) {
            var select = new ParameterInput(this.fieldName, item.Email, true, item.Id);
            this.Destroyed();
            this.CurrentSession.SessionEvent.emit(select);
        }
    };
    ToComponent.prototype.Destroyed = function () {
        var isDestroyed = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    };
    ToComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ToComponent',
            templateUrl: './ToComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, EntityListService_1.EntityListService])
    ], ToComponent);
    return ToComponent;
}());
exports.ToComponent = ToComponent;
var ParameterInput = /** @class */ (function () {
    function ParameterInput(fieldName, email, isCheck, userId) {
        if (userId === void 0) { userId = null; }
        this.FieldName = fieldName;
        this.Email = email;
        this.IsCheck = isCheck;
        this.UserId = userId;
    }
    return ParameterInput;
}());
//# sourceMappingURL=ToComponent.js.map