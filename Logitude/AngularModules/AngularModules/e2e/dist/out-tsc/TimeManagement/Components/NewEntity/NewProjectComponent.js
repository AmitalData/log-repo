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
var TMProjectPM_1 = require("../../EntityPMs/TMProjectPM");
var TMProjectPMService_1 = require("../../Services/StandardPMs/TMProjectPMService");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var NewProjectComponent = /** @class */ (function (_super) {
    __extends(NewProjectComponent, _super);
    function NewProjectComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "TMProject";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.EntityPM = new TMProjectPM_1.TMProjectPM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.CreateDate = todayDate;
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.UpdateDate = todayDate;
        _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        return _this;
    }
    NewProjectComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM.CustomerId = args.EntityArgs.CustomerId;
        this.EntityPM.CustomerName = args.EntityArgs.CustomerName;
        this.EntityPM.OwnerId = args.EntityArgs.OwnerId;
        this.EntityPM.OwnerName = args.EntityArgs.OwnerName;
        this.EntityPM.ProjectNumber = args.EntityArgs.ProjectNumber;
        this.EntityPM.Id = args.EntityArgs.Id;
        this.EntityPM.IsInnerProject = true;
        this.EntityPM.CategoryId = args.EntityArgs.CategoryId;
    };
    Object.defineProperty(NewProjectComponent.prototype, "CustomerId", {
        get: function () {
            return this.EntityPM.CustomerId;
        },
        set: function (value) {
            if (this.EntityPM.CustomerId != value) {
                this.EntityPM.CustomerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "DayOffTypeCode", {
        get: function () {
            return this.EntityPM.DayOffTypeCode;
        },
        set: function (value) {
            if (this.EntityPM.DayOffTypeCode != value) {
                this.EntityPM.DayOffTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "BudgetId", {
        get: function () {
            return this.EntityPM.BudgetId;
        },
        set: function (value) {
            if (this.EntityPM.BudgetId != value) {
                this.EntityPM.BudgetId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "IsProrated", {
        get: function () {
            return this.EntityPM.IsProrated;
        },
        set: function (value) {
            if (this.EntityPM.IsProrated != value) {
                this.EntityPM.IsProrated = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "ExcludeFromProrating", {
        get: function () {
            return this.EntityPM.ExcludeFromProrating;
        },
        set: function (value) {
            if (this.EntityPM.ExcludeFromProrating != value) {
                this.EntityPM.ExcludeFromProrating = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "CategoryId", {
        get: function () {
            return this.EntityPM.CategoryId;
        },
        set: function (value) {
            if (this.EntityPM.CategoryId != value) {
                this.EntityPM.CategoryId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "OwnerId", {
        get: function () {
            return this.EntityPM.OwnerId;
        },
        set: function (value) {
            if (this.EntityPM.OwnerId != value) {
                this.EntityPM.OwnerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "ExternalProjectNumber", {
        get: function () {
            return this.EntityPM.ExternalProjectNumber;
        },
        set: function (value) {
            if (this.EntityPM.ExternalProjectNumber != value) {
                this.EntityPM.ExternalProjectNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewProjectComponent.prototype, "Name", {
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
    Object.defineProperty(NewProjectComponent.prototype, "Description", {
        get: function () {
            return this.EntityPM.Description;
        },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    // Commands
    NewProjectComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewProjectComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating...");
        var myService = new TMProjectPMService_1.TMProjectPMService();
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
    NewProjectComponent = __decorate([
        core_1.Component({
            selector: 'NewProjectComponent',
            moduleId: module.id,
            templateUrl: './NewProjectComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewProjectComponent);
    return NewProjectComponent;
}(BaseComponent_1.BaseComponent));
exports.NewProjectComponent = NewProjectComponent;
//# sourceMappingURL=NewProjectComponent.js.map