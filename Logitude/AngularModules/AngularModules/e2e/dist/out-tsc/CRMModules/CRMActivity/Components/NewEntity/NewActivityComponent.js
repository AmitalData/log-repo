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
var ActivityPM_1 = require("../../../../CRM/EntityPMs/ActivityPM");
var Args_1 = require("../../../../CRM/Args");
var ActivityPMService_1 = require("../../../../CRM/Services/StandardPMs/ActivityPMService");
var ActivityPMInitService_1 = require("../../../../CRM/EntityPMInitServices/ActivityPMInitService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var NewActivityComponent = /** @class */ (function () {
    function NewActivityComponent() {
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TypeCode = "";
        this.Retries = 0;
        this.IsAddCustomerAllowed = true;
        this.IsEnabled = true;
        this.RunComponent();
    }
    NewActivityComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.TypeCode = args.TypeCode;
            if (args.Activity != null) {
                this.EntityPM = args.Activity;
            }
            else {
                this.EntityPM = new ActivityPM_1.ActivityPM();
                ActivityPMInitService_1.ActivityPMInitService.InitValues(this.EntityPM, true);
                this.EntityPM.ActivityTypeCode = this.TypeCode;
            }
        }
        else {
            this.EntityPM = new ActivityPM_1.ActivityPM();
            ActivityPMInitService_1.ActivityPMInitService.InitValues(this.EntityPM, true);
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.CustomerId)) {
            this.EntityPM.CustomerId = args.CustomerId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.QuoteId)) {
            this.EntityPM.QuoteId = args.QuoteId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.OpportunityId)) {
            this.EntityPM.OpportunityId = args.OpportunityId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.TicketId)) {
            this.EntityPM.TicketId = args.TicketId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.Subject)) {
            this.EntityPM.Subject = args.Subject;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.DueDate)) {
            this.EntityPM.DueDate = args.DueDate;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(args.CallWithId)) {
            this.EntityPM.CallWithId = args.CallWithId;
        }
        this.IsAddCustomerAllowed = args.IsAddCustomerAllowed;
    };
    NewActivityComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewActivityComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewActivityComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./CRMModules/CRMActivity/Components/NewEntity/ActivityInputTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            _this.ActivityInputTemplate = cmpRef.instance;
            var args = new Args_1.ActivityInputArgs();
            args.Activity = _this.EntityPM;
            args.TypeCode = _this.TypeCode;
            args.IsEnabled = true;
            args.IsEditMode = false;
            args.IsAddCustomerAllowed = _this.IsAddCustomerAllowed;
            _this.ActivityInputTemplate.InitTemplate(args);
        });
    };
    NewActivityComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewActivityComponent.prototype.OkButtonClicked = function () {
        this.Ok();
    };
    NewActivityComponent.prototype.SaveAsCompletedClicked = function () {
        this.EntityPM.IsMarkedCompleted = true;
        this.Ok();
    };
    NewActivityComponent.prototype.Ok = function () {
        var _this = this;
        this.ValidationErrorsList = this.ActivityInputTemplate.Validate();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var service = new ActivityPMService_1.ActivityPMService();
            service.insert(this.EntityPM).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit('ok');
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], NewActivityComponent.prototype, "viewContainerRef", void 0);
    NewActivityComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewActivityComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewActivityComponent);
    return NewActivityComponent;
}());
exports.NewActivityComponent = NewActivityComponent;
//# sourceMappingURL=NewActivityComponent.js.map