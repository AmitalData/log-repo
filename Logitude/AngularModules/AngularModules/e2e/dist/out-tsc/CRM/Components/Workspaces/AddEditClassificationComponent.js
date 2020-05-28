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
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../Infrastructure/Tools");
var TicketClassificationPMService_1 = require("../../Services/StandardPMs/TicketClassificationPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CachedDataManager_1 = require("../../../Infrastructure/Utilities/CachedDataManager");
var AddEditClassificationComponent = /** @class */ (function () {
    function AddEditClassificationComponent() {
        this.ObjectTableName = "TicketClassification";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditClassificationComponent.prototype.SetDataContext = function (args) {
        this.entityPM = args.entityPM;
        this.IsNew = args.isNew;
        this.DataContext = args;
        this.Clone();
    };
    AddEditClassificationComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditClassificationComponent.prototype.OkButtonClicked = function () {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Name) && this.DataContext.Name == "General") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.EmployeeGroupId))
                errors.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("TicketClassification.F.EmployeeGroupId")));
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.IsNew) {
                this.InsertClassification();
            }
            else {
                this.UpdateClassification();
            }
        }
    };
    AddEditClassificationComponent.prototype.InsertClassification = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new TicketClassificationPMService_1.TicketClassificationPMService();
        service.insert(this.entityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            CachedDataManager_1.CachedDataManager.RefreshTableData("TicketClassification", true);
        });
    };
    AddEditClassificationComponent.prototype.UpdateClassification = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new TicketClassificationPMService_1.TicketClassificationPMService();
        service.update(this.entityPM).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            CachedDataManager_1.CachedDataManager.RefreshTableData("TicketClassification", true);
        });
    };
    AddEditClassificationComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.entityPM);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('EmployeeGroupId');
        this.myCloner.AddField('DefaultSeverityId');
        this.myCloner.AddField('ManagerUserId');
        this.myCloner.AddField('EscalationNotify');
        this.myCloner.AddField('Inactive');
        this.myCloner.AddEntity(this.entityPM);
    };
    AddEditClassificationComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditClassificationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditClassificationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditClassificationComponent);
    return AddEditClassificationComponent;
}());
exports.AddEditClassificationComponent = AddEditClassificationComponent;
//# sourceMappingURL=AddEditClassificationComponent.js.map