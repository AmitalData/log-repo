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
var TMEmployeeTimePM_1 = require("../../EntityPMs/TMEmployeeTimePM");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var TimeManagementDomainService_1 = require("../../Services/TimeManagementDomainService");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var TMProjectListService_1 = require("../../Services/StandardLists/TMProjectListService");
var NewLineComponent = /** @class */ (function (_super) {
    __extends(NewLineComponent, _super);
    function NewLineComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "TMEmployeeTime";
        _this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        _this.DateOfWorkDate = new TimeManagementDomainService_1.TimeSheetItemDay();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityId = "";
        _this.IsNew = true;
        _this.dateOfWorkDateFormat = "";
        _this.TimeManagementAPIHelper = new TimeManagementDomainService_1.TimeManagementAPIHelper();
        _this.TimeSheetItem = new TimeManagementDomainService_1.TimeSheetItem();
        _this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        _this.TMProjectListService = new TMProjectListService_1.TMProjectListService();
        _this.EntityPM = new TMEmployeeTimePM_1.TMEmployeeTimePM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.CreateDate = todayDate;
        _this.EntityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.UpdateDate = todayDate;
        _this.EntityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        _this.EntityPM.NeedsProrating = true;
        _this.SetUIProperties();
        return _this;
    }
    NewLineComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            if (!args.IsNew) {
                this.EntityPM = args.EntityPM;
                this.Clone();
                this.EntityId = this.EntityPM.Id;
                this.DateOfWorkDate.Date = this.EntityPM.DateOfWork;
                this.DateOfWorkMinutes = this.EntityPM.TimeInMinutes;
                this.Father = args.Father;
                this.LocationCode = args.LocationCode;
                this.SetUIProperties();
                this.IsNew = false;
            }
            else {
                this.Father = args.Father;
                this.EntityId = args.EntityId;
                this.LocationCode = args.LocationCode;
                this.EntityPM.EmployeeUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.EntityPM.LocationCode = this.LocationCode;
                this.ProjectId = args.ProjectId;
                this.TimeSheetItem.ProjectId_db = args.ProjectId;
                this.WINumber = args.WINumber;
                this.TimeSheetItem.WINumber_db = args.WINumber;
                this.TimeSheetItem.Description_db = args.Description;
                this.DateOfWorkDateFormat = this.ApplyTimeFormat(this.DateOfWorkMinutes);
                this.DateOfWork = args.DateOfWork;
                this.DateOfWorkDate.Date = this.DateOfWork;
                this.SprintId = args.SprintId;
            }
        }
    };
    NewLineComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetEnabled("LocationCode", this.ObjectTableName, true);
        this.UIProperties.SetRequired("DateOfWorkDateFormat", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.DateOfWorkDateFormat));
        this.UIProperties.SetRequired("SprintId", this.ObjectTableName, Tools_1.AppTool.IsNullOrEmpty(this.SprintId));
        this.UIProperties.SetRequired("DateOfWork", this.ObjectTableName, this.DateOfWork == null ? true : false);
    };
    NewLineComponent.prototype.ApplyTimeFormat = function (minutes) {
        var formattedMinutes = "";
        var val = minutes;
        var h = val / 60 | 0, m = val % 60 | 0;
        var result = h + ":" + Tools_1.AppTool.PadLeft("" + m, 2, '0');
        if (result == "0:00") {
            formattedMinutes = "";
        }
        else {
            formattedMinutes = result;
        }
        return formattedMinutes;
    };
    Object.defineProperty(NewLineComponent.prototype, "ProjectId", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.ProjectId;
            }
        },
        set: function (value) {
            if (this.EntityPM.ProjectId != value) {
                this.EntityPM.ProjectId = value;
            }
            this.SetUIProperties();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "Project", {
        get: function () { return this.project; },
        set: function (value) {
            if (this.project != value) {
                this.project = value;
                if (this.project != null && !Tools_1.AppTool.IsNullOrEmpty(this.project.DayOffTypeCode)) {
                    this.LocationCode = "D";
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "EmployeeUserId", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.EmployeeUserId;
            }
        },
        set: function (value) {
            if (this.EntityPM.EmployeeUserId != value) {
                this.EntityPM.EmployeeUserId = value;
            }
            this.SetUIProperties();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "SprintId", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.SprintId;
            }
        },
        set: function (value) {
            if (this.EntityPM.SprintId != value) {
                this.EntityPM.SprintId = value;
            }
            this.SetUIProperties();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "LocationCode", {
        get: function () {
            if (this.EntityPM != null) {
                return this.EntityPM.LocationCode;
            }
        },
        set: function (value) {
            if (this.EntityPM.LocationCode != value) {
                this.EntityPM.LocationCode = value;
                this.setProject(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    NewLineComponent.prototype.setProject = function (value) {
        var _this = this;
        if (this.TMProjectListService == null) {
            this.TMProjectListService = new TMProjectListService_1.TMProjectListService();
        }
        this.TMProjectListService.getSingle(this.ProjectId).subscribe(function (myResult) {
            var project = myResult.Result;
            if (project != null) {
                _this.project = project;
            }
            else {
                _this.project = null;
            }
        });
    };
    Object.defineProperty(NewLineComponent.prototype, "WINumber", {
        get: function () {
            return this.EntityPM.WINumber;
        },
        set: function (value) {
            if (this.EntityPM.WINumber != value) {
                this.EntityPM.WINumber = value;
            }
            this.SetUIProperties();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "Description", {
        get: function () {
            return this.EntityPM.Description;
        },
        set: function (value) {
            if (this.EntityPM.Description != value) {
                this.EntityPM.Description = value;
            }
            this.SetUIProperties();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "DateOfWork", {
        get: function () {
            return this.EntityPM.DateOfWork;
        },
        set: function (value) {
            if (this.EntityPM.DateOfWork != value) {
                this.EntityPM.DateOfWork = value;
                this.DateOfWorkDate.Date = value;
                this.UIProperties.SetRequired("DateOfWork", this.ObjectTableName, this.DateOfWork == null ? true : false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "DateOfWorkMinutes", {
        get: function () {
            if (this.DateOfWorkDate != null) {
                return this.DateOfWorkDate.Minuts;
            }
        },
        set: function (value) {
            if (this.DateOfWorkDate != null) {
                this.DateOfWorkDate.Minuts = value;
                this.DateOfWorkDateFormat = this.ApplyTimeFormat(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewLineComponent.prototype, "DateOfWorkDateFormat", {
        get: function () {
            return this.dateOfWorkDateFormat;
        },
        set: function (value) {
            if (this.dateOfWorkDateFormat != value) {
                this.dateOfWorkDateFormat = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewLineComponent.prototype.CancelButtonClicked = function () {
        if (!this.IsNew) {
            this.RejectChanges();
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    NewLineComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.DateOfWorkMinutes == null || this.DateOfWorkMinutes == 0 || Number.isNaN(this.DateOfWorkMinutes)) {
            errors.push("Please fill the Time");
        }
        if (this.DateOfWork == null) {
            errors.push("Date of work is required");
        }
        if (this.SprintId == null) {
            errors.push("Sprint is required");
        }
        if ((this.Project != null && !Tools_1.AppTool.IsNullOrEmpty(this.Project.DayOffTypeCode) && this.LocationCode != "D") ||
            (this.LocationCode == "D" && this.Project != null && Tools_1.AppTool.IsNullOrEmpty(this.Project.DayOffTypeCode))) {
            errors.push("Project with a Day Off type requires a Day off Location");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.ProjectId) && this.LocationCode == "D") {
            errors.push("Project is required for Day Off location");
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.InsertTMEmployeeTime();
        }
    };
    NewLineComponent.prototype.InsertTMEmployeeTime = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Creating...");
        this.CurrentSession.StartBusyIndicatorSaving();
        this.TimeManagementAPIHelper.Id = SessionLocator_1.SessionLocator.Tenant;
        this.TimeManagementAPIHelper.EmployeeUserId = this.EmployeeUserId;
        this.TimeManagementAPIHelper.LocationCode = this.LocationCode;
        this.TimeManagementAPIHelper.StartDate = this.Father.StartDate;
        this.TimeManagementAPIHelper.EndDate = this.Father.EndDate;
        this.TimeSheetItem.ProjectId = this.ProjectId;
        this.TimeSheetItem.Description = this.Description;
        this.TimeSheetItem.WINumber = this.WINumber;
        this.TimeSheetItem.LocationCode = this.LocationCode;
        this.TimeSheetItem.EmployeeUserId = this.EmployeeUserId;
        this.TimeSheetItem.Days = [];
        this.TimeSheetItem.Days.push(this.DateOfWorkDate);
        this.TimeManagementAPIHelper.Items.push(this.TimeSheetItem);
        var itemPM = new TMEmployeeTimePM_1.TMEmployeeTimePM();
        itemPM.Id = this.EntityId;
        itemPM.ProjectId = this.ProjectId;
        itemPM.Description = this.Description;
        itemPM.WINumber = this.WINumber;
        itemPM.LocationCode = this.LocationCode;
        itemPM.EmployeeUserId = this.EmployeeUserId;
        itemPM.TimeInMinutes = this.DateOfWorkDate.Minuts;
        itemPM.DateOfWork = this.DateOfWorkDate.Date;
        itemPM.SprintId = this.SprintId;
        this.TimeManagementAPIHelper.ItemsPM.push(itemPM);
        if (this.myDomainService == null) {
            this.myDomainService = new TimeManagementDomainService_1.TimeManagementDomainService();
        }
        this.myDomainService.UpdateTimeSheetList(this.TimeManagementAPIHelper).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit('OK');
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
        });
    };
    NewLineComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('ProjectId');
        this.myCloner.AddField('EmployeeUserId');
        this.myCloner.AddField('WINumber');
        this.myCloner.AddField('SprintId');
        this.myCloner.AddField('DateOfWork');
        this.myCloner.AddField('LocationCode');
        this.myCloner.AddEntity(this.EntityPM);
    };
    NewLineComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    NewLineComponent = __decorate([
        core_1.Component({
            selector: 'NewLineComponent',
            moduleId: module.id,
            templateUrl: './NewLineComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewLineComponent);
    return NewLineComponent;
}(BaseComponent_1.BaseComponent));
exports.NewLineComponent = NewLineComponent;
//# sourceMappingURL=NewLineComponent.js.map