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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TraceEventPMService_1 = require("../../../Infrastructure/Services/StandardPMs/TraceEventPMService");
var WebFreightDomainService_1 = require("../../../Infrastructure/Services/WebFreightDomainService");
var AddEditEventComponent = /** @class */ (function () {
    function AddEditEventComponent() {
        this.ValidationErrorsList = [];
        this.EntityPM = null;
        this.ObjectTableName = "TraceEvent";
        this.myPMService = null;
        this.myDomainService = null;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    AddEditEventComponent.prototype.SetWindowArgs = function (args) {
        this.DataContext = args;
        this.EntityPM = args.EntityPM;
        this.EntityPM.CloneMe();
    };
    AddEditEventComponent.prototype.CancelButtonClicked = function () {
        this.EntityPM.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditEventComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.DataContext.IsNewEntity) {
                if (this.myDomainService == null) {
                    this.myDomainService = new WebFreightDomainService_1.WebFreightDomainService();
                }
                this.myDomainService.InsertTraceEvent(this.EntityPM.EntityId, this.EntityPM.ObjectTableId, this.EntityPM.EventTypeId, this.EntityPM.EventDateTime, this.EntityPM.Notes).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            var myResult = myResponse.Result;
                            if (myResult.StatusChanged) {
                                if (_this.CurrentSession.CurrentEditComponent) {
                                    if (_this.CurrentSession.CurrentEditComponent.EntityId == myResult.EntityId) {
                                        switch (_this.CurrentSession.CurrentEditComponent.ObjectTableName) {
                                            case "Shipment":
                                            case "Master": {
                                                var isEntityDirty = _this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.StatusId = myResult.StatusId;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.StatusName = myResult.StatusName;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.StatusDate = myResult.StatusDate;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.StatusLocation = myResult.StatusLocation;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.LastStatusLogDate = myResult.LastStatusLogDate;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventId = myResult.LastSharedEventId;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventLocation = myResult.LastSharedEventLocation;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventNotes = myResult.LastSharedEventNotes;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventDate = myResult.LastSharedEventDate;
                                                _this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = isEntityDirty;
                                                _this.CurrentSession.CurrentEditComponent.BuildHeaderScreen();
                                                _this.CurrentSession.CurrentEditComponent.LoadCompleted.emit(true);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            _this.CurrentSession.CloseCurrentWindow();
                            _this.DataContext.father.LoadData();
                        }
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                if (this.myPMService == null) {
                    this.myPMService = new TraceEventPMService_1.TraceEventPMService();
                }
                this.myPMService.update(this.EntityPM).subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        else {
                            _this.CurrentSession.CloseCurrentWindow();
                            _this.DataContext.father.LoadData();
                        }
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        }
    };
    AddEditEventComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditEventComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditEventComponent);
    return AddEditEventComponent;
}());
exports.AddEditEventComponent = AddEditEventComponent;
//# sourceMappingURL=AddEditEventComponent.js.map