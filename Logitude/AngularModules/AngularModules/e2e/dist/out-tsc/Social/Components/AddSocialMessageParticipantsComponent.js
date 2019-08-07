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
var Tools_1 = require("../../Infrastructure/Tools");
var ConversationHeaderParticipantPM_1 = require("../EntityPMs/ConversationHeaderParticipantPM");
var ConversationHeaderParticipantExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService");
var AddSocialMessageParticipantsComponent = /** @class */ (function () {
    function AddSocialMessageParticipantsComponent() {
        this.ConversationHeaderParticipantPMLists = [];
        this.ConversationHeaderId = "";
        this.UserIds = "";
        this.ValidationErrorsList = [];
        this.ExcludedResult = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.UserList = [];
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService_1.ConversationHeaderParticipantExtendedPMService();
    }
    AddSocialMessageParticipantsComponent.prototype.ngOnInit = function () {
    };
    AddSocialMessageParticipantsComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.SocialMessageParticipantsComponent = args.SocialMessageParticipantsComponent;
            this.ConversationHeaderId = args.ConversationHeaderId;
            this.ExcludedResult = args.SocialMessageParticipantsComponent.ExcludedResult;
        }
    };
    AddSocialMessageParticipantsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddSocialMessageParticipantsComponent.prototype.SaveButtonClick = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(this.UserIds)) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this.UserList = this.UserIds.split(';');
            var isNoName = false;
            this.ConversationHeaderParticipantPMLists = [];
            this.UserList.forEach(function (userid) {
                var participantUser = _this.ParticipantUserLists.filter(function (d) { return d.Id == userid; })[0];
                var name = "";
                if (participantUser)
                    name = participantUser.EnglishName;
                else
                    isNoName = true;
                _this.ConversationHeaderParticipantPMLists.push(_this.GetNewConversationHeaderParticipantPM(userid, name));
            });
            this.conversationHeaderParticipantExtendedPMService.SaveConversationHeaderParticipantPMLists(this.ConversationHeaderParticipantPMLists).subscribe(function (res) {
                var pmResponse = res;
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError && pmResponse.Result) {
                    if (!isNoName) {
                        _this.SocialMessageParticipantsComponent.BluidLists(pmResponse.Result);
                    }
                    else
                        _this.SocialMessageParticipantsComponent.LoadData();
                    //this.SocialMessageParticipantsComponent.IsChange = true;
                }
                _this.CloseButtonClicked();
            });
        }
        else {
            this.ValidationErrorsList.push("Please select participants");
        }
    };
    AddSocialMessageParticipantsComponent.prototype.GetNewConversationHeaderParticipantPM = function (userId, name) {
        var newConversationHeaderParticipantPM = new ConversationHeaderParticipantPM_1.ConversationHeaderParticipantPM();
        newConversationHeaderParticipantPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        newConversationHeaderParticipantPM.ConversationHeaderId = this.ConversationHeaderId;
        newConversationHeaderParticipantPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newConversationHeaderParticipantPM.IsLeft = false;
        newConversationHeaderParticipantPM.IsRead = false;
        newConversationHeaderParticipantPM.Replied = false;
        newConversationHeaderParticipantPM.IsDelete = false;
        newConversationHeaderParticipantPM.ParticipantUserId = userId;
        newConversationHeaderParticipantPM.ParticipantName = name;
        newConversationHeaderParticipantPM.LastReadDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        return newConversationHeaderParticipantPM;
    };
    AddSocialMessageParticipantsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddSocialMessageParticipantsComponent',
            templateUrl: './AddSocialMessageParticipantsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddSocialMessageParticipantsComponent);
    return AddSocialMessageParticipantsComponent;
}());
exports.AddSocialMessageParticipantsComponent = AddSocialMessageParticipantsComponent;
//# sourceMappingURL=AddSocialMessageParticipantsComponent.js.map