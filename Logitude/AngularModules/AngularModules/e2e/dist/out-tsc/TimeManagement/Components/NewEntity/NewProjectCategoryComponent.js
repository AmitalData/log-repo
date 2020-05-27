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
var TMProjectCategoryPM_1 = require("../../EntityPMs/TMProjectCategoryPM");
var TMProjectCategoryPMService_1 = require("../../Services/StandardPMs/TMProjectCategoryPMService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var NewProjectCategoryComponent = /** @class */ (function (_super) {
    __extends(NewProjectCategoryComponent, _super);
    function NewProjectCategoryComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "TMProjectCategory";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.EntityPM = new TMProjectCategoryPM_1.TMProjectCategoryPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        return _this;
    }
    Object.defineProperty(NewProjectCategoryComponent.prototype, "Name", {
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
    Object.defineProperty(NewProjectCategoryComponent.prototype, "Inactive", {
        get: function () {
            return this.EntityPM.Inactive;
        },
        set: function (value) {
            if (this.EntityPM.Inactive != value) {
                this.EntityPM.Inactive = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewProjectCategoryComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewProjectCategoryComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating...");
        var myService = new TMProjectCategoryPMService_1.TMProjectCategoryPMService();
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
    NewProjectCategoryComponent = __decorate([
        core_1.Component({
            selector: 'NewProjectCategoryComponent',
            moduleId: module.id,
            templateUrl: './NewProjectCategoryComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewProjectCategoryComponent);
    return NewProjectCategoryComponent;
}(BaseComponent_1.BaseComponent));
exports.NewProjectCategoryComponent = NewProjectCategoryComponent;
//# sourceMappingURL=NewProjectCategoryComponent.js.map