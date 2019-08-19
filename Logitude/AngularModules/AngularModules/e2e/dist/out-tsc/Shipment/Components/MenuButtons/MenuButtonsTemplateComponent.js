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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityPMService_1 = require("../../../Infrastructure/Services/EntityPMService");
var MenuButtonsTemplateComponent = /** @class */ (function (_super) {
    __extends(MenuButtonsTemplateComponent, _super);
    function MenuButtonsTemplateComponent(entityPMService) {
        var _this = _super.call(this) || this;
        _this.entityPMService = entityPMService;
        _this.DataContext = _this;
        _this.IsNotesStackPanelVisible = false;
        _this.ValidationErrorsList = [];
        _this.ValidationWarningsList = [];
        _this.NotesHeader = "Notes";
        _this.EventNotes = "";
        _this.IsReasonStackPanel = false;
        _this.IsConvertShipmentType = false;
        _this.ReopenDone = new core_1.EventEmitter();
        _this.SaveClicked = new core_1.EventEmitter();
        _this.SaveCompleted = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EnabledOkButton = true;
        return _this;
    }
    MenuButtonsTemplateComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTableName = args.ObjectTableName;
        this.IsNotesStackPanelVisible = args.IsNotesStackPanelVisible;
        this.NotesHeader = args.NotesHeader;
        this.EventNotes = args.EventNote;
        this.IsReasonStackPanel = args.IsReasonStackPanel;
        this.ActionStepsStateList = args.ActionStepsStateList;
        this.IsConvertShipmentType = args.IsConvertShipmentType;
        if (args.EnabledOkButton != null) {
            this.EnabledOkButton = args.EnabledOkButton;
        }
        this.ValidationErrorsList = args.ValidationErrorsList;
        this.ValidationWarningsList = args.ValidationWarningsList;
    };
    MenuButtonsTemplateComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    MenuButtonsTemplateComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.ReopenDone.emit(this.EventNotes);
            this.CurrentSession.CloseCurrentWindowEmit("confirm");
        }
    };
    MenuButtonsTemplateComponent.prototype.SaveChanges = function () {
        if (this.EntityPM.IsDirty) {
            this.SaveEntityChanges(false);
        }
        else {
            this.SaveCompleted.emit(true);
        }
    };
    MenuButtonsTemplateComponent.prototype.SaveChangesAndClose = function () {
        this.SaveEntityChanges(true);
    };
    MenuButtonsTemplateComponent.prototype.SaveEntityChanges = function (isClosing) {
        var _this = this;
        if (this.EntityPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.entityPMService.update(this.ObjectTableName, this.EntityPM).then(function (res) {
                res.subscribe(function (response) {
                    _this.CurrentSession.StopBusyIndicator();
                    var mm = response;
                    if (!mm.HasError) {
                        _this.ValidationErrorsList = [];
                        if (isClosing) {
                            _this.DestroyEditControl();
                        }
                        else {
                            _this.SaveCompleted.emit(true);
                        }
                    }
                    else {
                        _this.ValidationErrorsList = mm.ErrorsArray;
                    }
                }, function (error) {
                    console.log("Error===========>", error);
                    _this.CurrentSession.StopBusyIndicator();
                });
            });
        }
    };
    MenuButtonsTemplateComponent.prototype.DestroyEditControl = function () {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], MenuButtonsTemplateComponent.prototype, "ReopenDone", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], MenuButtonsTemplateComponent.prototype, "SaveClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], MenuButtonsTemplateComponent.prototype, "SaveCompleted", void 0);
    MenuButtonsTemplateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MenuButtonsTemplateComponent.html',
        }),
        __metadata("design:paramtypes", [EntityPMService_1.EntityPMService])
    ], MenuButtonsTemplateComponent);
    return MenuButtonsTemplateComponent;
}(BaseComponent_1.BaseComponent));
exports.MenuButtonsTemplateComponent = MenuButtonsTemplateComponent;
var MenuButtonsTemplateArgs = /** @class */ (function () {
    function MenuButtonsTemplateArgs() {
        this.IsNotesStackPanelVisible = false;
        this.IsReasonStackPanel = false;
        this.NotesHeader = "Notes";
        this.EventNote = "";
        this.EnabledOkButton = true;
        this.ValidationWarningsList = [];
        this.ValidationErrorsList = [];
        this.IsConvertShipmentType = false;
    }
    return MenuButtonsTemplateArgs;
}());
exports.MenuButtonsTemplateArgs = MenuButtonsTemplateArgs;
//# sourceMappingURL=MenuButtonsTemplateComponent.js.map