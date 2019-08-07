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
var BusinessProcessQueuePMService_1 = require("../../../../Infrastructure/Services/StandardPMs/BusinessProcessQueuePMService");
var BusinessProcessQueuePM_1 = require("../../../../Infrastructure/EntityPMs/BusinessProcessQueuePM");
var BusinessProcessQueuePMInitService_1 = require("../../../../Infrastructure/EntityPMInitServices/BusinessProcessQueuePMInitService");
var QueueNewComponent = /** @class */ (function (_super) {
    __extends(QueueNewComponent, _super);
    function QueueNewComponent() {
        var _this = _super.call(this) || this;
        _this.Session = SessionLocator_1.SessionLocator.Tenant;
        _this.DataContext = _this;
        _this.ObjectTableName = "BusinessProcessQueue";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = new BusinessProcessQueuePM_1.BusinessProcessQueuePM();
        BusinessProcessQueuePMInitService_1.BusinessProcessQueuePMInitService.InitValues(_this.EntityPM, true);
        _this.SetUIProperties();
        return _this;
    }
    QueueNewComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("BusinessRoleId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.BusinessRoleId));
        this.UIProperties.SetRequired("ObjectTableId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableId));
    };
    Object.defineProperty(QueueNewComponent.prototype, "Name", {
        get: function () { return this.EntityPM.Name; },
        set: function (value) {
            if (this.EntityPM.Name != value)
                this.EntityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueueNewComponent.prototype, "LocalName", {
        get: function () { return this.EntityPM.LocalName; },
        set: function (value) {
            if (this.EntityPM.LocalName != value)
                this.EntityPM.LocalName = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueueNewComponent.prototype, "BusinessRoleId", {
        get: function () { return this.EntityPM.BusinessRoleId; },
        set: function (value) {
            if (this.EntityPM.BusinessRoleId != value) {
                this.EntityPM.BusinessRoleId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueueNewComponent.prototype, "ObjectTableId", {
        get: function () { return this.EntityPM.ObjectTableId; },
        set: function (value) {
            if (this.EntityPM.ObjectTableId != value) {
                this.EntityPM.ObjectTableId = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueueNewComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) {
            if (this.EntityPM.Notes != value)
                this.EntityPM.Notes = value;
        },
        enumerable: true,
        configurable: true
    });
    QueueNewComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    QueueNewComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableId)) {
            errors.push("Entity field is required");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.BusinessRoleId)) {
            errors.push("Business Role field is required");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myService = new BusinessProcessQueuePMService_1.BusinessProcessQueuePMService();
            myService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    QueueNewComponent = __decorate([
        core_1.Component({
            selector: 'QueueNewComponent',
            moduleId: module.id,
            templateUrl: './QueueNewComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], QueueNewComponent);
    return QueueNewComponent;
}(BaseComponent_1.BaseComponent));
exports.QueueNewComponent = QueueNewComponent;
//# sourceMappingURL=QueueNewComponent.js.map