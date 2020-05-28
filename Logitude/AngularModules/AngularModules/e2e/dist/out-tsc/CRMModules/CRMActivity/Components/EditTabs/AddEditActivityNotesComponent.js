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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AddEditActivityNotesComponent = /** @class */ (function () {
    function AddEditActivityNotesComponent() {
        this.activityPM = null;
        this.entityPM = null;
        this.ObjectTableName = "ActivityNote";
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditActivityNotesComponent.prototype.SetDataContext = function (args) {
        this.activityPM = args.activityPM;
        this.entityPM = args.entityPM;
        this.isNew = args.isNew;
        this.DataContext = args;
        this.Clone();
    };
    AddEditActivityNotesComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditActivityNotesComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.entityPM, this.ObjectTableName, errors);
        if (errors.length == 0) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.entityPM.Notes)) {
                errors.push("Notes is required");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.isNew) {
                if (this.DataContext.activityPM.ActivityNotes.indexOf(this.DataContext.entityPM) == -1) {
                    this.DataContext.activityPM.AddActivityNote(this.DataContext.entityPM);
                }
                this.isNew = false;
            }
            else if (this.DataContext.isDataEdited) {
                var maxDate = new Date();
                var nowData = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                if (this.DataContext.activityPM.ActivityNotes.length > 0) {
                    this.DataContext.activityPM.ActivityNotes.forEach(function (item) {
                        if (item.UpdateDate.valueOf > maxDate.valueOf) {
                            maxDate = item.UpdateDate;
                        }
                    });
                }
                if (maxDate == null) {
                    maxDate = nowData;
                }
                else {
                    maxDate.setUTCHours(maxDate.getUTCHours() + 1);
                }
                this.DataContext.entityPM.UpdateDate = maxDate;
            }
            this.DataContext.father.BuildNotes();
        }
        this.CurrentSession.CloseCurrentWindowEmit("OK");
    };
    AddEditActivityNotesComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.entityPM);
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('PostToFollowers');
        this.myCloner.AddField('CreateDate');
        this.myCloner.AddField('CreatedByUserName');
        this.myCloner.AddField('UpdateDate');
        this.myCloner.AddField('UpdatedByUserName');
        this.myCloner.AddEntity(this.entityPM);
    };
    AddEditActivityNotesComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditActivityNotesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditActivityNotesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditActivityNotesComponent);
    return AddEditActivityNotesComponent;
}());
exports.AddEditActivityNotesComponent = AddEditActivityNotesComponent;
//# sourceMappingURL=AddEditActivityNotesComponent.js.map