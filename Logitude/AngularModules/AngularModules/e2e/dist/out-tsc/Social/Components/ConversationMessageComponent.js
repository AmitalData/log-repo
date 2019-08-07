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
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var ConversationHeaderMessagePM_1 = require("../EntityPMs/ConversationHeaderMessagePM");
var ConversationHeaderParticipantExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService");
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var ConversationHeaderPMService_1 = require("../Services/StandardPMs/ConversationHeaderPMService");
var ConversationHeaderMessageExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderMessageExtendedPMService");
var GroupByPipe_1 = require("../../Infrastructure/Pipes/GroupByPipe");
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var ConversationHeaderMessagePMService_1 = require("../Services/StandardPMs/ConversationHeaderMessagePMService");
var ConversationMessageComponent = /** @class */ (function () {
    function ConversationMessageComponent() {
        this.WaitingForResponseKey = Guid_1.Guid.newGuid();
        this.ConversationHeaderMessageLists = [];
        this.ConversationHeaderMessagePMLists = [];
        this.IsNoData = false;
        this.IsStartWaitingLoading = false;
        this.BusyIndicatorText = "";
        this.ShowBusyIndicator = false;
        this.Area = "Message";
        this.LoggedContactImageDetailId = "";
        this.LoggedContactDefaultColor = "";
        this.MessageListsId = Guid_1.Guid.newGuid();
        this.IsLoadedMessages = false;
        this.ShowCheckBoxWaitingForResponse = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.messageBody = "";
        this.isWaitingForResponse = false;
        this.conversationHeaderMessageExtendedPMService = new ConversationHeaderMessageExtendedPMService_1.ConversationHeaderMessageExtendedPMService();
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService_1.ConversationHeaderParticipantExtendedPMService();
        this.conversationHeaderPMService = new ConversationHeaderPMService_1.ConversationHeaderPMService();
        this.conversationHeaderMessagePMService = new ConversationHeaderMessagePMService_1.ConversationHeaderMessagePMService();
    }
    ConversationMessageComponent.prototype.ngOnInit = function () {
        if (this.ConversationHeader) {
            this.BusyIndicatorText = "Loading...";
            this.ShowBusyIndicator = true;
            if (this.ConversationHeader.EntityPM.CreatedByUserId == SessionLocator_1.SessionLocator.LoggedUserId) {
                this.ShowCheckBoxWaitingForResponse = true;
            }
            else {
                this.ShowCheckBoxWaitingForResponse = false;
            }
            this.LoadData();
            if (this.ConversationHeader.EntityPM.NumberUnreadComment > 0) {
                this.MakeMeReadMessage();
            }
            if (this.SocialMessagesComponent) {
                this.LoggedContactDefaultColor = this.SocialMessagesComponent.PostsArgs.LoggedContactDefaultColor;
                this.LoggedContactImageDetailId = this.SocialMessagesComponent.PostsArgs.LoggedContactImageDetailId;
            }
        }
    };
    Object.defineProperty(ConversationMessageComponent.prototype, "MessageBody", {
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
    ConversationMessageComponent.prototype.LoadData = function () {
        var _this = this;
        this.conversationHeaderMessageExtendedPMService.GetAllConversationMessageForHeaderQuery(this.ConversationHeader.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            _this.ConversationHeaderMessagePMLists = [];
            _this.IsLoadedMessages = true;
            _this.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                pmResponse.Result.forEach(function (item) {
                    _this.ConversationHeaderMessagePMLists.push(item);
                });
                _this.BuildItemsSource();
            }
        });
    };
    Object.defineProperty(ConversationMessageComponent.prototype, "IsWaitingForResponse", {
        get: function () {
            if (this.ConversationHeader)
                this.isWaitingForResponse = this.ConversationHeader.EntityPM.IsWaitingForResponse;
            else
                this.isWaitingForResponse = false;
            return this.isWaitingForResponse;
        },
        set: function (newValue) {
            if (this.isWaitingForResponse != newValue) {
                this.isWaitingForResponse = newValue;
                this.WaitingForResponse(false);
            }
        },
        enumerable: true,
        configurable: true
    });
    ConversationMessageComponent.prototype.WaitingForResponse = function (isloadBusyIndicator) {
        var _this = this;
        if (isloadBusyIndicator === void 0) { isloadBusyIndicator = true; }
        this.ConversationHeader.IsChange = true;
        if (!this.IsStartWaitingLoading) {
            this.IsStartWaitingLoading = true;
            if (isloadBusyIndicator) {
                this.ShowBusyIndicator = true;
                this.BusyIndicatorText = "Saving...";
            }
            this.IsStartWaitingLoading = true;
            if (this.ConversationHeader.EntityPM.IsWaitingForResponse) {
                this.ConversationHeader.EntityPM.IsWaitingForResponse = false;
            }
            else {
                this.ConversationHeader.EntityPM.IsWaitingForResponse = true;
            }
            this.conversationHeaderPMService.update(this.ConversationHeader.EntityPM).subscribe(function (res) {
                var pmResponse = res;
                _this.IsStartWaitingLoading = false;
                if (_this.ConversationHeader.EntityPM.IsWaitingForResponse)
                    _this.ConversationHeader.VisibilityIsWaitingForResponse = true;
                else
                    _this.ConversationHeader.VisibilityIsWaitingForResponse = false;
                if (isloadBusyIndicator) {
                    _this.ShowBusyIndicator = false;
                }
            });
        }
    };
    ConversationMessageComponent.prototype.MakeMeReadMessage = function () {
        var _this = this;
        this.ConversationHeader.IsChange = true;
        this.conversationHeaderParticipantExtendedPMService.MakeMeReadMessage(this.ConversationHeader.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.ConversationHeader.EntityPM.IsRead = true;
                _this.ConversationHeader.EntityPM.NumberUnreadComment = 0;
                _this.ConversationHeader.IsShowNumberUnreadComment = false;
                _this.ConversationHeader.NumberUnreadComment = "0";
                _this.ConversationHeader.BackgroundConversationHeader = _this.ConversationHeader.Area == "Message" ? "#F2F2F2" : "#F5F5F5";
                _this.ConversationHeader.ForegroundMessageBody = "#000000";
                _this.ConversationHeader.ForegroundMessageParticipants = "#000000";
                _this.ConversationHeader.MarkAsReadLable = "Mark as Unread";
                _this.ConversationHeader.FontWeightbody = "normal";
                _this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
            }
        });
    };
    ConversationMessageComponent.prototype.MarkAsReadButtonClick = function () {
        var _this = this;
        this.ConversationHeader.IsChange = true;
        this.BusyIndicatorText = "Saving...";
        this.ShowBusyIndicator = true;
        this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantReadAndUnRead(this.ConversationHeader.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId, this.ConversationHeader.MarkAsReadLable).subscribe(function (res) {
            var pmResponse = res;
            _this.ShowBusyIndicator = false;
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    if (_this.ConversationHeader.MarkAsReadLable == "Mark as Unread") {
                        _this.ConversationHeader.EntityPM.NumberUnreadComment = 1;
                        _this.ConversationHeader.NumberUnreadComment = "1";
                        _this.ConversationHeader.ForegroundMessageBody = "#000000";
                        _this.ConversationHeader.BackgroundConversationHeader = _this.ConversationHeader.Area == "Message" ? "#ffffff" : "#F5F5F5";
                        _this.ConversationHeader.ForegroundMessageParticipants = "#000000";
                        _this.ConversationHeader.FontWeightbody = "bold";
                        _this.ConversationHeader.IsShowNumberUnreadComment = true;
                        if (_this.ConversationHeader.EntityPM.IsWaitingForResponse) {
                            _this.WaitingForResponse();
                        }
                    }
                    else {
                        _this.ConversationHeader.EntityPM.NumberUnreadComment = 0;
                        _this.ConversationHeader.NumberUnreadComment = "0";
                        _this.ConversationHeader.IsShowNumberUnreadComment = false;
                        _this.ConversationHeader.FontWeightbody = "normal";
                        _this.ConversationHeader.ForegroundMessageBody = "#787878";
                        _this.ConversationHeader.BackgroundConversationHeader = _this.ConversationHeader.Area == "Message" ? "#F2F2F2" : "#F5F5F5";
                        _this.ConversationHeader.ForegroundMessageParticipants = "#282E30";
                    }
                }
                _this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
            }
        });
    };
    ConversationMessageComponent.prototype.DeleteConversationButtonClick = function () {
        var _this = this;
        this.ConversationHeader.IsChange = true;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this Message?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.BusyIndicatorText = "Saving...";
                _this.ShowBusyIndicator = true;
                _this.conversationHeaderParticipantExtendedPMService.DeleteConversationHeaderParticipant(_this.ConversationHeader.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
                    var pmResponse = res;
                    _this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError) {
                        if (_this.SocialMessagesComponent && _this.ConversationHeader.Area == "Message") {
                            _this.SocialMessagesComponent.RemoveConversationHeader(_this.ConversationHeader); //.ConversationHeaderLists.filter(d => d.EntityPM.Id != this.ConversationHeader.EntityPM.Id);
                            _this.BackButtonClicked();
                        }
                        else if (_this.ConversationHeader.Area == "Inbox") {
                            _this.CurrentSession.FireEvent("RemoveItemFromInboxMessagesRefresh");
                        }
                    }
                });
            }
        });
    };
    ConversationMessageComponent.prototype.ReplyButtonClick = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MessageBody)) {
            if (this.MessageBody.length < 999) {
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;
                var message = new ConversationHeaderMessagePM_1.ConversationHeaderMessagePM();
                message.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                message.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserPM.Id;
                message.Tenant = SessionLocator_1.SessionLocator.Tenant;
                message.ConversationHeaderId = this.ConversationHeader.ConversationHeaderId;
                message.MessageBody = this.MessageBody;
                message.UserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                message.DefaultColor = this.LoggedContactDefaultColor;
                message.UserImageDetailId = this.LoggedContactImageDetailId;
                // message.
                this.MessageBody = "";
                this.conversationHeaderMessagePMService.insert(message).subscribe(function (res) {
                    var pmResponse = res;
                    _this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError && pmResponse.Result) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("ESN", "New General Message");
                        _this.ConversationHeaderMessagePMLists.push(message);
                        _this.BuildItemsSource(true);
                        _this.ConversationHeader.EntityPM.LasMessageUserId = message.CreatedByUserId;
                        _this.ConversationHeader.EntityPM.LasMessageUserName = message.UserName;
                        _this.ConversationHeader.EntityPM.LastMessageDate = message.CreateDate;
                        _this.ConversationHeader.EntityPM.IsReplied = true;
                        _this.ConversationHeader.MessageBody = message.MessageBody;
                        _this.ConversationHeader.CreateDate = message.CreateDate;
                        _this.ConversationHeader.IsChange = true;
                        if (_this.ConversationHeader.EntityPM.CreatedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                            if (_this.ConversationHeader.EntityPM.LasMessageUserId != _this.ConversationHeader.EntityPM.CreatedByUserId)
                                _this.ConversationHeader.IsShowArrowReceiveMessage = true;
                            else
                                _this.ConversationHeader.IsShowArrowReceiveMessage = false;
                        }
                        else
                            _this.ConversationHeader.IsShowArrowReceiveMessage = false;
                        //ShowArrowSend
                        if (_this.ConversationHeader.EntityPM.LasMessageUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                            if (_this.ConversationHeader.EntityPM.NumberUnreadComment > 0)
                                _this.ConversationHeader.IsShowArrowSend = true;
                            else
                                _this.ConversationHeader.IsShowArrowSend = false;
                        }
                        else
                            _this.ConversationHeader.IsShowArrowSend = false;
                        if (_this.ConversationHeader.EntityPM.MessageParticipantsCount >= 3) {
                            if (_this.ConversationHeader.EntityPM.LasMessageUserName != null) {
                                if (_this.ConversationHeader.EntityPM.LasMessageUserId == SessionLocator_1.SessionLocator.LoggedUserId) {
                                    _this.ConversationHeader.LasMessageUserName = "";
                                }
                                else {
                                    _this.ConversationHeader.LasMessageUserName = _this.ConversationHeader.EntityPM.LasMessageUserName + ": ";
                                }
                            }
                        }
                        else
                            _this.ConversationHeader.LasMessageUserName = "";
                        if (_this.ConversationHeader.EntityPM.IsWaitingForResponse == true) {
                            if (_this.ConversationHeader.EntityPM.CreatedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                                _this.ConversationHeader.EntityPM.IsWaitingForResponse = false;
                                _this.ConversationHeader.VisibilityIsWaitingForResponse = false;
                                _this.ConversationHeader.VisibilityIsNotWaitingForResponse = true;
                            }
                        }
                        if (_this.ConversationHeader.EntityPM.NumberUnreadComment > 0) {
                            _this.MakeMeReadMessage();
                        }
                        _this.conversationHeaderPMService.update(_this.ConversationHeader.EntityPM).subscribe(function (res) {
                            var pmResponse = res;
                            _this.MakeMessageRepliedOrRead("Replied");
                            _this.MakeMessageRead();
                            _this.MakeDeleteParticioantsUnDelete();
                        });
                    }
                });
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Message maximum charachters should be less than 1000!");
            }
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("This Message seems to be empty. Please, write something to Message");
        }
    };
    ConversationMessageComponent.prototype.MakeMessageRepliedOrRead = function (type) {
        this.ConversationHeader.IsChange = true;
        var isLoad = false;
        if (type == "Replied")
            isLoad = true;
        else if (type == "Read") {
            if (this.ConversationHeader.EntityPM.IsRead == false) {
                isLoad = true;
            }
        }
        if (isLoad) {
            this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantRepliedOrRead(this.ConversationHeader.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId, type).subscribe(function (res) {
                var pmResponse = res;
            });
        }
    };
    ConversationMessageComponent.prototype.MakeMessageRead = function () {
        this.ConversationHeader.IsChange = true;
        this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantReadMessage(this.ConversationHeader.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
        });
    };
    ConversationMessageComponent.prototype.MakeDeleteParticioantsUnDelete = function () {
        this.ConversationHeader.IsChange = true;
        this.conversationHeaderParticipantExtendedPMService.MakeDeleteParticipantUnDelete(this.ConversationHeader.ConversationHeaderId).subscribe(function (res) {
            var pmResponse = res;
        });
    };
    ConversationMessageComponent.prototype.BuildItemsSource = function (isScrolToBottom) {
        var _this = this;
        if (isScrolToBottom === void 0) { isScrolToBottom = false; }
        var myPipe = new GroupByPipe_1.GroupByPipe();
        this.ConversationHeaderMessageLists = [];
        var conversationHeaderMessageViewModelLists = [];
        this.ConversationHeaderMessagePMLists.forEach(function (item) {
            conversationHeaderMessageViewModelLists.push(new ConversationHeaderMessageViewModel(item, _this.ConversationHeader));
        });
        var list = myPipe.transform(conversationHeaderMessageViewModelLists, "Date");
        list.forEach(function (value, key) {
            _this.ConversationHeaderMessageLists.push(new ConversationMessageData(value));
        });
        if (isScrolToBottom)
            this.ScrolToBottom();
    };
    ConversationMessageComponent.prototype.BackButtonClicked = function () {
        if (this.SocialMessagesComponent) {
            this.SocialMessagesComponent.IsViewMessage = false;
            if (this.ConversationHeader.IsChange) {
                this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
            }
        }
    };
    ConversationMessageComponent.prototype.StopBusyIndicator = function () {
        if (this.IsLoadedMessages) {
            this.ShowBusyIndicator = false;
        }
    };
    ConversationMessageComponent.prototype.ScrolToBottom = function () {
        var htmlid = document.getElementById(this.MessageListsId);
        htmlid.scrollTop = htmlid.scrollHeight + 30;
    };
    ConversationMessageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ConversationMessageComponent',
            templateUrl: './ConversationMessageComponent.html',
            inputs: ['SocialMessagesComponent', 'ConversationHeader', 'Area'],
        }),
        __metadata("design:paramtypes", [])
    ], ConversationMessageComponent);
    return ConversationMessageComponent;
}());
exports.ConversationMessageComponent = ConversationMessageComponent;
var ConversationMessageData = /** @class */ (function () {
    function ConversationMessageData(item) {
        this.ConversationHeaderMessageViewModelLists = [];
        this.Key = item.key;
        this.ConversationHeaderMessageViewModelLists = item.value;
    }
    return ConversationMessageData;
}());
exports.ConversationMessageData = ConversationMessageData;
var ConversationHeaderMessageViewModel = /** @class */ (function () {
    function ConversationHeaderMessageViewModel(entityPM, conversationHeader) {
        this.IsShowArrowReceiveMessage = false;
        this.Date = Tools_1.DateTool.GetDateFormats(entityPM.CreateDate).DateString;
        this.EntityPM = entityPM;
        this.CreatedByUserName = entityPM.UserName;
        this.UserImageDetailId = entityPM.UserImageDetailId;
        this.CreateDate = entityPM.CreateDate;
        this.MessageBody = entityPM.MessageBody;
        this.UserImagebackground = entityPM.DefaultColor;
        if (!this.UserImageDetailId) {
            if (this.CreatedByUserName) {
                var Name = this.CreatedByUserName.split(' ');
                var userNameImage = "";
                if (Name.length == 1) {
                    if (Name[0]) {
                        if (Name[0].length > 2) {
                            userNameImage = Name[0].charAt(0);
                            userNameImage += Name[0].charAt(1);
                        }
                        else {
                            userNameImage = Name[0];
                        }
                    }
                }
                else if (Name.length > 1) {
                    if (Name[0]) {
                        if (Name[0].length > 1) {
                            userNameImage += Name[0].charAt(0);
                        }
                        else {
                            userNameImage += Name[0];
                        }
                    }
                    if (Name[1]) {
                        if (Name[1].length > 1) {
                            userNameImage += Name[1].charAt(0);
                        }
                        else {
                            userNameImage += Name[1];
                        }
                    }
                    else {
                        if (Name[0].length > 2) {
                            userNameImage = Name[0].charAt(0);
                            userNameImage += Name[0].charAt(1);
                        }
                        else {
                            userNameImage = Name[0];
                        }
                    }
                }
                if (userNameImage)
                    this.UserNameImage = userNameImage.toUpperCase();
                this.UserNameImage = userNameImage;
            }
        }
        if (conversationHeader.EntityPM.CreatedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
            if (entityPM.CreatedByUserId == conversationHeader.EntityPM.CreatedByUserId)
                this.IsShowArrowReceiveMessage = false;
            else
                this.IsShowArrowReceiveMessage = true;
        }
        else
            this.IsShowArrowReceiveMessage = false;
    }
    return ConversationHeaderMessageViewModel;
}());
exports.ConversationHeaderMessageViewModel = ConversationHeaderMessageViewModel;
//# sourceMappingURL=ConversationMessageComponent.js.map