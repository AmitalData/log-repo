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
var ActivityPMService_1 = require("../../../../CRM/Services/StandardPMs/ActivityPMService");
var ActivityValidator_1 = require("../../../../CRM/Validators/ActivityValidator");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var NewTaskComponent = /** @class */ (function (_super) {
    __extends(NewTaskComponent, _super);
    function NewTaskComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Activity";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Retries = 0;
        _this.IsAddCustomerVisible = false;
        _this.owner = null;
        _this.ValidationErrorsList = [];
        _this.myActivityPMService = new ActivityPMService_1.ActivityPMService();
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.EntityPM = _this.myActivityPMService.GetNewEntityPM();
        return _this;
    }
    NewTaskComponent.prototype.ngOnInit = function () {
        this.RunComponent();
    };
    NewTaskComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewTaskComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewTaskComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.Run(_this.EntityPM, _this.ObjectTableName, "Activity.AdditionalFields");
        });
    };
    NewTaskComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.EntityPM.ActivityTypeCode = args.TypeCode;
            this.IsAddCustomerVisible = args.IsAddCustomerAllowed;
            this.SetUIProperties();
        }
    };
    NewTaskComponent.prototype.SetUIProperties = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.OpportunityId)) {
            this.UIProperties.SetEnabled("CustomerId", this.ObjectTableName, false);
        }
    };
    Object.defineProperty(NewTaskComponent.prototype, "Subject", {
        //Properties
        get: function () { return this.EntityPM.Subject; },
        set: function (newValue) {
            if (this.EntityPM.Subject != newValue) {
                this.EntityPM.Subject = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaskComponent.prototype, "Description", {
        get: function () { return this.EntityPM.Description; },
        set: function (newValue) {
            if (this.EntityPM.Description != newValue) {
                this.EntityPM.Description = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaskComponent.prototype, "OwnerId", {
        get: function () { return this.EntityPM.OwnerId; },
        set: function (newValue) {
            if (this.EntityPM.OwnerId != newValue) {
                this.EntityPM.OwnerId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaskComponent.prototype, "Owner", {
        get: function () { return this.owner; },
        set: function (newValue) {
            if (this.owner != newValue) {
                this.owner = newValue;
                this.OnOwnerChanged(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaskComponent.prototype, "StartDateTime", {
        get: function () { return this.EntityPM.StartDateTime; },
        set: function (newValue) {
            if (this.EntityPM.StartDateTime != newValue) {
                this.EntityPM.StartDateTime = newValue;
                this.EntityPM.EndDateTime = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaskComponent.prototype, "PriorityCode", {
        get: function () { return this.EntityPM.PriorityCode; },
        set: function (newValue) {
            if (this.EntityPM.PriorityCode != newValue) {
                this.EntityPM.PriorityCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaskComponent.prototype, "DueDate", {
        get: function () { return this.EntityPM.DueDate; },
        set: function (newValue) {
            if (this.EntityPM.DueDate != newValue) {
                this.EntityPM.DueDate = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewTaskComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) {
            if (this.EntityPM.CustomerId != newValue) {
                this.EntityPM.CustomerId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewTaskComponent.prototype.OnOwnerChanged = function (list) {
        if (list == null) {
            this.EntityPM.BusinessUnitId = null;
        }
        else {
            this.EntityPM.BusinessUnitId = list.BusinessUnitId;
        }
    };
    NewTaskComponent.prototype.AddCustomerClicked = function () {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(function (response) {
            _this.entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(function (response1) {
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                logWindow.Title = "New Potential Customer";
                logWindow.Width = 990;
                logWindow.Height = 600;
                logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");
                logWindow.ComponentLoaded.subscribe(function (comp) {
                    logWindow.WindowClosed.subscribe(function (s) {
                        if (s) {
                            _this.CustomerId = comp.EntityPM.Id;
                        }
                    });
                });
            });
        });
    };
    NewTaskComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewTaskComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.BusinessUnitId)) {
            this.EntityPM.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
        }
        var activityValidator = new ActivityValidator_1.ActivityValidator();
        this.ValidationErrorsList = activityValidator.Validate(this.EntityPM);
        if (this.ValidationErrorsList.length == 0) {
            this.myActivityPMService.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.CurrentSession.CloseCurrentWindowEmit('OK');
                }
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewTaskComponent.prototype, "viewContainerRef", void 0);
    NewTaskComponent = __decorate([
        core_1.Component({
            selector: 'NewTaskComponent',
            moduleId: module.id,
            templateUrl: './NewTaskComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewTaskComponent);
    return NewTaskComponent;
}(BaseComponent_1.BaseComponent));
exports.NewTaskComponent = NewTaskComponent;
//# sourceMappingURL=NewTaskComponent.js.map