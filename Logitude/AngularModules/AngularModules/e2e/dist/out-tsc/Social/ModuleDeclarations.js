"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SocialMainComponent_1 = require("./Components/SocialMainComponent");
var SocialPostsComponent_1 = require("./Components/SocialPostsComponent");
var SocialMessagesComponent_1 = require("./Components/SocialMessagesComponent");
var EditPostComponent_1 = require("./Components/EditPostComponent");
var SocialPeopleComponent_1 = require("./Components/SocialPeopleComponent");
var SocialContactNameLink_1 = require("./Components/QueryColumnsComponents/SocialContactNameLink");
var SocialPeopleFollowComponent_1 = require("./Components/QueryColumnsComponents/SocialPeopleFollowComponent");
var NewSocialMessageComponent_1 = require("./Components/NewSocialMessageComponent");
var SocialMessageParticipantsComponent_1 = require("./Components/SocialMessageParticipantsComponent");
var AddSocialMessageParticipantsComponent_1 = require("./Components/AddSocialMessageParticipantsComponent");
var ConversationMessageComponent_1 = require("./Components/ConversationMessageComponent");
var SocialInboxMessageComponent_1 = require("./Components/SocialInboxMessageComponent");
exports.ControlsComponents = [
    ConversationMessageComponent_1.ConversationMessageComponent,
];
exports.Components = [
    SocialMainComponent_1.SocialMainComponent,
    SocialPostsComponent_1.SocialPostsComponent,
    SocialMessagesComponent_1.SocialMessagesComponent,
    EditPostComponent_1.EditPostComponent,
    SocialPeopleComponent_1.SocialPeopleComponent,
    SocialContactNameLink_1.SocialContactNameLink,
    SocialPeopleFollowComponent_1.SocialPeopleFollowComponent,
    NewSocialMessageComponent_1.NewSocialMessageComponent,
    SocialMessageParticipantsComponent_1.SocialMessageParticipantsComponent,
    AddSocialMessageParticipantsComponent_1.AddSocialMessageParticipantsComponent,
    ConversationMessageComponent_1.ConversationMessageComponent,
    SocialInboxMessageComponent_1.SocialInboxMessageComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "SocialMainComponent": {
                myResult = SocialMainComponent_1.SocialMainComponent;
                break;
            }
            case "SocialPostsComponent": {
                myResult = SocialPostsComponent_1.SocialPostsComponent;
                break;
            }
            case "SocialMessagesComponent": {
                myResult = SocialMessagesComponent_1.SocialMessagesComponent;
                break;
            }
            case "EditPostComponent": {
                myResult = EditPostComponent_1.EditPostComponent;
                break;
            }
            case "SocialPeopleComponent": {
                myResult = SocialPeopleComponent_1.SocialPeopleComponent;
                break;
            }
            case "SocialContactNameLink": {
                myResult = SocialContactNameLink_1.SocialContactNameLink;
                break;
            }
            case "SocialPeopleFollowComponent": {
                myResult = SocialPeopleFollowComponent_1.SocialPeopleFollowComponent;
                break;
            }
            case "NewSocialMessageComponent": {
                myResult = NewSocialMessageComponent_1.NewSocialMessageComponent;
                break;
            }
            case "SocialMessageParticipantsComponent": {
                myResult = SocialMessageParticipantsComponent_1.SocialMessageParticipantsComponent;
                break;
            }
            case "AddSocialMessageParticipantsComponent": {
                myResult = AddSocialMessageParticipantsComponent_1.AddSocialMessageParticipantsComponent;
                break;
            }
            case "ConversationMessageComponent": {
                myResult = ConversationMessageComponent_1.ConversationMessageComponent;
                break;
            }
            case "SocialInboxMessageComponent": {
                myResult = SocialInboxMessageComponent_1.SocialInboxMessageComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map