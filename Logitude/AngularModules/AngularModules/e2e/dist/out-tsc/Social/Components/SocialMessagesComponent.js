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
var ConversationHeaderExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderExtendedPMService");
var MessageFilters_1 = require("../DataContracts/MessageFilters");
var Tools_1 = require("../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var ConversationHeaderPMService_1 = require("../Services/StandardPMs/ConversationHeaderPMService");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var SocialMessagesComponent = /** @class */ (function () {
    function SocialMessagesComponent() {
        this.ShowBusyIndicator = false;
        this.BusyIndicatorText = "";
        this.IsViewMessage = false;
        this.SocialMessageRefreshEvent = null;
        this.RefreshSocialContactLogo = null;
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
        this.conversationHeaderExtendedPMService = new ConversationHeaderExtendedPMService_1.ConversationHeaderExtendedPMService();
        this.conversationHeaderPMService = new ConversationHeaderPMService_1.ConversationHeaderPMService();
        this.SocialMessagesComponent = this;
        this.Listen();
    }
    SocialMessagesComponent.prototype.ngOnInit = function () {
    };
    SocialMessagesComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.SocialMessageRefreshEvent) {
            this.SocialMessageRefreshEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "SocialMessagesRefresh") {
                    _this.LoadingMessageList();
                }
            });
        }
        if (!this.RefreshSocialContactLogo) {
            this.RefreshSocialContactLogo = this.CurrentSession.SessionEvent.subscribe(function (arg) {
                if (arg) {
                    if (arg.Name == "RefreshSocialLogo") {
                        _this.PostsArgs.LoggedContactImageDetailId = arg.ImageId;
                    }
                }
            });
        }
    };
    SocialMessagesComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SocialMessageRefreshEvent);
        Tools_1.AppTool.KillEventEmitter(this.RefreshSocialContactLogo);
    };
    SocialMessagesComponent.prototype.InitializeMessageComponent = function (args) {
        this.PostsArgs = args;
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
            this.MessageFilters.AreaMessage = "Message";
            this.ScreenCode = args.ScreenCode;
            this.LoadingMessageList();
        }
    };
    SocialMessagesComponent.prototype.ScrolToTop = function () {
        var htmlid = HTMLID(this.MessageListsId);
        htmlid.scrollTop(0);
    };
    SocialMessagesComponent.prototype.LoadingMessageList = function () {
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
    SocialMessagesComponent.prototype.LoadData = function () {
        var _this = this;
        if (!this.IsLoadRun) {
            this.IsLoadRun = true;
            this.IsNoData = false;
            //this.CurrentSession.StartBusyIndicator("Loading...");
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
                        _this.ConversationHeaderLists.push(new ConversationHeaderViewModelData(item, _this.InsideEntity));
                    });
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
    SocialMessagesComponent.prototype.OnSelectMessage = function (item) {
        this.ConversationHeader = item;
        this.IsViewMessage = true;
    };
    SocialMessagesComponent.prototype.onScroll = function () {
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
    SocialMessagesComponent.prototype.StopBusyIndicator = function () {
        if (this.IsLoadedMessages) {
            //this.CurrentSession.StopBusyIndicator();
            this.ShowBusyIndicator = false;
        }
    };
    SocialMessagesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SocialMessagesComponent.prototype.OnmMouseleave = function (item) {
        this.ConversationHeaderLists.forEach(function (item) {
            item.VisibilityIsNotWaitingForResponse = false;
        });
    };
    SocialMessagesComponent.prototype.OnmMouseOver = function (item) {
        if (item.EntityPM.IsWaitingForResponse == true && item.EntityPM.CreatedByUserId == SessionLocator_1.SessionLocator.LoggedUserId) {
            item.VisibilityIsNotWaitingForResponse = false;
        }
        else if (item.EntityPM.CreatedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
            item.VisibilityIsNotWaitingForResponse = false;
        }
        else
            item.VisibilityIsNotWaitingForResponse = true;
    };
    SocialMessagesComponent.prototype.WaitingForResponse = function (item) {
        var _this = this;
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
                _this.ShowBusyIndicator = false;
            });
        }
    };
    SocialMessagesComponent.prototype.NewMessageButtonClick = function (postsArgs) {
        if (postsArgs === void 0) { postsArgs = null; }
        var areaMessage = "Inbox";
        if (!postsArgs) {
            postsArgs = this.PostsArgs;
            areaMessage = "Message";
        }
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.PostsArgs = postsArgs;
        windowArgs.AreaMessage = areaMessage;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 550;
        logWindow.Height = 390;
        logWindow.Title = "New Message";
        logWindow.Show("./Social/Components/NewSocialMessageComponent");
    };
    SocialMessagesComponent.prototype.GotoInboxButtonClick = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./Social/Components/SocialInboxMessageComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.InitializeMessageComponent(_this.PostsArgs, _this);
        });
    };
    SocialMessagesComponent.prototype.RefreshButtonClick = function () {
        this.LoadingMessageList();
        this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
    };
    SocialMessagesComponent.prototype.RemoveConversationHeader = function (item) {
        if (item != null) {
            var index = this.ConversationHeaderLists.indexOf(item);
            if (index > -1) {
                this.ConversationHeaderLists.splice(index, 1);
            }
        }
    };
    SocialMessagesComponent.prototype.RemoveConversationHeaderFromInBox = function (item) {
        var conversationHeader = this.ConversationHeaderLists.filter(function (d) { return d.ConversationHeaderId == item.ConversationHeaderId; })[0];
        if (conversationHeader) {
            this.RemoveConversationHeader(conversationHeader);
        }
    };
    SocialMessagesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialMessagesComponent',
            templateUrl: './SocialMessagesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SocialMessagesComponent);
    return SocialMessagesComponent;
}());
exports.SocialMessagesComponent = SocialMessagesComponent;
var ConversationHeaderViewModelData = /** @class */ (function () {
    function ConversationHeaderViewModelData(entityPM, insideEntity, area) {
        if (area === void 0) { area = "Message"; }
        this.MessageBody = "";
        this.BackgroundConversationHeader = "";
        this.DisplayEntityInformation = false;
        this.ObjectTableName = "";
        this.ForegroundMessageParticipants = "";
        this.VisibilityIsWaitingForResponse = false;
        this.VisibilityIsNotWaitingForResponse = false;
        this.FontWeightbody = "";
        this.IsShowNumberUnreadComment = false;
        this.IsShowArrowSend = false;
        this.IsShowArrowReceiveMessage = false;
        this.IsShowUnreadadndDeleteMessageArea = false;
        this.WaitingForResponse = false;
        this.Area = "";
        this.IsChange = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.markAsReadLable = "Mark as Unread";
        this.EntityPM = entityPM;
        this.MessageBody = entityPM.LasMessageBody;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.ConversationHeaderId = entityPM.Id;
        this.Area = area;
        if (entityPM.MessageParticipantsCount >= 3) {
            if (entityPM.LasMessageUserName != null) {
                if (entityPM.LasMessageUserId == SessionLocator_1.SessionLocator.LoggedUserId) {
                    this.LasMessageUserName = "";
                }
                else {
                    this.LasMessageUserName = entityPM.LasMessageUserName + ": ";
                }
            }
        }
        else
            this.LasMessageUserName = "";
        this.NumberUnreadComment = entityPM.NumberUnreadComment ? entityPM.NumberUnreadComment.toString() : "";
        if (entityPM.NumberUnreadComment > 0) {
            this.BackgroundConversationHeader = this.Area == "Message" ? "#ffffff" : "#F5F5F5";
            this.ForegroundMessageParticipants = "#000000";
            this.FontWeightbody = "bold";
            this.IsShowNumberUnreadComment = true;
        }
        else {
            this.BackgroundConversationHeader = this.Area == "Message" ? "#F2F2F2" : "#F5F5F5";
            this.ForegroundMessageParticipants = "#282E30";
            this.FontWeightbody = "normal";
        }
        if (entityPM.CreatedByUserId == SessionLocator_1.SessionLocator.LoggedUserId)
            this.WaitingForResponse = true;
        if (entityPM.IsWaitingForResponse == true && entityPM.CreatedByUserId == SessionLocator_1.SessionLocator.LoggedUserId)
            this.VisibilityIsWaitingForResponse = true;
        else
            this.VisibilityIsWaitingForResponse = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EntityId) && (!insideEntity || area == "Inbox")) {
            this.DisplayEntityInformation = true;
        }
        this.CreateDate = this.EntityPM.LastMessageDate;
        this.EntityDescription = this.EntityPM.EntityDescription;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(function (d) { return d.Id == entityPM.ObjectTableId; })[0];
            this.ObjectTableName = table.Name;
            this.ObjectTableId = table.Id;
        }
        if (entityPM.MessageParticipantsCount >= 3) {
            this.MessageParticipants = entityPM.MessageParticipants;
        }
        else {
            var ParticipantsName = [];
            if (entityPM.MessageParticipantsCount < 3) {
                ParticipantsName = entityPM.MessageParticipants.split(',');
                if (!Tools_1.AppTool.IsNullOrEmpty(ParticipantsName[0])) {
                    if (ParticipantsName[0] == SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(ParticipantsName[1])) {
                            this.MessageParticipants = ParticipantsName[1];
                        }
                    }
                    else {
                        this.MessageParticipants = ParticipantsName[0];
                    }
                }
            }
        }
        if (entityPM.MessageParticipantsCount != null) {
            if (entityPM.MessageParticipantsCount > 4) {
                var countothers = entityPM.MessageParticipantsCount - 3;
                this.MessageOthers = "+ " + countothers.toString() + " more";
            }
        }
        if (entityPM.NumberUnreadComment > 0) {
            this.ForegroundMessageBody = "#000000";
        }
        else {
            this.ForegroundMessageBody = "#787878";
        }
        //ShowArrowReceiveMessage
        if (entityPM.CreatedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
            if (entityPM.LasMessageUserId != entityPM.CreatedByUserId)
                this.IsShowArrowReceiveMessage = true;
            else
                this.IsShowArrowReceiveMessage = false;
        }
        else
            this.IsShowArrowReceiveMessage = false;
        //ShowArrowSend
        if (entityPM.LasMessageUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
            if (entityPM.NumberUnreadComment > 0)
                this.IsShowArrowSend = true;
            else
                this.IsShowArrowSend = false;
        }
        else
            this.IsShowArrowSend = false;
        this.SetImageEntity(this);
    }
    Object.defineProperty(ConversationHeaderViewModelData.prototype, "MarkAsReadLable", {
        get: function () {
            if (this.EntityPM) {
                if (this.EntityPM.NumberUnreadComment > 0) {
                    this.markAsReadLable = "Mark as Read";
                }
                else {
                    this.markAsReadLable = "Mark as Unread";
                }
            }
            return this.markAsReadLable;
        },
        set: function (newValue) {
            if (this.markAsReadLable != newValue) {
                this.markAsReadLable = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ConversationHeaderViewModelData.prototype.SetImageEntity = function (conversationHeaderViewModelData) {
        var entityImageSource = "";
        var entityPM = conversationHeaderViewModelData.EntityPM;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.EntityId) && !Tools_1.AppTool.IsNullOrEmpty(entityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(function (d) { return d.Id == entityPM.ObjectTableId; })[0];
            switch (table.Name) {
                case "Customer":
                    entityImageSource = "./Images/Maintenance/Customer.png";
                    break;
                case "Opportunity":
                    entityImageSource = "./Images/Opportunity.png";
                    break;
                case "Activity":
                    entityImageSource = "./Images/Activities/AP.png";
                    if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.EntityDescription)) {
                        var descr = entityPM.EntityDescription.split('-');
                        if (descr.length > 1) {
                            switch (descr[0]) {
                                case "AP":
                                    {
                                        entityImageSource = "./Images/Activities/AP.png";
                                        break;
                                    }
                                case "CL":
                                    {
                                        entityImageSource = "./Images/Activities/CL.png";
                                        break;
                                    }
                                case "VM":
                                    {
                                        entityImageSource = "./Images/Buttons/VM.png";
                                        break;
                                    }
                                case "TS":
                                    {
                                        entityImageSource = "./Images/Activities/TS.png";
                                        break;
                                    }
                                case "EO":
                                    {
                                        entityImageSource = "./Images/Activities/EO.png";
                                        break;
                                    }
                                case "EI":
                                    {
                                        entityImageSource = "./Images/Buttons/EI.png";
                                        break;
                                    }
                            }
                        }
                    }
                    break;
                default:
                    entityImageSource = null;
                    break;
            }
            ;
        }
        conversationHeaderViewModelData.EntityImageSource = entityImageSource;
    };
    ConversationHeaderViewModelData.prototype.ViewMessageOthers = function (conversationHeaderViewModelData) {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.ConversationHeader = conversationHeaderViewModelData;
        //windowArgs.Area = conversationHeaderViewModelData.Area;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 550;
        logWindow.Height = 390;
        logWindow.Title = "Participants";
        logWindow.Show("./Social/Components/SocialMessageParticipantsComponent");
    };
    ConversationHeaderViewModelData.prototype.ViewEntityButtonClick = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.EntityId) && !Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(function (d) { return d.Id == item.EntityPM.ObjectTableId; })[0];
            if (table) {
                this._entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(function (response) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: item.EntityPM.EntityId, ObjectTableName: table.Name });
                    });
                });
            }
        }
    };
    return ConversationHeaderViewModelData;
}());
exports.ConversationHeaderViewModelData = ConversationHeaderViewModelData;
//# sourceMappingURL=SocialMessagesComponent.js.map