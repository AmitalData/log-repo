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
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var TicketStageListService_1 = require("../../Services/StandardLists/TicketStageListService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var TicketValidator_1 = require("../../Validators/TicketValidator");
var Args_1 = require("../../Args");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var TicketHelperComponent = /** @class */ (function () {
    function TicketHelperComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.StagesList = [];
        this.ObjectTableName = "Ticket";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        // Send Button
        this.stageButtonContent = new Args_1.TicketStagesArgs();
        this.stageButtonCode = "";
        this.isOpened = false;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            this.Listen();
            this.BuildComponent();
        }
    }
    TicketHelperComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (!this.SaveCompletedEvent) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                        _this.BuildComponent();
                    }
                });
            }
            if (!this.LoadCompletedEvent) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                    if (isLoadSuccess) {
                        _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    };
    TicketHelperComponent.prototype.BuildComponent = function () {
        this.BuildStages();
    };
    Object.defineProperty(TicketHelperComponent.prototype, "StageButtonContent", {
        get: function () {
            return this.stageButtonContent;
        },
        set: function (value) {
            this.stageButtonContent = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketHelperComponent.prototype, "StageButtonCode", {
        get: function () {
            return this.stageButtonCode;
        },
        set: function (value) {
            this.stageButtonCode = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketHelperComponent.prototype, "IsOpened", {
        get: function () {
            return this.isOpened;
        },
        set: function (value) {
            this.isOpened = value;
        },
        enumerable: true,
        configurable: true
    });
    TicketHelperComponent.prototype.BuildStages = function () {
        this.StagesList = [];
        this.GetTicketStageMethod();
    };
    // Ticket Stages
    TicketHelperComponent.prototype.GetTicketStageMethod = function () {
        var _this = this;
        var myService = new TicketStageListService_1.TicketStageListService();
        myService.getAll().subscribe(function (resp) {
            if (!resp.HasError) {
                var stages = resp.Result;
                stages.forEach(function (item) {
                    var IsEnabled = true;
                    if ((!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketClosure") && (item.Code == "RE" || item.Code == "CS")) ||
                        (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsClosed") && item.Code == "CS") ||
                        (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsResolved") && item.Code == "RE") ||
                        (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "SaveAsOpen") && item.Code == "OP")) {
                        IsEnabled = false;
                    }
                    var stage = new Args_1.TicketStagesArgs();
                    stage.Code = item.Code;
                    stage.Name = "Save as " + item.Name;
                    stage.IsEnabled = IsEnabled;
                    stage.ItemOpacity = IsEnabled ? 1 : 0.5;
                    _this.StagesList.push(stage);
                    if (_this.EntityPM.StageCode == stage.Code) {
                        _this.StageButtonContent = stage;
                    }
                });
                _this.StageButtonContent.Name = "Save as " + _this.EntityPM.StageName;
                _this.StageButtonCode = _this.EntityPM.StageCode;
            }
        });
    };
    TicketHelperComponent.prototype.OptionSelectionChanged = function (option) {
        if (option != null) {
            this.StageButtonContent = option;
            this.StageButtonCode = option.Code;
            this.IsOpened = false;
            this.ChangeStageCommand();
        }
    };
    TicketHelperComponent.prototype.ChangeStageCommand = function () {
        var validator = new TicketValidator_1.TicketValidator();
        var isValid = true;
        var errors = validator.ValidateCurrenctEntity(this.EntityPM);
        if (errors != null && errors.length > 0) {
            isValid = false;
            if (this.CurrentSession.CurrentEditComponent != null) {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            }
        }
        if (isValid) {
            this.CheckTicketOwnerPermission();
        }
    };
    TicketHelperComponent.prototype.CheckTicketOwnerPermission = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetTicketOwnerPermission(this.EntityPM.OwnerId, this.EntityPM.OwnerName).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.CompleteWorking();
            }
            else {
                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    TicketHelperComponent.prototype.CompleteWorking = function () {
        if (this.StageButtonCode != "CS") {
            this.EntityPM.IsClosed = false;
        }
        // Resolve Case or closed
        if (this.StageButtonCode == "RE" || this.StageButtonCode == "CS") {
            var currentDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            if (this.StageButtonCode == "RE") {
                if (this.EntityPM.FirstResolveDate == null) {
                    this.EntityPM.FirstResolveDate = currentDateTime;
                }
                if (this.EntityPM.FirstResponseTime == null) {
                    this.EntityPM.FirstResponseTime = currentDateTime;
                }
                this.EntityPM.FullResolvedTime = currentDateTime;
            }
            if (this.StageButtonCode == "CS") {
                if (this.EntityPM.FirstResolveDate == null) {
                    this.EntityPM.FirstResolveDate = currentDateTime;
                }
                if (this.EntityPM.FullResolvedTime == null) {
                    this.EntityPM.FullResolvedTime = currentDateTime;
                }
                if (this.EntityPM.FirstResponseTime == null) {
                    this.EntityPM.FirstResponseTime = currentDateTime;
                }
                if (this.EntityPM.FirstCloseDate == null) {
                    this.EntityPM.FirstCloseDate = currentDateTime;
                }
                this.EntityPM.LastCloseDate = currentDateTime;
            }
            // Validate entity 
            this.ClosuerWindow();
        }
        else {
            this.SaveChanges();
        }
    };
    TicketHelperComponent.prototype.ClosuerWindow = function () {
        var _this = this;
        var windowTitle = "Ticket Closure";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 350;
        logWindow.Title = windowTitle;
        var args = new Args_1.TicketClosureArgs();
        args.Ticket = this.EntityPM;
        args.StageCode = this.StageButtonCode;
        logWindow.WindowArgs = args;
        logWindow.Show('./CRMModules/CRMTickets/Components/EditTabs/Others/TicketClosureComponent');
        logWindow.ComponentLoaded.subscribe(function (comp) {
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    if (comp.IsOkClosed == true) {
                        _this.SaveChanges();
                    }
                }
            });
        });
    };
    TicketHelperComponent.prototype.SaveChanges = function () {
        var _this = this;
        //Update Ticket Stage 
        var myService = new TicketStageListService_1.TicketStageListService();
        myService.getAllFromCache().subscribe(function (resp) {
            if (!resp.HasError) {
                var stages = resp.Result;
                var selectedStage = stages.filter(function (d) { return d.Code == _this.StageButtonCode && d.Tenant == _this.EntityPM.Tenant; })[0];
                if (selectedStage != null) {
                    _this.EntityPM.StageId = selectedStage.Id;
                    _this.EntityPM.StageCode = selectedStage.Code;
                    _this.EntityPM.StageName = selectedStage.Name;
                    if (_this.entityArgs.EditComponent != null) {
                        _this.entityArgs.EditComponent.SaveChanges();
                    }
                }
            }
        });
    };
    TicketHelperComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: "TicketHelperComponent.html",
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TicketHelperComponent);
    return TicketHelperComponent;
}());
exports.TicketHelperComponent = TicketHelperComponent;
//# sourceMappingURL=TicketHelperComponent.js.map