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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BIReportFolderPM_1 = require("../../../../Infrastructure/EntityPMs/BIReportFolderPM");
var BIReportFolderPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/BIReportFolderPMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var NewBIReportFolderComponent = /** @class */ (function (_super) {
    __extends(NewBIReportFolderComponent, _super);
    function NewBIReportFolderComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.ObjectTableName = "BIReportFolder";
        _this.IsNewQuery = true;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new BIReportFolderPM_1.BIReportFolderPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        ;
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        ;
        _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.myService = new BIReportFolderPMService_1.BIReportFolderPMService();
        _this.SetUIProperties();
        return _this;
    }
    NewBIReportFolderComponent.prototype.SetWindowArgs = function () {
        this.SetUIProperties();
    };
    NewBIReportFolderComponent.prototype.SetUIProperties = function () {
    };
    Object.defineProperty(NewBIReportFolderComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (newValue) {
            if (this.EntityPM.Name != newValue) {
                this.EntityPM.Name = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBIReportFolderComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewBIReportFolderComponent.prototype, "Index", {
        get: function () { return this.EntityPM.Index; },
        set: function (newValue) {
            if (this.EntityPM.Index != newValue) {
                this.EntityPM.Index = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewBIReportFolderComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewBIReportFolderComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.myService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
            });
        }
    };
    NewBIReportFolderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewBIReportFolderComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewBIReportFolderComponent);
    return NewBIReportFolderComponent;
}(BaseComponent_1.BaseComponent));
exports.NewBIReportFolderComponent = NewBIReportFolderComponent;
//# sourceMappingURL=NewBIReportFolderComponent.js.map