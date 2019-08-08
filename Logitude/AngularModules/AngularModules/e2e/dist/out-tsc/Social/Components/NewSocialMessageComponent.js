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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var Tools_1 = require("../../Infrastructure/Tools");
var ConversationHeaderPM_1 = require("../EntityPMs/ConversationHeaderPM");
var ConversationHeaderParticipantPM_1 = require("../EntityPMs/ConversationHeaderParticipantPM");
var ConversationHeaderMessagePM_1 = require("../EntityPMs/ConversationHeaderMessagePM");
var ConversationHeaderPMService_1 = require("../Services/StandardPMs/ConversationHeaderPMService");
var ConversationHeaderMessagePMService_1 = require("../Services/StandardPMs/ConversationHeaderMessagePMService");
var ConversationHeaderParticipantExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService");
var NewSocialMessageComponent = /** @class */ (function () {
    function NewSocialMessageComponent() {
        this.IsSaveConversationHeaderParticipantComplete = false;
        this.IsSaveConversationHeaderMessagePMComplete = false;
        this.WaitingForResponseKey = Guid_1.Guid.newGuid();
        this.RegardingEntity = "";
        this.UserList = [];
        this.ExcludedResult = [];
        this.ValidationErrorsList = [];
        this.ConversationHeaderParticipantPMLists = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.EntityId = "";
        this.ObjectTableId = "";
        this.EntityDescription = "";
        this.AreaMessage = "";
        this.Placeholder = "Type message here...";
        this.messageBody = "";
        this.isWaitingForResponse = true;
        this.conversationHeaderPMService = new ConversationHeaderPMService_1.ConversationHeaderPMService();
        this.conversationHeaderMessagePMService = new ConversationHeaderMessagePMService_1.ConversationHeaderMessagePMService();
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService_1.ConversationHeaderParticipantExtendedPMService();
        this.ExcludedResult = [];
        this.ExcludedResult.push(SessionLocator_1.SessionLocator.LoggedUserId);
    }
    NewSocialMessageComponent.prototype.ngOnInit = function () {
    };
    NewSocialMessageComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.PostsArgs = args.PostsArgs;
            this.AreaMessage = args.AreaMessage;
            if (this.PostsArgs) {
                this.ObjectTableId = !Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.ObjectTableId) ? this.PostsArgs.ObjectTableId : null;
                this.EntityId = !Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.EntityId) ? this.PostsArgs.EntityId : null;
                this.EntityDescription = !Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.EntityDescription) ? this.PostsArgs.EntityDescription : null;
                this.RegardingEntity = !Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.RegardingEntity) ? this.PostsArgs.RegardingEntity : null;
            }
        }
    };
    Object.defineProperty(NewSocialMessageComponent.prototype, "MessageBody", {
        get: function () {
            return this.messageBody;
        },
        set: function (newValue) {
            if (this.messageBody != newValue) {
                this.messageBody = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewSocialMessageComponent.prototype, "IsWaitingForResponse", {
        get: function () {
            return this.isWaitingForResponse;
        },
        set: function (newValue) {
            if (this.isWaitingForResponse != newValue) {
                this.isWaitingForResponse = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    NewSocialMessageComponent.prototype.GetNewConversationHeaderParticipantPM = function (userId) {
        var newConversationHeaderParticipantPM = new ConversationHeaderParticipantPM_1.ConversationHeaderParticipantPM();
        newConversationHeaderParticipantPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newConversationHeaderParticipantPM.ConversationHeaderId = this.NewConversationHeaderPM.Id;
        newConversationHeaderParticipantPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newConversationHeaderParticipantPM.IsLeft = false;
        newConversationHeaderParticipantPM.IsRead = false;
        newConversationHeaderParticipantPM.Replied = false;
        newConversationHeaderParticipantPM.IsDelete = false;
        newConversationHeaderParticipantPM.ParticipantUserId = userId;
        newConversationHeaderParticipantPM.LastReadDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        return newConversationHeaderParticipantPM;
    };
    NewSocialMessageComponent.prototype.StartSaving = function () {
        var _this = this;
        this.IsSaveConversationHeaderParticipantComplete = false;
        this.IsSaveConversationHeaderMessagePMComplete = false;
        this.CurrentSession.StartBusyIndicator("Sending...");
        this.NewConversationHeaderPM = new ConversationHeaderPM_1.ConversationHeaderPM();
        this.NewConversationHeaderPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        this.NewConversationHeaderPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        this.NewConversationHeaderPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.NewConversationHeaderPM.IsWaitingForResponse = this.IsWaitingForResponse;
        this.NewConversationHeaderPM.EntityId = this.EntityId;
        this.NewConversationHeaderPM.ObjectTableId = this.ObjectTableId;
        this.NewConversationHeaderPM.EntityDescription = this.EntityDescription;
        this.conversationHeaderPMService.insert(this.NewConversationHeaderPM).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.SaveConversationHeaderParticipantPM();
                var conversationHeaderMessagePM = new ConversationHeaderMessagePM_1.ConversationHeaderMessagePM();
                conversationHeaderMessagePM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                conversationHeaderMessagePM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                conversationHeaderMessagePM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                conversationHeaderMessagePM.ConversationHeaderId = _this.NewConversationHeaderPM.Id;
                conversationHeaderMessagePM.MessageBody = _this.MessageBody;
                conversationHeaderMessagePM.UserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                conversationHeaderMessagePM.RegardingEntity = _this.RegardingEntity;
                _this.conversationHeaderMessagePMService.insert(conversationHeaderMessagePM).subscribe(function (res) {
                    var pmResponse = res;
                    _this.IsSaveConversationHeaderMessagePMComplete = true;
                    if (pmResponse.HasError && pmResponse.ErrorsArray) {
                        pmResponse.ErrorsArray.forEach(function (error) {
                            _this.ValidationErrorsList.push(error);
                        });
                    }
                    _this.StopBusyIndicator(!pmResponse.HasError);
                });
            }
            else {
                if (pmResponse.HasError && pmResponse.ErrorsArray) {
                    pmResponse.ErrorsArray.forEach(function (error) {
                        _this.ValidationErrorsList.push(error);
                    });
                }
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewSocialMessageComponent.prototype.SaveConversationHeaderParticipantPM = function () {
        var _this = this;
        this.UserList = this.UserIds.split(';');
        this.ConversationHeaderParticipantPMLists = [];
        this.UserList.forEach(function (userid) {
            _this.ConversationHeaderParticipantPMLists.push(_this.GetNewConversationHeaderParticipantPM(userid));
        });
        var userParticipant = this.UserList.filter(function (d) { return d == SessionLocator_1.SessionLocator.LoggedUserId; })[0];
        if (!userParticipant) {
            this.ConversationHeaderParticipantPMLists.push(this.GetNewConversationHeaderParticipantPM(SessionLocator_1.SessionLocator.LoggedUserId));
        }
        this.conversationHeaderParticipantExtendedPMService.SaveConversationHeaderParticipantPMLists(this.ConversationHeaderParticipantPMLists).subscribe(function (res) {
            var pmResponse = res;
            _this.IsSaveConversationHeaderParticipantComplete = true;
            if (pmResponse.HasError && pmResponse.ErrorsArray) {
                pmResponse.ErrorsArray.forEach(function (error) {
                    _this.ValidationErrorsList.push(error);
                });
            }
            _this.StopBusyIndicator(!pmResponse.HasError);
        });
    };
    NewSocialMessageComponent.prototype.StopBusyIndicator = function (isCloseWindow) {
        if (this.IsSaveConversationHeaderParticipantComplete && this.IsSaveConversationHeaderMessagePMComplete) {
            this.CurrentSession.StopBusyIndicator();
            if (isCloseWindow) {
                if (this.AreaMessage == "Inbox")
                    this.CurrentSession.FireEvent("SocialInboxMessagesRefresh");
                else if (this.AreaMessage == "Message") {
                    this.CurrentSession.FireEvent("SocialMessagesRefresh");
                }
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    };
    NewSocialMessageComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewSocialMessageComponent.prototype.MessageInputFocus = function () {
        this.Placeholder = "";
    };
    NewSocialMessageComponent.prototype.SaveButtonClick = function () {
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MessageBody) && this.MessageBody != "Type message here...") {
            if (this.MessageBody.length <= 999) {
                if (Tools_1.AppTool.IsNullOrEmpty(this.UserIds)) {
                    this.ValidationErrorsList.push("Please select at least one recipient");
                }
                else
                    this.StartSaving();
            }
            else
                this.ValidationErrorsList.push("Message Body field Must be less than 1000 characters");
        }
        else
            this.ValidationErrorsList.push("Message Body field is required");
    };
    NewSocialMessageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'NewSocialMessageComponent',
            templateUrl: './NewSocialMessageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewSocialMessageComponent);
    return NewSocialMessageComponent;
}());
exports.NewSocialMessageComponent = NewSocialMessageComponent;
//# sourceMappingURL=NewSocialMessageComponent.js.map