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
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var ConversationHeaderParticipantExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService");
var SocialMessageParticipantsComponent = /** @class */ (function () {
    function SocialMessageParticipantsComponent() {
        this.ConversationHeaderParticipantPMLists = [];
        this.ConversationHeaderId = "";
        this.Area = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.MessageOthers = "";
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService_1.ConversationHeaderParticipantExtendedPMService();
    }
    SocialMessageParticipantsComponent.prototype.ngOnInit = function () {
    };
    SocialMessageParticipantsComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.ConversationHeader = args.ConversationHeader;
            if (this.ConversationHeader) {
                this.ConversationHeaderId = this.ConversationHeader.ConversationHeaderId;
                this.Area = this.ConversationHeader.Area;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ConversationHeaderId)) {
            this.LoadData();
        }
    };
    SocialMessageParticipantsComponent.prototype.AddConversationHeaderParticipantPMButtonClicked = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.SocialMessageParticipantsComponent = this;
        windowArgs.ConversationHeaderId = this.ConversationHeaderId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 510;
        logWindow.Height = 200;
        logWindow.Title = "New Participants";
        logWindow.Show("./Social/Components/AddSocialMessageParticipantsComponent");
    };
    SocialMessageParticipantsComponent.prototype.LoadData = function () {
        var _this = this;
        this.ExcludedResult = [];
        this.ConversationHeaderParticipantPMLists = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.conversationHeaderParticipantExtendedPMService.GetAllConversationHeaderParticipantPMByConversationHeaderId(this.ConversationHeaderId).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                _this.BluidLists(pmResponse.Result);
            }
        });
    };
    SocialMessageParticipantsComponent.prototype.BluidLists = function (conversationHeaderParticipantPMLists, addParticipant) {
        var _this = this;
        if (addParticipant === void 0) { addParticipant = false; }
        conversationHeaderParticipantPMLists.forEach(function (item) {
            _this.ConversationHeaderParticipantPMLists.push(item);
            _this.ExcludedResult.push(item.ParticipantUserId);
        });
        this.ConversationHeader.MessageOthers = "+ " + (this.ConversationHeaderParticipantPMLists.length - 3).toString() + " more";
        if (addParticipant)
            this.ConversationHeader.IsChange = true;
    };
    SocialMessageParticipantsComponent.prototype.SortItemSource = function () {
        this.ConversationHeaderParticipantPMLists = this.ConversationHeaderParticipantPMLists.sort();
        this.ConversationHeaderParticipantPMLists.sort(function (a, b) {
            if (a.ParticipantName.toLowerCase() < b.ParticipantName.toLowerCase()) {
                return -1;
            }
            else if (a.ParticipantName.toLowerCase() > b.ParticipantName.toLowerCase()) {
                return 1;
            }
            else {
                return 0;
            }
        });
    };
    SocialMessageParticipantsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SocialMessageParticipantsComponent.prototype.SaveButtonClicked = function () {
        this.CloseButtonClicked();
    };
    SocialMessageParticipantsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialMessageParticipantsComponent',
            templateUrl: './SocialMessageParticipantsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SocialMessageParticipantsComponent);
    return SocialMessageParticipantsComponent;
}());
exports.SocialMessageParticipantsComponent = SocialMessageParticipantsComponent;
//# sourceMappingURL=SocialMessageParticipantsComponent.js.map