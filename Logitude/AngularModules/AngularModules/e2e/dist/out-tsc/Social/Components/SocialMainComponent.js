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
var LocationDirective_1 = require("../../Infrastructure/Utilities/LocationDirective");
var PostsArgs_1 = require("../../Infrastructure/DataContracts/PostsArgs");
var ConversationHeaderExtendedPMService_1 = require("../Services/ExtendedPMs/ConversationHeaderExtendedPMService");
var Tools_1 = require("../../Infrastructure/Tools");
var SocialMainComponent = /** @class */ (function () {
    function SocialMainComponent() {
        this.SocialMessagesCountRefreshEvent = null;
        this.MessageTabTitle = "Message";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Retries = 0;
        this.Page_Message = null;
        this.Page_Post = null;
        this.conversationHeaderExtendedPMService = new ConversationHeaderExtendedPMService_1.ConversationHeaderExtendedPMService();
        this.Listen();
    }
    SocialMainComponent.prototype.ngOnInit = function () {
    };
    SocialMainComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.SocialMessagesCountRefreshEvent) {
            this.SocialMessagesCountRefreshEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "SociaMessagesCountRefresh") {
                    _this.GetCountUnReadMassage();
                }
            });
        }
    };
    SocialMainComponent.prototype.ngOnDestroy = function () {
        if (this.SocialMessagesCountRefreshEvent) {
            this.SocialMessagesCountRefreshEvent.unsubscribe();
            this.SocialMessagesCountRefreshEvent = null;
        }
    };
    SocialMainComponent.prototype.InitializeSocialMainComponent = function (args) {
        this.PostsArgs = args;
        if (!this.PostsArgs) {
            var postsArgs = new PostsArgs_1.PostsArgs();
            postsArgs.QueryName = "Following";
            postsArgs.SubQueryName = "All";
            postsArgs.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
            postsArgs.ScreenCode = "PostControl";
            this.PostsArgs = postsArgs;
        }
        this.PostsArgs.EntityId = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.EntityId) ? "" : this.PostsArgs.EntityId;
        this.PostsArgs.AreaMessage = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.AreaMessage) ? "" : this.PostsArgs.AreaMessage;
        this.PostsArgs.EntityDescription = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.EntityDescription) ? "" : this.PostsArgs.EntityDescription;
        this.PostsArgs.ObjectTableId = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.ObjectTableId) ? "" : this.PostsArgs.ObjectTableId;
        this.PostsArgs.QueryName = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.QueryName) ? "" : this.PostsArgs.QueryName;
        this.PostsArgs.RegardingEntity = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.RegardingEntity) ? "" : this.PostsArgs.RegardingEntity;
        this.PostsArgs.ScreenCode = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.ScreenCode) ? "" : this.PostsArgs.ScreenCode;
        this.PostsArgs.SubQueryName = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.SubQueryName) ? "" : this.PostsArgs.SubQueryName;
        this.PostsArgs.UserId = Tools_1.AppTool.IsNullOrEmpty(this.PostsArgs.UserId) ? SessionLocator_1.SessionLocator.LoggedUserId : this.PostsArgs.UserId;
        this.GetCountUnReadMassage();
        this.LoadLoggedContactMessageInfo();
    };
    SocialMainComponent.prototype.LoadLoggedContactMessageInfo = function () {
        var _this = this;
        this.conversationHeaderExtendedPMService.GetLoggedContactMessageInfo(SessionLocator_1.SessionLocator.LoggedUserPM.Id, SessionLocator_1.SessionLocator.LoggedUserPM.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                if (!Tools_1.AppTool.IsNullOrEmpty(pmResponse.Result)) {
                    _this.PostsArgs.LoggedContactDefaultColor = pmResponse.Result.split('@')[0];
                    _this.PostsArgs.LoggedContactImageDetailId = pmResponse.Result.split('@')[1];
                }
            }
            _this.RunComponent();
        });
    };
    SocialMainComponent.prototype.SetSelectedItem = function () {
        this.SelectedTabCode = "SOP";
    };
    SocialMainComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.SetSelectedItem();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    SocialMainComponent.prototype.GetCountUnReadMassage = function () {
        var _this = this;
        if (this.PostsArgs) {
            this.conversationHeaderExtendedPMService.GetCountUnReadConversationHeaderPMs(this.PostsArgs.UserId, this.PostsArgs.EntityId, this.PostsArgs.ObjectTableId, this.PostsArgs.AreaMessage).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError && (pmResponse.Result || pmResponse.Result == 0)) {
                    _this.MessageTabTitle = "Message (" + pmResponse.Result.toString() + ")";
                }
            });
        }
    };
    SocialMainComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(SocialMainComponent.prototype, "SelectedTabCode", {
        get: function () { return this.selectedTabCode; },
        set: function (newValue) {
            if (this.selectedTabCode != newValue) {
                this.selectedTabCode = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    SocialMainComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.selectedTabCode != null) {
                var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.selectedTabCode; })[0];
                if (myLocation != null) {
                    switch (this.selectedTabCode) {
                        //Post
                        case "SOP": {
                            if (this.Page_Post == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Social/Components/SocialPostsComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_Post = cmpRef.instance;
                                    _this.Page_Post.InitializePostComponent(_this.PostsArgs);
                                });
                            }
                            break;
                        }
                        //Message
                        case "SOM": {
                            if (this.Page_Message == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Social/Components/SocialMessagesComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_Message = cmpRef.instance;
                                    _this.Page_Message.InitializeMessageComponent(_this.PostsArgs);
                                });
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    SocialMainComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], SocialMainComponent.prototype, "AllLocations", void 0);
    SocialMainComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialMainComponent',
            templateUrl: './SocialMainComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SocialMainComponent);
    return SocialMainComponent;
}());
exports.SocialMainComponent = SocialMainComponent;
//# sourceMappingURL=SocialMainComponent.js.map