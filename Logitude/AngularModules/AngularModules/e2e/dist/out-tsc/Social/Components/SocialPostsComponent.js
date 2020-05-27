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
var PostExtendedPMService_1 = require("../Services/ExtendedPMs/PostExtendedPMService");
var PostFilters_1 = require("../DataContracts/PostFilters");
var PostsArgs_1 = require("../../Infrastructure/DataContracts/PostsArgs");
var Tools_1 = require("../../Infrastructure/Tools");
var MessageWindow_1 = require("../../Controls/Windows/MessageWindow");
var PostPMService_1 = require("../Services/StandardPMs/PostPMService");
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var ContactPMService_1 = require("../../Common/Services/StandardPMs/ContactPMService");
var PostPM_1 = require("../EntityPMs/PostPM");
var PostLikePM_1 = require("../EntityPMs/PostLikePM");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var SocialPostsComponent = /** @class */ (function () {
    function SocialPostsComponent(_entityResourceService, cd) {
        this._entityResourceService = _entityResourceService;
        this.cd = cd;
        this.PointerEventsInPutPost = "auto";
        this.OpacityAreaInPutPost = "1";
        this.IsShowAreaPost = true;
        this.IsChange = false;
        this.IsShowButtonNewPost = false;
        this.AreaPostHeight = "90px";
        this.MessagePost = "What are you working on?";
        this.SubQueryName = "All Posts";
        this.HeightInPutPost = "30px";
        this.postFilters = new PostFilters_1.PostFilters();
        this.PostsLists = [];
        this.PostQueryHeaderLists = [];
        this.UserId = "";
        this.ImageId = "";
        this.SocialPostsRefreshEvent = null;
        this.PostLableVisibility = true;
        this.IsEntityMode = false;
        this.IsUserMode = false;
        this.ScreenCode = "";
        this.InsideEntity = false;
        this.IsLoggedUser = false;
        this.IsShowUserImage = false;
        this.IsChangeUserLogo = false;
        this.IsRefreshImage = false;
        this.PostListsId = Guid_1.Guid.newGuid();
        this.PageIndex = 0;
        this.PageSize = 0;
        this.LoadedPostCount = 0;
        this.IsShowPeopleComponent = false;
        this.IsLoadRun = false;
        this.IsNoResult = false;
        this.HideLeftArea = false;
        this.IsLoadedSocialContact = false;
        this.IsLoadedLoggedContact = false;
        this.IsLoadedSocial = false;
        this.ShowBusyIndicator = false;
        this.BusyIndicatorText = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LoggedContactDefaultColor = "";
        this.IsFollower = false;
        this.Retries = 0;
        this.postExtendedPMService = new PostExtendedPMService_1.PostExtendedPMService();
        this.contactPMService = new ContactPMService_1.ContactPMService();
        this.postPMService = new PostPMService_1.PostPMService();
        this.PostQueryHeaderLists = [];
        this.PostQueryHeaderLists.push(new PostQueryHeaderClass("Feed"));
        this.PostQueryHeaderLists.push(new PostQueryHeaderClass("People"));
        this.Listen();
    }
    SocialPostsComponent.prototype.ngOnInit = function () {
    };
    SocialPostsComponent.prototype.Listen = function () {
        var _this = this;
        if (!this.SocialPostsRefreshEvent) {
            this.SocialPostsRefreshEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "SocialPostsRefresh") {
                    _this.LoadingSocialList();
                }
            });
        }
    };
    SocialPostsComponent.prototype.InitializePostComponent = function (args) {
        this.PostsArgs = args;
        this.LoggedContactDefaultColor = this.PostsArgs.LoggedContactDefaultColor;
        if (args.IsEntityMode || args.ScreenCode == "CRM") {
            this.PostLableVisibility = true;
        }
        this.IsEntityMode = args.IsEntityMode;
        this.IsUserMode = args.IsUserMode;
        this.ScreenCode = args.ScreenCode;
        this.InsideEntity = args.InsideEntity;
        this.HideLeftArea = args.HideLeftArea;
        if (Tools_1.AppTool.IsNullOrEmpty(args.UserId)) {
            args.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
        this.UserId = args.UserId;
        if (SessionLocator_1.SessionLocator.LoggedUserId == this.UserId)
            this.IsLoggedUser = true;
        this.LoadLoggedContact();
        if (Tools_1.AppTool.IsNullOrEmpty(args.SubQueryName)) {
            args.SubQueryName = "All";
            if (args.ScreenCode == "CRM" || args.ScreenCode == "Quote" || args.ScreenCode == "Customer" || args.ScreenCode == "Opportunity") {
                args.SubQueryName = "User";
            }
        }
        this.SelectedMainMenu = this.PostQueryHeaderLists[0].PostQueryLineLists.filter(function (d) { return d.Code == args.QueryName; })[0];
        if (args.SubQueryName == "All")
            this.SubQueryName = "All Posts";
        else if (args.SubQueryName == "User")
            this.SubQueryName = "User Posts";
        else if (args.SubQueryName == "Auto")
            this.SubQueryName = "Auto Posts";
        if (this.IsUserMode) {
            this.GetPostSummaryData();
        }
        this.postFilters = new PostFilters_1.PostFilters();
        this.postFilters.PageIndex = 0;
        this.postFilters.PageSize = 10;
        this.postFilters.QueryName = args.QueryName;
        this.postFilters.SubQueryName = args.SubQueryName;
        this.postFilters.UserId = args.UserId;
        this.postFilters.EntityId = !Tools_1.AppTool.IsNullOrEmpty(args.EntityId) ? args.EntityId : null;
        this.postFilters.ObjectTableId = !Tools_1.AppTool.IsNullOrEmpty(args.ObjectTableId) ? args.ObjectTableId : null;
        this.postFilters.EntityDescription = !Tools_1.AppTool.IsNullOrEmpty(args.EntityDescription) ? args.EntityDescription : null;
        //this.postFilters.RegardingEntity = !AppTool.IsNullOrEmpty(args.RegardingEntity) ? args.RegardingEntity : null;
        this.postFilters.SearchByEntity = args.InsideEntity;
        this.LoadingSocialList();
    };
    SocialPostsComponent.prototype.LoadingSocialList = function () {
        this.PostsPMLists = [];
        this.PostsLists = [];
        this.LoadedPostCount = 0;
        this.PageIndex = 0;
        var NumberofRow = Number(window.innerHeight / 82).toString();
        var size = 10;
        if (NumberofRow.indexOf(".") > -1) {
            NumberofRow = NumberofRow.split(".")[0];
            size = Number(NumberofRow) + 1;
        }
        else {
            size = Number(NumberofRow);
        }
        this.PageSize = (size - 1) < 10 ? 10 : (size - 1);
        this.IsNoResult = false;
        this.ScrolToTop();
        this.LoadData();
    };
    SocialPostsComponent.prototype.LoadSocialContact = function () {
        var _this = this;
        this.postExtendedPMService.GetSocialContact(this.UserId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.IsLoadedSocialContact = true;
            _this.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.SocialContactPM = pmResponse.Result;
                if (_this.SocialContactPM) {
                    _this.UserViewName = _this.SocialContactPM.EnglishName;
                    _this.ImageId = _this.SocialContactPM != null ? _this.SocialContactPM.ImageDetailId : "";
                }
            }
            _this.IsShowUserImage = true;
        });
    };
    SocialPostsComponent.prototype.LoadLoggedContact = function () {
        var _this = this;
        if (!this.IsLoggedUser) {
            this.LoadSocialContact();
        }
        else {
            this.IsLoadedSocialContact = true;
        }
        if (this.PostsArgs.TiggerViewModel && this.PostsArgs.TiggerViewModel.LoggedContactPM) {
            this.IsLoadedLoggedContact = true;
            this.StopBusyIndicator();
            this.LoggedContactPM = this.PostsArgs.TiggerViewModel.LoggedContactPM;
            if (this.IsLoggedUser) {
                this.SocialContactPM = this.LoggedContactPM;
                if (this.SocialContactPM) {
                    this.UserViewName = this.SocialContactPM.EnglishName;
                    this.ImageId = this.SocialContactPM != null ? this.SocialContactPM.ImageDetailId : "";
                }
            }
            if (this.IsLoggedUser) {
                this.IsShowUserImage = true;
            }
        }
        else {
            this.postExtendedPMService.GetSocialContact(SessionLocator_1.SessionLocator.LoggedUserPM.Id, SessionLocator_1.SessionLocator.LoggedUserPM.Tenant).subscribe(function (res) {
                var pmResponse = res;
                _this.IsLoadedLoggedContact = true;
                _this.StopBusyIndicator();
                if (!pmResponse.HasError && pmResponse.Result) {
                    _this.LoggedContactPM = pmResponse.Result;
                    if (_this.IsLoggedUser) {
                        _this.SocialContactPM = _this.LoggedContactPM;
                        if (_this.SocialContactPM) {
                            _this.UserViewName = _this.SocialContactPM.EnglishName;
                            _this.ImageId = _this.SocialContactPM != null ? _this.SocialContactPM.ImageDetailId : "";
                        }
                    }
                }
                if (_this.IsLoggedUser) {
                    _this.IsShowUserImage = true;
                }
            });
        }
    };
    SocialPostsComponent.prototype.LoadData = function () {
        var _this = this;
        if (!this.IsLoadRun) {
            this.IsLoadRun = true;
            this.IsNoData = false;
            //this.CurrentSession.StartBusyIndicator("Loading...");
            this.BusyIndicatorText = "Loading...";
            this.ShowBusyIndicator = true;
            this.postFilters.PageIndex = this.PageIndex;
            this.postFilters.PageSize = this.PageSize;
            this.postExtendedPMService.PostFilteredPosts(this.postFilters).subscribe(function (res) {
                var pmResponse = res;
                _this.IsLoadedSocial = true;
                _this.StopBusyIndicator();
                _this.IsLoadRun = false;
                _this.IsNoResult = true;
                if (!pmResponse.HasError && pmResponse.Result) {
                    if (pmResponse.Result.length > 0 && pmResponse.Result.length == _this.PageSize)
                        _this.IsNoResult = false;
                    _this.LoadedPostCount += pmResponse.Result.length;
                    pmResponse.Result.forEach(function (item) {
                        _this.PostsPMLists.push(item);
                    });
                    pmResponse.Result.forEach(function (item) {
                        _this.PostsLists.push(new PostViewModelData(item, _this, "Post"));
                    });
                }
                else {
                    _this.IsLoadedSocial = true;
                    _this.StopBusyIndicator();
                }
                if (_this.PostsLists.length == 0)
                    _this.IsNoData = true;
            });
        }
    };
    SocialPostsComponent.prototype.GetPostSummaryData = function () {
        var _this = this;
        this.postExtendedPMService.GetPostSummaryData(this.UserId, SessionLocator_1.SessionLocator.LoggedUserId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.CoundFollowerUser = pmResponse.Result.CoundFollowerUser;
                _this.Coundlikesreceived = pmResponse.Result.Coundlikesreceived;
                _this.IsFollower = pmResponse.Result.IsFollower;
                if (!_this.IsFollower) {
                    //this.OpacityAreaInPutPost = "0.5";
                    // this.PointerEventsInPutPost = "none";
                    //this.IsShowAreaPost = false;
                }
            }
        });
    };
    SocialPostsComponent.prototype.BuildItemsSource = function () {
        var _this = this;
        this.PostsLists = [];
        this.PostsPMLists = this.PostsPMLists.sort(function (a, b) { return (a.UpdateDate > b.UpdateDate) ? -1 : ((a.UpdateDate < b.UpdateDate) ? 1 : 0); });
        this.PostsPMLists.forEach(function (item) {
            _this.PostsLists.push(new PostViewModelData(item, _this, "Post"));
        });
        this.IsNoData = false;
        if (this.PostsLists.length == 0) {
            this.IsNoData = true;
        }
    };
    SocialPostsComponent.prototype.RefreshMainLog = function (imageId) {
        var _this = this;
        this.ImageId = imageId;
        if (this.LoggedContactPM) {
            this.LoggedContactPM.ImageDetailId = imageId;
        }
        this.IsRefreshImage = !this.IsRefreshImage;
        this.PostsPMLists.filter(function (d) { return d.CreatedById == SessionLocator_1.SessionLocator.LoggedUserId || (d.PostComments.length > 0 && d.PostComments.filter(function (d) { return d.CreatedById == SessionLocator_1.SessionLocator.LoggedUserId; })[0] != null); }).forEach(function (post) {
            post.UserImageDetailId = _this.ImageId;
            post.PostComments.filter(function (d) { return d.CreatedById == SessionLocator_1.SessionLocator.LoggedUserId; }).forEach(function (comment) {
                comment.UserImageDetailId = _this.ImageId;
            });
        });
        this.BuildItemsSource();
    };
    SocialPostsComponent.prototype.PostInputFocus = function () {
        if (this.MessagePost == "What are you working on?") {
            this.MessagePost = "";
            this.HeightInPutPost = "80px";
            this.AreaPostHeight = "100px";
            this.IsShowButtonNewPost = true;
        }
    };
    SocialPostsComponent.prototype.ImageUploadedCompleted = function (event) {
        var _this = this;
        this.CurrentSession.FireEvent({ Name: 'RefreshSocialLogo', ImageId: event });
        if (this.LoggedContactPM != null) {
            if (this.LoggedContactPM.ImageDetailId != event) {
                //this.CurrentSession.StartBusyIndicator("Saving...");
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;
                this.LoggedContactPM.ImageDetailId = event;
                this.ImageId = event;
                this.contactPMService.update(this.LoggedContactPM).subscribe(function (res) {
                    var pmResponse = res;
                    _this.CurrentSession.FireEvent("SocialMessagesRefresh");
                    //this.CurrentSession.StopBusyIndicator();
                    _this.ShowBusyIndicator = false;
                    _this.PostsPMLists.filter(function (d) { return d.CreatedById == SessionLocator_1.SessionLocator.LoggedUserId || (d.PostComments.length > 0 && d.PostComments.filter(function (d) { return d.CreatedById == SessionLocator_1.SessionLocator.LoggedUserId; })[0] != null); }).forEach(function (post) {
                        post.UserImageDetailId = _this.ImageId;
                        post.PostComments.filter(function (d) { return d.CreatedById == SessionLocator_1.SessionLocator.LoggedUserId; }).forEach(function (comment) {
                            comment.UserImageDetailId = _this.ImageId;
                        });
                    });
                    _this.BuildItemsSource();
                    if (_this.IsUserMode) {
                        if (_this.PostsArgs.TiggerViewModel) {
                            _this.PostsArgs.TiggerViewModel.RefreshMainLog(event);
                        }
                    }
                });
            }
        }
    };
    SocialPostsComponent.prototype.QueryPostMethod = function (selectQuerytext) {
        if (selectQuerytext == "All")
            this.SubQueryName = "All Posts";
        else if (selectQuerytext == "User")
            this.SubQueryName = "User Posts";
        else {
            if (selectQuerytext == "Auto")
                this.SubQueryName = "Auto Posts";
        }
        if (this.postFilters.SubQueryName != selectQuerytext) {
            this.postFilters.SubQueryName = selectQuerytext;
            this.LoadingSocialList();
        }
    };
    SocialPostsComponent.prototype.RefreshButtonClicked = function () {
        //this.LoadData();
        this.LoadingSocialList();
    };
    SocialPostsComponent.prototype.CreateNewPostButtonClick = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MessagePost)) {
            if (this.MessagePost.length <= 4000) {
                var entityPM = new PostPM_1.PostPM();
                entityPM.BodyText = this.MessagePost;
                entityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                entityPM.CreatedById = entityPM.CreatedById = SessionLocator_1.SessionLocator.LoggedUserId;
                entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
                entityPM.ObjectTableId = this.postFilters.ObjectTableId;
                entityPM.EntityId = this.postFilters.EntityId;
                entityPM.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                entityPM.EntityDescription = this.postFilters.EntityDescription;
                entityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                entityPM.UserImageDetailId = this.LoggedContactPM != null ? this.LoggedContactPM.ImageDetailId : "";
                entityPM.IndexColor = this.LoggedContactPM != null ? this.LoggedContactPM.IndexColor : 1;
                entityPM.DefaultColor = this.LoggedContactDefaultColor;
                this.IsChange = true;
                //this.CurrentSession.StartBusyIndicator("Saving...");
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;
                this.postPMService.insert(entityPM).subscribe(function (res) {
                    var pmResponse = res;
                    //this.CurrentSession.StopBusyIndicator();
                    _this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError && pmResponse.Result) {
                        //this.LoadData();
                        if (_this.IsLoggedUser || _this.IsFollower) {
                            _this.PostsPMLists.push(entityPM);
                            _this.BuildItemsSource();
                        }
                        _this.ScrolToTop();
                        _this.MessagePost = "What are you working on?";
                        _this.HeightInPutPost = "30px";
                        _this.AreaPostHeight = "90px";
                        _this.IsShowButtonNewPost = false;
                    }
                });
            }
            else {
                var messageWindow = new MessageWindow_1.MessageWindow();
                messageWindow.Show("Post maximum charachters should be less than 4000!");
            }
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("This Post seems to be empty. Please, write something to Post");
        }
    };
    SocialPostsComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    SocialPostsComponent.prototype.OnMainListSelectedItemChanged = function (selected) {
        if (this.SelectedMainMenu != selected) {
            this.SelectedMainMenu = selected;
            if (this.SelectedMainMenu.Code != "People") {
                this.postFilters.QueryName = this.SelectedMainMenu.Code;
                this.IsShowPeopleComponent = false;
                this.LoadingSocialList();
            }
            else {
                this.PostsLists = [];
                this.IsShowPeopleComponent = true;
                this.RunComponent();
            }
        }
    };
    SocialPostsComponent.prototype.BackButtonClicked = function () {
        if (this.ComponentRef) {
            if (this.PostsArgs.TiggerViewModel) {
                if (this.IsChange) {
                    this.PostsArgs.TiggerViewModel.RefreshButtonClicked();
                }
            }
            this.ComponentRef.destroy();
        }
    };
    SocialPostsComponent.prototype.ViewPostUserFeedsButtonClick = function (item) {
        if (!this.IsUserMode) {
            var postFilters = new PostFilters_1.PostFilters();
            var postsArgs = new PostsArgs_1.PostsArgs();
            postsArgs.QueryName = "UserPosts";
            postsArgs.SubQueryName = "All";
            postsArgs.UserId = item.EntityPM.CreatedById;
            postsArgs.ScreenCode = "UserPostsControl";
            postsArgs.IsUserMode = true;
            postsArgs.TiggerViewModel = this;
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Social/Components/SocialPostsComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.InitializePostComponent(postsArgs);
            });
        }
    };
    SocialPostsComponent.prototype.ViewEntityButtonClick = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.EntityId) && !Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(function (d) { return d.Id == item.EntityPM.ObjectTableId; })[0];
            if (table) {
                this._entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(function (response) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: item.EntityPM.EntityId, ObjectTableName: table.Name });
                        // cmpRef.instance.BackCompleted.subscribe(($event: any) => this.RefreshButtonClicked());
                    });
                });
            }
        }
    };
    SocialPostsComponent.prototype.CommentPostButtonClick = function (item) {
        item.IsShowReplyCommentArea = true;
        item.CommentInputFocus();
    };
    SocialPostsComponent.prototype.RemovePostButtonClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this post?");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                item.EntityPM.IsCancelled = true;
                _this.IsChange = true;
                //this.CurrentSession.StartBusyIndicator("Saving...");
                _this.BusyIndicatorText = "Saving...";
                _this.ShowBusyIndicator = true;
                _this.postPMService.update(item.EntityPM).subscribe(function (res) {
                    var pmResponse = res;
                    //this.CurrentSession.StopBusyIndicator();
                    _this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError && pmResponse.Result) {
                        _this.PostsPMLists = _this.PostsPMLists.filter(function (d) { return d.Id != item.EntityPM.Id; });
                        _this.BuildItemsSource();
                        //  this.LoadData();
                    }
                });
            }
        });
    };
    SocialPostsComponent.prototype.EditPostButtonClicked = function (item) {
        //EditPostComponent
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        windowArgs.PostViewModelData = item;
        this.IsChange = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 700;
        logWindow.Height = 200;
        logWindow.Title = "Edit Post";
        logWindow.Show("./Social/Components/EditPostComponent");
    };
    SocialPostsComponent.prototype.LikePostButtonClick = function (item) {
        var _this = this;
        this.IsChange = true;
        if (item.LikeLabel == "Like") {
            var postLikePM = new PostLikePM_1.PostLikePM(null);
            postLikePM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            postLikePM.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
            postLikePM.UserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            postLikePM.PostId = item.EntityPM.Id;
            if (!item.EntityPM.PostLikes.filter(function (d) { return d.PostId == item.EntityPM.Id && d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0]) {
                item.EntityPM.AddPostLike(postLikePM);
            }
        }
        else {
            var postLikePM = item.EntityPM.PostLikes.filter(function (d) { return d.PostId == item.EntityPM.Id && d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0];
            if (postLikePM != null) {
                item.EntityPM.RemovePostLike(postLikePM);
            }
        }
        //this.CurrentSession.StartBusyIndicator("Saving...");
        this.BusyIndicatorText = "Saving...";
        this.ShowBusyIndicator = true;
        this.postPMService.update(item.EntityPM).subscribe(function (res) {
            var pmResponse = res;
            //this.CurrentSession.StopBusyIndicator();
            _this.ShowBusyIndicator = false;
            if (!pmResponse.HasError && pmResponse.Result) {
                _this.RefreshLikesPost(item);
            }
        });
    };
    SocialPostsComponent.prototype.LikeCommentButtonClick = function (item) {
        var _this = this;
        this.IsChange = true;
        if (item.LikeLabel == "Like") {
            var postLikePM = new PostLikePM_1.PostLikePM(null);
            postLikePM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            postLikePM.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
            postLikePM.UserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
            postLikePM.PostId = item.EntityPM.Id;
            //this.CurrentSession.StartBusyIndicator("Saving...");
            this.BusyIndicatorText = "Saving...";
            this.ShowBusyIndicator = true;
            this.postExtendedPMService.InsertPostLike(postLikePM).subscribe(function (res) {
                var pmResponse = res;
                //this.CurrentSession.StopBusyIndicator();
                _this.ShowBusyIndicator = false;
                if (!pmResponse.HasError && pmResponse.Result) {
                    if (!item.EntityPM.PostLikes.filter(function (d) { return d.PostId == item.EntityPM.Id && d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0]) {
                        item.EntityPM.PostLikes.push(pmResponse.Result);
                    }
                    _this.PefreshCommentLike(item);
                }
            });
        }
        else {
            var postLikePM = item.EntityPM.PostLikes.filter(function (d) { return d.PostId == item.EntityPM.Id && d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0];
            if (postLikePM != null) {
                item.EntityPM.RemovePostLike(postLikePM);
            }
            //this.CurrentSession.StartBusyIndicator("Saving...");
            this.BusyIndicatorText = "Saving...";
            this.ShowBusyIndicator = true;
            this.postExtendedPMService.DeletePostLike(item.EntityPM.Id, SessionLocator_1.SessionLocator.LoggedUserId, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var pmResponse = res;
                //this.CurrentSession.StopBusyIndicator();
                _this.ShowBusyIndicator = false;
                if (!pmResponse.HasError) {
                    _this.PefreshCommentLike(item);
                }
            });
        }
    };
    SocialPostsComponent.prototype.RefreshLikesPost = function (item) {
        item.LikeLabel = "Like";
        item.DisplayTextOfLike = "";
        item.DisplayNumberOfLike = false;
        item.DisplayAreaLike = false;
        item.NumberPostLike = "";
        var index = 1;
        item.ToolTipLike = "";
        item.DisplayOtherLike = "";
        if (item.EntityPM.PostLikes.length > 0) {
            if (item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - 1].UserId == SessionLocator_1.SessionLocator.LoggedUserId) {
                index = 2;
            }
            item.NumberPostLike = item.EntityPM.PostLikes.length > 0 ? "(" + item.EntityPM.PostLikes.length.toString() + ")" : "";
            item.DisplayNumberOfLike = true;
            item.DisplayAreaLike = true;
            if (item.EntityPM.PostLikes.length == 1) {
                if (item.EntityPM.PostLikes.filter(function (d) { return d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0]) {
                    item.LikeLabel = "UnLike";
                    item.DisplayTextOfLike = "You like this";
                }
                else {
                    item.DisplayTextOfLike = item.EntityPM.PostLikes[0].UserName + " like this";
                }
            }
            else if (item.EntityPM.PostLikes.length < 5) {
                if (item.EntityPM.PostLikes.filter(function (d) { return d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0]) {
                    item.LikeLabel = "UnLike";
                    if (item.EntityPM.PostLikes.length == 2)
                        item.DisplayTextOfLike = "You and ";
                    else
                        item.DisplayTextOfLike = "You , ";
                }
                item.EntityPM.PostLikes.forEach(function (like) {
                    if (like.UserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                        if (like == item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - (index + 1)]) {
                            item.DisplayTextOfLike += like.UserName + " and ";
                        }
                        else if (like != item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - index]) {
                            item.DisplayTextOfLike += like.UserName + " , ";
                        }
                        else {
                            item.DisplayTextOfLike += (like.UserName + " like this");
                        }
                    }
                });
            }
            else {
                var count = 0;
                var isMeLike = false;
                if (item.EntityPM.PostLikes.filter(function (d) { return d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0] && item.EntityPM.PostLikes.length > 1) {
                    item.DisplayTextOfLike = "You , ";
                    item.LikeLabel = "UnLike";
                    isMeLike = true;
                }
                item.EntityPM.PostLikes.forEach(function (like) {
                    if (like.UserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                        count += 1;
                        if (count == 1) {
                            item.DisplayTextOfLike += like.UserName + " , ";
                        }
                        else if (count == 2) {
                            item.DisplayTextOfLike += like.UserName + " and ";
                        }
                        else {
                            if (like != item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - index]) {
                                item.ToolTipLike += like.UserName + " ,";
                            }
                            else
                                item.ToolTipLike += like.UserName;
                        }
                    }
                });
                var pepoleLikeNamCoun = 2;
                if (isMeLike)
                    pepoleLikeNamCoun += 1;
                item.CountOtherLike = ((item.EntityPM.PostLikes.length - pepoleLikeNamCoun).toString());
                item.DisplayOtherLike = " Others like this";
            }
        }
        else {
            item.LikeLabel = "Like";
            item.DisplayTextOfLike = "";
        }
    };
    SocialPostsComponent.prototype.PefreshCommentLike = function (item) {
        item.NumberPostLike = "";
        item.LikeLabel = "Like";
        item.ToolTipLike = "";
        if (item.EntityPM.PostLikes.length > 0) {
            item.DisplayNumberOfLike = true;
            if (item.EntityPM.PostLikes.filter(function (d) { return d.UserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0]) {
                item.LikeLabel = "UnLike";
            }
            item.NumberPostLike = " (" + item.EntityPM.PostLikes.length + ")";
            item.EntityPM.PostLikes.forEach(function (like) {
                if (like != item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - 1]) {
                    item.ToolTipLike += like.UserName + " ,";
                }
                else
                    item.ToolTipLike += like.UserName;
            });
        }
    };
    SocialPostsComponent.prototype.ScrolToTop = function () {
        var htmlid = HTMLID(this.PostListsId);
        htmlid.scrollTop(0);
    };
    SocialPostsComponent.prototype.ConvertBodyText = function (body) {
        var textarea = document.createElement("textarea");
        textarea.value = body;
        return textarea.value;
    };
    SocialPostsComponent.prototype.ngOnDestroy = function () {
        if (this.SocialPostsRefreshEvent) {
            this.SocialPostsRefreshEvent.unsubscribe();
            this.SocialPostsRefreshEvent = null;
        }
    };
    //SocialPeopelArea
    SocialPostsComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef || this.SocialPeopleViewContainerRef) {
            if (!this.SocialPeopleViewContainerRef) {
                this.SocialPeopleViewContainerRef = this.viewContainerRef;
            }
            if (this.SocialPeopleViewContainerRef) {
                this.LoadSocialPeopleComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    SocialPostsComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 100) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    SocialPostsComponent.prototype.LoadSocialPeopleComponent = function () {
        var _this = this;
        //if (!this.viewContainerRef) {
        //    this.viewContainerRef = this.SocialPeopleViewContainerRef;
        //}
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Social/Components/SocialPeopleComponent', this.SocialPeopleViewContainerRef)
            .then(function (cmpRef) {
            _this.SocialPeopleComponent = cmpRef.instance;
            //this.Page_Post.InitializePostComponent(postsArgs);
        });
    };
    //IsLoadedSocial
    SocialPostsComponent.prototype.StopBusyIndicator = function () {
        if (this.IsLoadedSocial && this.IsLoadedLoggedContact && this.IsLoadedSocialContact) {
            //this.CurrentSession.StopBusyIndicator();
            this.ShowBusyIndicator = false;
            this.MessagePost = "What are you working on?";
            this.HeightInPutPost = "30px";
            this.AreaPostHeight = "90px";
            this.IsShowButtonNewPost = false;
        }
    };
    SocialPostsComponent.prototype.onScroll = function () {
        var element = document.getElementById(this.PostListsId);
        if (element != null) {
            var height = Number(element.style.height.replace("px", ""));
            if ((element.scrollHeight - element.scrollTop) == element.clientHeight) {
                if (!this.IsNoResult && !this.IsLoadRun) {
                    this.PageIndex = this.LoadedPostCount;
                    this.PageSize = 10;
                    this.LoadData();
                }
            }
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], SocialPostsComponent.prototype, "viewContainerRef", void 0);
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], SocialPostsComponent.prototype, "SocialPeopleViewContainerRef", void 0);
    SocialPostsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialPostsComponent',
            templateUrl: './SocialPostsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, core_1.ChangeDetectorRef])
    ], SocialPostsComponent);
    return SocialPostsComponent;
}());
exports.SocialPostsComponent = SocialPostsComponent;
var PostQueryHeaderClass = /** @class */ (function () {
    function PostQueryHeaderClass(name) {
        this.PostQueryLineLists = [];
        this.Name = name;
        this.PostQueryLineLists = [];
        if (this.Name == "Feed") {
            this.PostQueryLineLists.push(new PostQueryLineClass("What I follow", "Following"));
            this.PostQueryLineLists.push(new PostQueryLineClass("To Me", "ToMe"));
            this.PostQueryLineLists.push(new PostQueryLineClass("All Company", "All"));
        }
        else {
            this.PostQueryLineLists.push(new PostQueryLineClass("All People", "People"));
        }
    }
    return PostQueryHeaderClass;
}());
exports.PostQueryHeaderClass = PostQueryHeaderClass;
var PostQueryLineClass = /** @class */ (function () {
    function PostQueryLineClass(name, code) {
        this.Name = name;
        this.Code = code;
    }
    return PostQueryLineClass;
}());
exports.PostQueryLineClass = PostQueryLineClass;
var PostViewModelData = /** @class */ (function () {
    function PostViewModelData(entityPM, viewMode, type) {
        var _this = this;
        this.CountOtherLike = "";
        this.DisplayOtherLike = "";
        this.IsShowReplyCommentArea = false;
        this.DisplayEntityInformation = false;
        this.CommentsList = [];
        this.LikeLabel = "Like";
        this.DisplayNumberOfLike = false;
        this.ToolTipLike = "";
        this.NumberPostComments = "";
        this.EntityImageSource = "";
        this.VisibityEditDeletPost = false;
        this.ObjectTableName = "";
        this.ObjectTableId = "";
        this.MessageComment = "Write a comment...";
        this.HeightInPutComment = "25px";
        this.IsShowButtonNewComment = false;
        this.AreaAddCommentHeight = "50px";
        this.EntityPM = entityPM;
        this.CreatedByUserName = this.EntityPM.CreatedByUserName;
        this.UserImageDetailId = this.EntityPM.UserImageDetailId;
        this.EntityDescription = this.EntityPM.EntityDescription;
        this.CreateDate = this.EntityPM.CreateDate;
        this.SetImageEntity(this);
        this.CommentsList = [];
        this.ViewMode = viewMode;
        this.BodyText = viewMode.ConvertBodyText(this.EntityPM.BodyText);
        if (this.EntityPM.CreatedById == SessionLocator_1.SessionLocator.LoggedUserId && !this.EntityPM.IsAutomatic)
            this.VisibityEditDeletPost = true;
        this.PostType = type;
        if (type == "Post") {
            entityPM.PostComments.forEach(function (item) {
                _this.CommentsList.push(new PostViewModelData(item, viewMode, "Comment"));
            });
            if (entityPM.PostComments.length > 0 && type == "Post")
                this.IsShowReplyCommentArea = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.EntityId) && !Tools_1.AppTool.IsNullOrEmpty(entityPM.ObjectTableId)) {
                var table = window.ObjectTables.filter(function (d) { return d.Id == entityPM.ObjectTableId; })[0];
                this.ObjectTableName = table.Name;
                this.ObjectTableId = table.Id;
            }
            this.NumberOfComments = this.EntityPM.NumberOfComments;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EntityId) && !viewMode.InsideEntity) {
                this.DisplayEntityInformation = true;
            }
            if (this.NumberOfComments == 0)
                this.DisplayLineEndAreaLike = true;
            else {
                this.NumberPostComments = "(" + this.NumberOfComments.toString() + ")";
            }
            viewMode.RefreshLikesPost(this);
        }
        else {
            viewMode.PefreshCommentLike(this);
        }
        if (!this.UserImageDetailId) {
            if (this.EntityPM.CreatedByUserName) {
                var Name = this.EntityPM.CreatedByUserName.split(' ');
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
            }
        }
        this.UserImagebackground = this.EntityPM.DefaultColor;
    }
    PostViewModelData.prototype.CommentInputFocus = function () {
        if (this.MessageComment == "Write a comment...") {
            this.MessageComment = "";
            this.HeightInPutComment = "40px";
            this.AreaAddCommentHeight = "50px";
            this.IsShowButtonNewComment = true;
        }
    };
    PostViewModelData.prototype.SetImageEntity = function (postViewModelData) {
        var entityImageSource = "";
        var entityPM = postViewModelData.EntityPM;
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
        postViewModelData.EntityImageSource = entityImageSource;
    };
    PostViewModelData.prototype.CreateNewCommentButtonClick = function (parentPos) {
        var _this = this;
        var entityPM = new PostPM_1.PostPM();
        entityPM.BodyText = parentPos.MessageComment;
        entityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        entityPM.CreatedById = entityPM.CreatedById = SessionLocator_1.SessionLocator.LoggedUserId;
        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        entityPM.CreatedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
        entityPM.EntityDescription = parentPos.ViewMode.postFilters.EntityDescription;
        entityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        entityPM.UserImageDetailId = parentPos.ViewMode.LoggedContactPM ? parentPos.ViewMode.LoggedContactPM.ImageDetailId : "";
        entityPM.IndexColor = parentPos.ViewMode.LoggedContactPM ? parentPos.ViewMode.LoggedContactPM.IndexColor : 0;
        entityPM.DefaultColor = parentPos.ViewMode.LoggedContactDefaultColor;
        entityPM.ParentPostId = parentPos.EntityPM.Id;
        entityPM.GroupId = parentPos.EntityPM.GroupId,
            entityPM.IsPrivate = parentPos.EntityPM.IsPrivate,
            entityPM.ObjectTableId = parentPos.ViewMode.postFilters.ObjectTableId;
        entityPM.EntityId = parentPos.ViewMode.postFilters.EntityId;
        parentPos.ViewMode.IsChange = true;
        //this.CurrentSession.StartBusyIndicator("Saving...");
        this.ViewMode.BusyIndicatorText = "Saving...";
        this.ViewMode.ShowBusyIndicator = true;
        //  parentPos.EntityPM.AddPostComment(entityPM);
        parentPos.ViewMode.postPMService.insert(entityPM).subscribe(function (res) {
            var pmResponse = res;
            //this.CurrentSession.StopBusyIndicator();
            _this.ViewMode.ShowBusyIndicator = false;
            if (!pmResponse.HasError && pmResponse.Result) {
                parentPos.EntityPM.PostComments.push(entityPM);
                parentPos.CommentsList.push(new PostViewModelData(entityPM, parentPos.ViewMode, "Comment"));
                parentPos.MessageComment = "Write a comment...";
                parentPos.HeightInPutComment = "25px";
                parentPos.AreaAddCommentHeight = "50px";
                parentPos.IsShowButtonNewComment = false;
            }
        });
    };
    return PostViewModelData;
}());
exports.PostViewModelData = PostViewModelData;
var CommentViewModelData = /** @class */ (function () {
    function CommentViewModelData(entityPM) {
        this.BodyComment = entityPM.BodyText;
    }
    return CommentViewModelData;
}());
exports.CommentViewModelData = CommentViewModelData;
//# sourceMappingURL=SocialPostsComponent.js.map