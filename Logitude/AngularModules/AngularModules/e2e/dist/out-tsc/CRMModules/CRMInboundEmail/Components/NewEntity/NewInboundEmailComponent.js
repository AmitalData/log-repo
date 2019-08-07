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
var InboundEmailPM_1 = require("../../../../Infrastructure/EntityPMs/InboundEmailPM");
var InboundEmailLinePM_1 = require("../../../../Infrastructure/EntityPMs/InboundEmailLinePM");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_2 = require("../../../../Infrastructure/Tools");
var InboundEmailWebService_1 = require("../../../../Infrastructure/Services/WebServices/InboundEmailWebService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var NewInboundEmailComponent = /** @class */ (function (_super) {
    __extends(NewInboundEmailComponent, _super);
    function NewInboundEmailComponent() {
        var _this = _super.call(this) || this;
        _this.entityPM = new InboundEmailPM_1.InboundEmailPM();
        _this.linePM = new InboundEmailLinePM_1.InboundEmailLinePM(null);
        _this.ObjectTableName = "InboundEmailLine";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    NewInboundEmailComponent.prototype.ngOnInit = function () {
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.entityPM = new InboundEmailPM_1.InboundEmailPM();
        this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.entityPM.CreateDate = todayDate;
        this.entityPM.UpdateDate = todayDate;
        this.linePM = new InboundEmailLinePM_1.InboundEmailLinePM(null);
        this.linePM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.linePM.InboundEmailId = this.entityPM.Id;
        this.linePM.CreateDate = todayDate;
        this.entityPM.AddInboundEmailLinePM(this.linePM);
    };
    Object.defineProperty(NewInboundEmailComponent.prototype, "Recepient", {
        get: function () { return this.linePM.Recepient; },
        set: function (value) {
            if (this.linePM.Recepient != value) {
                this.linePM.Recepient = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewInboundEmailComponent.prototype, "Subject", {
        get: function () { return this.linePM.Subject; },
        set: function (value) {
            if (this.linePM.Subject != value) {
                this.linePM.Subject = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewInboundEmailComponent.prototype, "Body", {
        get: function () { return this.linePM.Body; },
        set: function (value) {
            if (this.linePM.Body != value) {
                this.linePM.Body = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewInboundEmailComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewInboundEmailComponent.prototype.OkButtonClicked = function () {
        var errors = [];
        var toEmails = this.Recepient.replace(" ", "");
        var emailbody = this.Body;
        var emailSubject = this.Subject;
        if (Tools_2.AppTool.IsNullOrEmpty(toEmails)) {
            errors.push("Sending Email", "Please specify at least one recepient");
            return;
        }
        var isValidEmailsTo = this.CheckIsValidEmails(toEmails);
        if (!isValidEmailsTo) {
            errors.push("Sending Document", "Some of To e-mails are Invalid");
            return;
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            //this.Send();
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    NewInboundEmailComponent.prototype.CheckIsValidEmails = function (mailsList) {
        var isOk = true;
        var mails = mailsList.split(';');
        mails.forEach(function (email) {
            if (!Tools_2.AppTool.IsNullOrEmpty(email)) {
                isOk = Tools_2.FormatTool.IsEmail(email);
            }
        });
        return isOk;
    };
    NewInboundEmailComponent.prototype.Send = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("Sending...");
        if (this.myInboundEmailWebService == null) {
            this.myInboundEmailWebService = new InboundEmailWebService_1.InboundEmailWebService();
        }
        this.myInboundEmailWebService.SendInboundEmailAsync(this.Recepient, this.entityPM.Tenant, this.Subject, this.Body, this.entityPM.Id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var mySendingResultClass = myResponse.Result;
                if (mySendingResultClass != null) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show("Email has been sent successfully");
                }
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewInboundEmailComponent = __decorate([
        core_1.Component({
            selector: 'TicketDocsOutTabComponent',
            moduleId: module.id,
            templateUrl: './NewInboundEmailComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewInboundEmailComponent);
    return NewInboundEmailComponent;
}(BaseComponent_1.BaseComponent));
exports.NewInboundEmailComponent = NewInboundEmailComponent;
//# sourceMappingURL=NewInboundEmailComponent.js.map