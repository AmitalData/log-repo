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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SchedulerDetails_1 = require("../../../../Infrastructure/DataContracts/SchedulerDetails");
var SchedulerExtendedPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/SchedulerExtendedPMService");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var core_1 = require("@angular/core");
var AddEditTaskSchedulerComponent = /** @class */ (function () {
    function AddEditTaskSchedulerComponent() {
        this.ObjectTableName = "TasksScheduler";
        this.GeneralAreaHeight = "200px";
        this.IsEnableSaveButton = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.GeneralTemplateComponent = null;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.StartTimeTabTitle = "One Time";
        this.schedulerExtendedPMService = new SchedulerExtendedPMService_1.SchedulerExtendedPMService();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "UPDATE"))
            this.IsEnableSaveButton = true;
    }
    AddEditTaskSchedulerComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.BuildSchedulerDetailsData();
        this.Clone();
        this.SetTigger(this.DataContext.TriggerType);
        this.RunComponent();
    };
    AddEditTaskSchedulerComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.viewContainerRef) {
            var componentName = (this.DataContext.Type == "FTP" || this.DataContext.Type == "SFTP") ? "FTBSchedulerTemplateComponent" : "TaskSchedulerTemplateComponent";
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/SchedulerTemplates/" + componentName, this.viewContainerRef)
                .then(function (cmpRef) {
                _this.GeneralTemplateComponent = cmpRef.instance;
                _this.GeneralTemplateComponent.LoadComponent(_this.DataContext);
                _this.isLoaderReady = true;
            });
        }
        else {
            this.RunComponentTimer();
        }
    };
    AddEditTaskSchedulerComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AddEditTaskSchedulerComponent.prototype.BuildSchedulerDetailsData = function () {
        if (this.DataContext.Type == "FTP" || this.DataContext.Type == "SFTP") {
            this.GeneralAreaHeight = "310px";
            if (this.EntityPM.SchedulerDetailsData) {
                this.SetSchedulerDetailsData(this.EntityPM.SchedulerDetailsData);
            }
            else if (this.EntityPM.Id) {
                this.LoadSchedulerDetailsData();
            }
            else {
                var schedulerDetailsData = new SchedulerDetails_1.SchedulerDetails();
                schedulerDetailsData.FTPDetails = new SchedulerDetails_1.FTPSchedulerDetails();
                schedulerDetailsData.FTPDetails.IsSFTP = (this.EntityPM.Type == "SFTP" ? true : false);
                this.SetSchedulerDetailsData(schedulerDetailsData);
            }
        }
    };
    AddEditTaskSchedulerComponent.prototype.LoadSchedulerDetailsData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Loading...");
        this.schedulerExtendedPMService.GetSchedulerDetailsById(this.EntityPM.Id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.SetSchedulerDetailsData(myResponse.Result);
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    AddEditTaskSchedulerComponent.prototype.SetSchedulerDetailsData = function (schedulerDetailsData) {
        this.DataContext.SetSchedulerDetailsData(schedulerDetailsData);
    };
    Object.defineProperty(AddEditTaskSchedulerComponent.prototype, "IsDaily", {
        get: function () { return this.isDaily; },
        set: function (newValue) {
            if (this.isDaily != newValue) {
                this.isDaily = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTaskSchedulerComponent.prototype, "IsWeekly", {
        get: function () { return this.isWeekly; },
        set: function (newValue) {
            if (this.isWeekly != newValue) {
                this.isWeekly = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddEditTaskSchedulerComponent.prototype, "IsMonthly", {
        get: function () { return this.isMonthly; },
        set: function (newValue) {
            if (this.isMonthly != newValue) {
                this.isMonthly = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    AddEditTaskSchedulerComponent.prototype.SetTigger = function (triggerType) {
        switch (triggerType) {
            //case "O":
            //    {
            //        this.IsOneTime = true;
            //        this.IsDaily = false; 
            //        this.IsWeekly = false;
            //        this.IsMonthly = false;
            //        this.DataContext.TriggerType = "O";             
            //        this.StartTimeTabTitle = "One Time";
            //        break;
            //    }
            case "D":
                {
                    //this.IsOneTime = false;
                    this.IsDaily = true;
                    this.IsWeekly = false;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "D";
                    this.StartTimeTabTitle = "Daily";
                    break;
                }
            case "W":
                {
                    //this.IsOneTime = false;
                    this.IsDaily = false;
                    this.IsWeekly = true;
                    this.IsMonthly = false;
                    this.DataContext.TriggerType = "W";
                    this.StartTimeTabTitle = "Weekly";
                    break;
                }
            case "M":
                {
                    //this.IsOneTime = false;
                    this.IsDaily = false;
                    this.IsWeekly = false;
                    this.IsMonthly = true;
                    this.DataContext.TriggerType = "M";
                    this.StartTimeTabTitle = "Monthly";
                    break;
                }
            default: {
                //this.IsOneTime = true;
                this.IsDaily = true;
                this.IsWeekly = false;
                this.IsMonthly = false;
                //this.DataContext.TriggerType = "O";
                //this.StartTimeTabTitle = "One Time";
                this.DataContext.TriggerType = "D";
                this.StartTimeTabTitle = "Daily";
                break;
            }
        }
    };
    AddEditTaskSchedulerComponent.prototype.ValidateHost = function () {
        var isValid = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Host)) {
            var ipformat = /^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            if (this.DataContext.Host.match(ipformat)) {
                isValid = true;
            }
        }
        return isValid;
    };
    AddEditTaskSchedulerComponent.prototype.OKButtonClicked = function () {
        var _this = this;
        var errors = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.DataContext.Type == "FTP" || this.DataContext.Type == "SFTP") {
            this.DataContext.ServiceClassName = (this.DataContext.Type == "FTP" ? "FTPSchedulerTask" : "SFTPSchedulerTask");
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.UserName))
                errors.push(msg.replace("%FieldName", "UserName"));
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Password))
                errors.push(msg.replace("%FieldName", "Password"));
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Subject))
                errors.push(msg.replace("%FieldName", "Subject"));
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.From))
                errors.push(msg.replace("%FieldName", "From"));
            if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Host))
                errors.push(msg.replace("%FieldName", "Host"));
            else {
                var isValid = this.ValidateHost();
                if (!isValid)
                    errors.push("Invalid Host");
            }
        }
        Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.Name)) {
            errors.push(msg.replace("%FieldName", "Name"));
        }
        if (this.DataContext.RepeatInMinutes < 5) {
            errors.push("The lowest value you can add in Repeat in Minutes field is 5");
        }
        //if (AppTool.IsNullOrEmpty(this.DataContext.Description)) {
        //    errors.push(msg.replace("%FieldName", "Description"));
        //}
        if (this.DataContext.StartDateTime == null) {
            errors.push(msg.replace("%FieldName", "Start Date Time"));
            console.log("error");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.ServiceClassName)) {
            errors.push(msg.replace("%FieldName", "Service Class Name"));
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.DataContext.TriggerType)) {
            errors.push(msg.replace("%FieldName", "Trigger Type"));
        }
        else {
            switch (this.DataContext.TriggerType) {
                case "W":
                    {
                        if (!this.DataContext.Satarday && !this.DataContext.Sunday && !this.DataContext.Monday && !this.DataContext.Tuesday
                            && !this.DataContext.Wednesday && !this.DataContext.Thursday && !this.DataContext.Friday) {
                            errors.push(msg.replace("%FieldName", "Day"));
                        }
                        break;
                    }
                case "M":
                    {
                        if (this.DataContext.MonthlyDay == 0) {
                            errors.push(msg.replace("%FieldName", "Day of a month"));
                        }
                        break;
                    }
            }
        }
        if (this.EntityPM.Status == "In progress") {
            errors.push("The task is in progress. You are not allowed to edit it"); // the start time field
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.EntityPM.Type == "FTP" || this.EntityPM.Type == "SFTP")
                this.EntityPM.SchedulerDetailsData = this.DataContext.SchedulerDetailsData;
            this.CurrentSession.StartBusyIndicatorSaving();
            if (this.DataContext.IsNew) {
                this.schedulerExtendedPMService.insert(this.EntityPM).subscribe(function (myResult) {
                    var myResponse = myResult;
                    if (!myResponse.HasError) {
                        _this.CurrentSession.CloseCurrentWindow();
                        _this.EntityPM = myResponse.Result;
                        _this.EntityPM.IsDirty = false;
                        _this.DataContext.fatherComponent.RefreshTasksSchedular(_this.EntityPM);
                    }
                    else {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                if (this.EntityPM.IsDirty) {
                    this.EntityPM.UpdatedBy = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                    this.schedulerExtendedPMService.update(this.EntityPM).subscribe(function (myResult) {
                        var myResponse = myResult;
                        if (!myResponse.HasError) {
                            _this.EntityPM = myResponse.Result;
                            _this.EntityPM.IsDirty = false;
                            _this.CurrentSession.CloseCurrentWindow();
                            _this.DataContext.fatherComponent.RefreshTasksSchedular(_this.EntityPM);
                        }
                        else {
                            _this.ValidationErrorsList = myResponse.ErrorsArray;
                        }
                        _this.CurrentSession.StopBusyIndicator();
                    });
                }
                else {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CloseCurrentWindow();
                }
            }
        }
    };
    AddEditTaskSchedulerComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditTaskSchedulerComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('ServiceClassName');
        this.myCloner.AddField('Description');
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('StartDateTime');
        this.myCloner.AddField('RepeatInMinutes');
        this.myCloner.AddField('MonthlyDay');
        this.myCloner.AddField('Satarday');
        this.myCloner.AddField('Sunday');
        this.myCloner.AddField('Monday');
        this.myCloner.AddField('Tuesday');
        this.myCloner.AddField('Wednesday');
        this.myCloner.AddField('Thursday');
        this.myCloner.AddField('Friday');
        this.myCloner.AddField('TriggerType');
        this.myCloner.AddField('Host');
        this.myCloner.AddField('UserName');
        this.myCloner.AddField('Folder');
        this.myCloner.AddField('Password');
        this.myCloner.AddField('From');
        this.myCloner.AddField('Subject');
        this.myCloner.AddField('Prefix');
        this.myCloner.AddField('Extension');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditTaskSchedulerComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    __decorate([
        core_1.ViewChild('GeneralSectionLocation', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AddEditTaskSchedulerComponent.prototype, "viewContainerRef", void 0);
    AddEditTaskSchedulerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditTaskSchedulerComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditTaskSchedulerComponent);
    return AddEditTaskSchedulerComponent;
}());
exports.AddEditTaskSchedulerComponent = AddEditTaskSchedulerComponent;
//# sourceMappingURL=AddEditTaskSchedulerComponent.js.map