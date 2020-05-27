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
var SocialMessagesComponent_1 = require("../Components/SocialMessagesComponent");
var MessageFilters_1 = require("../DataContracts/MessageFilters");
var ConversationHeaderExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderExtendedPMService");
var ConversationHeaderPMService_1 = require("../Services/StandardPMs/ConversationHeaderPMService");
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var ConversationHeaderParticipantExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService");
var SocialInboxMessageComponent = /** @class */ (function () {
    function SocialInboxMessageComponent() {
        this.IsChange = false;
        this.InBoxMessageQuerySelectedValue = "All";
        this.ShowBusyIndicator = false;
        this.BusyIndicatorText = "";
        this.IsViewMessage = false;
        this.SocialInboxMessageRefreshEvent = null;
        this.ConversationHeaderLists = [];
        this.ScreenCode = "";
        this.PageIndex = 0;
        this.PageSize = 0;
        this.IsNoResult = false;
        this.IsLoadRun = false;
        this.IsNoData = false;
        this.IsLoadedMessages = false;
        this.LoadedMessagesCount = 0;
        this.InsideEntity = false;
        this.MessageListsId = Guid_1.Guid.newGuid();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsStartWaitingLoading = false;
        this.IsChangeConversationHeaderMessageComponent = false;
        this.conversationHeaderExtendedPMService = new ConversationHeaderExtendedPMService_1.ConversationHeaderExtendedPMService();
        this.conversationHeaderPMService = new ConversationHeaderPMService_1.ConversationHeaderPMService();
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService_1.ConversationHeaderParticipantExtendedPMService();
        this.Listen();
    }
    SocialInboxMessageComponent.prototype.ngOnInit = function () {
    };
    SocialInboxMessageComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.SocialInboxMessageRefreshEvent) {
            this.SocialInboxMessageRefreshEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "SocialInboxMessagesRefresh") {
                    _this.LoadingMessageList();
                    _this.IsChange = true;
                }
                if (s == "RemoveItemFromInboxMessagesRefresh") {
                    if (_this.ConversationHeader) {
                        _this.RemoveConversationHeader(_this.ConversationHeader);
                    }
                }
            });
        }
    };
    SocialInboxMessageComponent.prototype.ngOnDestroy = function () {
        if (this.SocialInboxMessageRefreshEvent) {
            this.SocialInboxMessageRefreshEvent.unsubscribe();
            this.SocialInboxMessageRefreshEvent = null;
        }
    };
    SocialInboxMessageComponent.prototype.InitializeMessageComponent = function (args, socialMessagesComponent) {
        this.PostsArgs = args;
        this.SocialMessagesComponent = socialMessagesComponent;
        if (Tools_1.AppTool.IsNullOrEmpty(this.ScreenCode) || this.ScreenCode == args.ScreenCode) {
            if (Tools_1.AppTool.IsNullOrEmpty(args.UserId)) {
                args.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
            }
            this.InsideEntity = args.InsideEntity;
            this.MessageFilters = new MessageFilters_1.MessageFilters();
            this.MessageFilters.PageIndex = 0;
            this.MessageFilters.PageSize = 10;
            this.MessageFilters.QueryName = "All";
            this.MessageFilters.UserId = args.UserId;
            this.MessageFilters.ObjectTableId = args.ObjectTableId;
            this.MessageFilters.EntityId = args.EntityId;
            this.MessageFilters.AreaMessage = "Inbox";
            this.ScreenCode = args.ScreenCode;
            this.LoadingMessageList();
        }
    };
    SocialInboxMessageComponent.prototype.BackButtonClicked = function () {
        if (this.ComponentRef) {
            if (this.IsChange || this.ConversationHeaderLists.filter(function (d) { return d.IsChange; })[0]) {
                this.CurrentSession.FireEvent("SocialMessagesRefresh");
                this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
            }
            this.ComponentRef.destroy();
        }
    };
    SocialInboxMessageComponent.prototype.InBoxMessageQueryChangedMethod = function (index) {
        var queryName = "";
        switch (index) {
            case 0:
                queryName = "All";
                break;
            case 1:
                queryName = "Unread";
                break;
            case 2:
                queryName = "Waiting for response";
                break;
        }
        this.InBoxMessageQuerySelectedValue = queryName;
        this.LoadingMessageList(queryName);
    };
    SocialInboxMessageComponent.prototype.MarkAsReadButtonClick = function (item) {
        var _this = this;
        this.BusyIndicatorText = "Saving...";
        this.ShowBusyIndicator = true;
        this.IsChange = true;
        this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantReadAndUnRead(item.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId, item.MarkAsReadLable).subscribe(function (res) {
            var pmResponse = res;
            _this.ShowBusyIndicator = false;
            if (!pmResponse.HasError) {
                if (pmResponse.Result) {
                    if (item.MarkAsReadLable == "Mark as Unread") {
                        item.EntityPM.NumberUnreadComment = 1;
                        item.NumberUnreadComment = "1";
                        item.ForegroundMessageBody = "#000000";
                        item.BackgroundConversationHeader = item.Area == "Message" ? "#ffffff" : "#F5F5F5";
                        item.ForegroundMessageParticipants = "#000000";
                        item.FontWeightbody = "bold";
                        item.IsShowNumberUnreadComment = true;
                        if (item.EntityPM.IsWaitingForResponse) {
                            _this.WaitingForResponse(item);
                        }
                    }
                    else {
                        item.EntityPM.NumberUnreadComment = 0;
                        item.NumberUnreadComment = "0";
                        item.IsShowNumberUnreadComment = false;
                        item.FontWeightbody = "normal";
                        item.ForegroundMessageBody = "#787878";
                        item.BackgroundConversationHeader = item.Area == "Message" ? "#F2F2F2" : "#F5F5F5";
                        item.ForegroundMessageParticipants = "#282E30";
                    }
                }
            }
        });
    };
    SocialInboxMessageComponent.prototype.DeleteConversationButtonClick = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this Message?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.BusyIndicatorText = "Saving...";
                _this.ShowBusyIndicator = true;
                _this.conversationHeaderParticipantExtendedPMService.DeleteConversationHeaderParticipant(item.ConversationHeaderId, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
                    var pmResponse = res;
                    _this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError) {
                        _this.RemoveConversationHeader(item);
                    }
                });
            }
        });
    };
    SocialInboxMessageComponent.prototype.RemoveConversationHeader = function (item) {
        if (item != null) {
            var index = this.ConversationHeaderLists.indexOf(item);
            if (index > -1) {
                this.ConversationHeaderLists.splice(index, 1);
                this.ConversationHeader = this.ConversationHeaderLists[0];
                this.IsChangeConversationHeaderMessageComponent = !this.IsChangeConversationHeaderMessageComponent;
                this.SocialMessagesComponent.RemoveConversationHeaderFromInBox(item);
                if (item.EntityPM.NumberUnreadComment > 0) {
                    this.IsChange = true;
                }
            }
        }
    };
    SocialInboxMessageComponent.prototype.RefreshButtonClick = function () {
        this.LoadingMessageList(this.InBoxMessageQuerySelectedValue);
    };
    SocialInboxMessageComponent.prototype.WaitingForResponse = function (item) {
        var _this = this;
        this.IsChange = true;
        if (!this.IsStartWaitingLoading) {
            this.IsStartWaitingLoading = true;
            this.BusyIndicatorText = "Saving...";
            this.ShowBusyIndicator = true;
            this.IsStartWaitingLoading = true;
            if (item.EntityPM.IsWaitingForResponse) {
                item.EntityPM.IsWaitingForResponse = false;
            }
            else {
                item.EntityPM.IsWaitingForResponse = true;
            }
            this.conversationHeaderPMService.update(item.EntityPM).subscribe(function (res) {
                var pmResponse = res;
                _this.IsStartWaitingLoading = false;
                if (item.EntityPM.IsWaitingForResponse)
                    item.VisibilityIsWaitingForResponse = true;
                else
                    item.VisibilityIsWaitingForResponse = false;
                if (_this.InBoxMessageQuerySelectedValue == "Waiting for response") {
                    _this.ConversationHeaderLists = _this.ConversationHeaderLists.filter(function (d) { return d.ConversationHeaderId != item.ConversationHeaderId; });
                    if (_this.ConversationHeaderLists.length == 0)
                        _this.IsNoData = true;
                }
                _this.ShowBusyIndicator = false;
            });
        }
    };
    SocialInboxMessageComponent.prototype.ScrolToTop = function () {
        var htmlid = HTMLID(this.MessageListsId);
        htmlid.scrollTop(0);
    };
    SocialInboxMessageComponent.prototype.LoadingMessageList = function (queryName) {
        if (queryName === void 0) { queryName = "All"; }
        this.MessageFilters.QueryName = queryName;
        this.ConversationHeaderPMLists = [];
        this.ConversationHeaderLists = [];
        this.LoadedMessagesCount = 0;
        this.PageIndex = 0;
        var NumberofRow = Number(window.innerHeight / 50).toString();
        var size = 10;
        if (NumberofRow.indexOf(".") > -1) {
            NumberofRow = NumberofRow.split(".")[0];
            size = Number(NumberofRow) + 1;
        }
        else {
            size = Number(NumberofRow);
        }
        this.PageSize = size < 10 ? 10 : size;
        this.IsNoResult = false;
        this.ScrolToTop();
        this.LoadData();
    };
    SocialInboxMessageComponent.prototype.LoadData = function () {
        var _this = this;
        if (!this.IsLoadRun) {
            this.IsLoadRun = true;
            this.IsNoData = false;
            this.BusyIndicatorText = "Loading...";
            this.ShowBusyIndicator = true;
            this.MessageFilters.PageIndex = this.PageIndex;
            this.MessageFilters.PageSize = this.PageSize;
            this.conversationHeaderExtendedPMService.GetMessageByFiltered(this.MessageFilters).subscribe(function (res) {
                var pmResponse = res;
                _this.IsLoadedMessages = true;
                _this.StopBusyIndicator();
                _this.IsLoadRun = false;
                _this.IsNoResult = true;
                if (!pmResponse.HasError && pmResponse.Result) {
                    _this.LoadedMessagesCount += pmResponse.Result.length;
                    if (pmResponse.Result.length > 0 && pmResponse.Result.length == _this.PageSize)
                        _this.IsNoResult = false;
                    pmResponse.Result.forEach(function (item) {
                        _this.ConversationHeaderPMLists.push(item);
                    });
                    pmResponse.Result.forEach(function (item) {
                        _this.ConversationHeaderLists.push(new SocialMessagesComponent_1.ConversationHeaderViewModelData(item, _this.InsideEntity, "Inbox"));
                    });
                    _this.ConversationHeader = _this.ConversationHeaderLists[0];
                    _this.IsChangeConversationHeaderMessageComponent = !_this.IsChangeConversationHeaderMessageComponent;
                }
                else {
                    _this.IsLoadedMessages = true;
                    _this.StopBusyIndicator();
                }
                if (_this.ConversationHeaderLists.length == 0)
                    _this.IsNoData = true;
            });
        }
    };
    SocialInboxMessageComponent.prototype.onScroll = function () {
        var element = document.getElementById(this.MessageListsId);
        if (element != null) {
            var height = Number(element.style.height.replace("px", ""));
            if ((element.scrollHeight - element.scrollTop) == element.clientHeight) {
                if (!this.IsNoResult && !this.IsLoadRun) {
                    this.PageIndex = this.LoadedMessagesCount;
                    this.PageSize = 10;
                    this.LoadData();
                }
            }
        }
    };
    SocialInboxMessageComponent.prototype.StopBusyIndicator = function () {
        if (this.IsLoadedMessages) {
            this.ShowBusyIndicator = false;
        }
    };
    SocialInboxMessageComponent.prototype.OnmMouseleave = function (item) {
        this.ConversationHeaderLists.forEach(function (item) {
            item.VisibilityIsNotWaitingForResponse = false;
        });
    };
    SocialInboxMessageComponent.prototype.OnmMouseOver = function (item) {
        if (item.EntityPM.IsWaitingForResponse == true && item.EntityPM.CreatedByUserId == SessionLocator_1.SessionLocator.LoggedUserId) {
            item.VisibilityIsNotWaitingForResponse = false;
        }
        else if (item.EntityPM.CreatedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
            item.VisibilityIsNotWaitingForResponse = false;
        }
        else
            item.VisibilityIsNotWaitingForResponse = true;
        this.ConversationHeaderLists.forEach(function (item) {
            item.IsShowUnreadadndDeleteMessageArea = false;
        });
        if (item != this.ConversationHeader) {
            item.IsShowUnreadadndDeleteMessageArea = true;
        }
    };
    SocialInboxMessageComponent.prototype.NewMessageButtonClick = function () {
        this.IsChange = true;
        if (this.SocialMessagesComponent)
            this.SocialMessagesComponent.NewMessageButtonClick(this.PostsArgs);
    };
    SocialInboxMessageComponent.prototype.OnSelectMessage = function (item) {
        if (item != this.ConversationHeader) {
            this.ConversationHeader = item;
            this.IsChangeConversationHeaderMessageComponent = !this.IsChangeConversationHeaderMessageComponent;
            item.IsShowUnreadadndDeleteMessageArea = false;
        }
    };
    SocialInboxMessageComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialInboxMessageComponent',
            templateUrl: './SocialInboxMessageComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SocialInboxMessageComponent);
    return SocialInboxMessageComponent;
}());
exports.SocialInboxMessageComponent = SocialInboxMessageComponent;
//# sourceMappingURL=SocialInboxMessageComponent.js.map