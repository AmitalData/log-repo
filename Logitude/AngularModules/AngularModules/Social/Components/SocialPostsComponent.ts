

declare var System: any;
declare var window: any;
import {Component, OnInit, ComponentRef, ChangeDetectorRef, ViewChildren, QueryList, ViewContainerRef, ViewChild}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {PostExtendedPMService} from '../Services/ExtendedPMs/PostExtendedPMService';
import {PostFilters} from '../DataContracts/PostFilters';
import {PostsArgs} from '../../Infrastructure/DataContracts/PostsArgs';

import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {PostPMService} from '../Services/StandardPMs/PostPMService';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {ContactPM} from '../../Common/EntityPMs/ContactPM';
import {ContactPMService} from '../../Common/Services/StandardPMs/ContactPMService';
import {PostPM} from '../EntityPMs/PostPM';
import {PostLikePM} from '../EntityPMs/PostLikePM';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService'
declare var HTMLID: any;
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';

import {LocationDirective} from '../../Infrastructure/Utilities/LocationDirective';
@Component({
    moduleId: module.id,
    selector: 'SocialPostsComponent',
    templateUrl: './SocialPostsComponent.html',


})

export class SocialPostsComponent implements OnInit {

    PointerEventsInPutPost: string = "auto";
    OpacityAreaInPutPost: string = "1";
    IsShowAreaPost: boolean = true;
    @ViewChild('Child', { read: ViewContainerRef }) viewContainerRef: ViewContainerRef;
    @ViewChild('Child', { read: ViewContainerRef }) SocialPeopleViewContainerRef: ViewContainerRef;
    IsChange: boolean = false;
    postExtendedPMService: PostExtendedPMService;
    public ComponentRef: ComponentRef<SocialPostsComponent>;
    contactPMService: ContactPMService;
    postPMService: PostPMService;
    IsShowButtonNewPost: boolean = false;
    AreaPostHeight: string = "90px";
    MessagePost: string = "What are you working on?";
    SubQueryName: string = "All Posts";
    HeightInPutPost: string = "30px";
    postFilters: PostFilters = new PostFilters();
    SelectedMainMenu: PostQueryLineClass;
    LoggedContactPM: ContactPM;
    SocialContactPM: ContactPM;

    PostsLists: PostViewModelData[] = [];
    PostQueryHeaderLists: PostQueryHeaderClass[] = [];
    UserId: string = "";
    ImageId: string = "";
    IsNoData: boolean;
    private SocialPostsRefreshEvent: any = null;
    PostLableVisibility: boolean = true;
    IsEntityMode: boolean = false;
    IsUserMode: boolean = false;
    ScreenCode: string = "";
    InsideEntity: boolean = false;
    IsLoggedUser: boolean = false;
    UserViewName: string;
    CoundFollowerUser: number;
    Coundlikesreceived: number;
    IsShowUserImage: boolean = false;
    IsChangeUserLogo: boolean = false;
    IsRefreshImage: boolean = false;
    PostsArgs: PostsArgs;
    PostListsId: string = Guid.newGuid();
    PageIndex: number = 0;
    PageSize: number = 0;
    LoadedPostCount: number = 0;
    IsShowPeopleComponent: boolean = false;
    IsLoadRun: boolean = false;
    IsNoResult: boolean = false;
    HideLeftArea: boolean = false;
    PostsPMLists: PostPM[];
    IsLoadedSocialContact: boolean = false;
    IsLoadedLoggedContact:boolean = false;
    IsLoadedSocial: boolean = false;

    ShowBusyIndicator: boolean = false;
    BusyIndicatorText: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private _entityResourceService: EntityResourceService, private cd: ChangeDetectorRef) {
        this.postExtendedPMService = new PostExtendedPMService();
        this.contactPMService = new ContactPMService();
        this.postPMService = new PostPMService();


        
  
        this.PostQueryHeaderLists = [];
        this.PostQueryHeaderLists.push(new PostQueryHeaderClass("Feed"));
        this.PostQueryHeaderLists.push(new PostQueryHeaderClass("People"));

       

        this.Listen();

    }

    ngOnInit(

    ) {

    }
 
    Listen() {
        if (!this.SocialPostsRefreshEvent) {
            this.SocialPostsRefreshEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "SocialPostsRefresh") {
                    this.LoadingSocialList();
                } 
            });
        }
    }

    LoggedContactDefaultColor: string = "";

    InitializePostComponent(args: PostsArgs) {
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
        

        if (AppTool.IsNullOrEmpty(args.UserId)) {
            args.UserId = SessionLocator.LoggedUserId;
        }




        this.UserId = args.UserId;

        if (SessionLocator.LoggedUserId == this.UserId) this.IsLoggedUser = true;

        this.LoadLoggedContact();

        if (AppTool.IsNullOrEmpty(args.SubQueryName)) {
            args.SubQueryName = "All";
            if (args.ScreenCode == "CRM" || args.ScreenCode == "Quote" || args.ScreenCode == "Customer" || args.ScreenCode == "Opportunity") {
                args.SubQueryName = "User";
            }
        }

        this.SelectedMainMenu = this.PostQueryHeaderLists[0].PostQueryLineLists.filter(d => d.Code == args.QueryName)[0];
        if (args.SubQueryName == "All") this.SubQueryName = "All Posts";
        else if (args.SubQueryName == "User") this.SubQueryName = "User Posts";
        else if (args.SubQueryName == "Auto") this.SubQueryName = "Auto Posts";


        if (this.IsUserMode) {

            this.GetPostSummaryData();
        }

        this.postFilters = new PostFilters()
        this.postFilters.PageIndex = 0;
        this.postFilters.PageSize = 10;
        this.postFilters.QueryName = args.QueryName;
        this.postFilters.SubQueryName = args.SubQueryName;
        this.postFilters.UserId = args.UserId;

        this.postFilters.EntityId = !AppTool.IsNullOrEmpty(args.EntityId) ? args.EntityId : null;
        this.postFilters.ObjectTableId = !AppTool.IsNullOrEmpty(args.ObjectTableId) ? args.ObjectTableId : null;
        this.postFilters.EntityDescription = !AppTool.IsNullOrEmpty(args.EntityDescription) ? args.EntityDescription : null; 
        //this.postFilters.RegardingEntity = !AppTool.IsNullOrEmpty(args.RegardingEntity) ? args.RegardingEntity : null;
        this.postFilters.SearchByEntity = args.InsideEntity;
       
        this.LoadingSocialList();

    }

 
    LoadingSocialList() {

        this.PostsPMLists = [];
        this.PostsLists = [];
        this.LoadedPostCount = 0;
        this.PageIndex = 0;

        var NumberofRow: string = Number(window.innerHeight / 82).toString();
        var size: number = 10;
        if (NumberofRow.indexOf(".") > -1) {
            NumberofRow = NumberofRow.split(".")[0];
            size = Number(NumberofRow) + 1;
        }
        else {
            size = Number(NumberofRow);
        }

        this.PageSize = (size - 1) < 10 ? 10 : (size-1);

        this.IsNoResult = false;
        this.ScrolToTop();
        this.LoadData();

    }

    LoadSocialContact() {

        this.postExtendedPMService.GetSocialContact(this.UserId, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            this.IsLoadedSocialContact = true;
            this.StopBusyIndicator();

            if (!pmResponse.HasError && pmResponse.Result) {
        
                this.SocialContactPM = pmResponse.Result;
                if (this.SocialContactPM) {
                    this.UserViewName = this.SocialContactPM.EnglishName;
                    this.ImageId = this.SocialContactPM != null ? this.SocialContactPM.ImageDetailId : "";
                }

            }
            this.IsShowUserImage = true;
        });
    }

    LoadLoggedContact() {

        if (!this.IsLoggedUser) {
            this.LoadSocialContact();
        } else {
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

            this.postExtendedPMService.GetSocialContact(SessionLocator.LoggedUserPM.Id, SessionLocator.LoggedUserPM.Tenant).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                this.IsLoadedLoggedContact = true;
                this.StopBusyIndicator();
                if (!pmResponse.HasError && pmResponse.Result) {
                    this.LoggedContactPM = pmResponse.Result;
                    if (this.IsLoggedUser) {
                        this.SocialContactPM = this.LoggedContactPM;
                        if (this.SocialContactPM) {
                            this.UserViewName = this.SocialContactPM.EnglishName;
                            this.ImageId = this.SocialContactPM != null ? this.SocialContactPM.ImageDetailId : "";
                        }
                    }

                }
                if (this.IsLoggedUser) {
                    this.IsShowUserImage = true;
                }


            });
        }

       

    }

    LoadData() {

        if (!this.IsLoadRun) {
            this.IsLoadRun = true;
            this.IsNoData = false;
            //this.CurrentSession.StartBusyIndicator("Loading...");
            this.BusyIndicatorText = "Loading...";
            this.ShowBusyIndicator = true;
            this.postFilters.PageIndex = this.PageIndex;
            this.postFilters.PageSize = this.PageSize;

            this.postExtendedPMService.PostFilteredPosts(this.postFilters).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                this.IsLoadedSocial = true;
                this.StopBusyIndicator();
           
                this.IsLoadRun = false;
                this.IsNoResult = true;
                if (!pmResponse.HasError && pmResponse.Result) {

                    if (pmResponse.Result.length > 0 && pmResponse.Result.length == this.PageSize) this.IsNoResult = false;

                    this.LoadedPostCount += pmResponse.Result.length;
                    pmResponse.Result.forEach((item) => {
                        this.PostsPMLists.push(item);
                    });

                    pmResponse.Result.forEach((item) => {
                        this.PostsLists.push(new PostViewModelData(item, this, "Post"));

                    });
                }
                else {
                    this.IsLoadedSocial = true;
                    this.StopBusyIndicator();
                }

                if (this.PostsLists.length == 0) this.IsNoData = true;
            });
        }
      
       
    }
    IsFollower: boolean = false;
    GetPostSummaryData() {

        this.postExtendedPMService.GetPostSummaryData(this.UserId, SessionLocator.LoggedUserId, SessionLocator.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                this.CoundFollowerUser = pmResponse.Result.CoundFollowerUser;
                this.Coundlikesreceived = pmResponse.Result.Coundlikesreceived;
                this.IsFollower = pmResponse.Result.IsFollower;
                if (!this.IsFollower) {
                    //this.OpacityAreaInPutPost = "0.5";
                   // this.PointerEventsInPutPost = "none";
                    //this.IsShowAreaPost = false;
                }
                
            }

        });
    }

    BuildItemsSource() {
        this.PostsLists = [];
        this.PostsPMLists = this.PostsPMLists.sort((a, b) => (a.UpdateDate > b.UpdateDate) ? -1 : ((a.UpdateDate < b.UpdateDate) ? 1 : 0));


        this.PostsPMLists.forEach((item) => {
            this.PostsLists.push(new PostViewModelData(item, this, "Post"));

        });

        this.IsNoData = false;
        if (this.PostsLists.length == 0) {
            this.IsNoData = true;
        }
    }

    RefreshMainLog(imageId:string) {
       
        this.ImageId = imageId;
        if (this.LoggedContactPM) {
            this.LoggedContactPM.ImageDetailId = imageId;
        }
            this.IsRefreshImage = !this.IsRefreshImage;
            this.PostsPMLists.filter(d => d.CreatedById == SessionLocator.LoggedUserId || (d.PostComments.length > 0 && d.PostComments.filter(d => d.CreatedById == SessionLocator.LoggedUserId)[0] != null)).forEach((post) => {

                post.UserImageDetailId = this.ImageId;
                post.PostComments.filter(d => d.CreatedById == SessionLocator.LoggedUserId).forEach((comment) => {
                    comment.UserImageDetailId = this.ImageId;
                });

            });

        
       this.BuildItemsSource();
    }

    PostInputFocus() {

        if (this.MessagePost == "What are you working on?") {
            this.MessagePost = "";
            this.HeightInPutPost = "80px";
            this.AreaPostHeight = "100px";
            this.IsShowButtonNewPost = true;
        }
    }

    ImageUploadedCompleted(event) {

        this.CurrentSession.FireEvent({ Name: 'RefreshSocialLogo', ImageId: event });

        if (this.LoggedContactPM != null) {
            if (this.LoggedContactPM.ImageDetailId != event) {

                //this.CurrentSession.StartBusyIndicator("Saving...");
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;

                this.LoggedContactPM.ImageDetailId = event;
                this.ImageId = event;
                this.contactPMService.update(this.LoggedContactPM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    this.CurrentSession.FireEvent("SocialMessagesRefresh");
                    //this.CurrentSession.StopBusyIndicator();
                    this.ShowBusyIndicator = false;
                    this.PostsPMLists.filter(d => d.CreatedById == SessionLocator.LoggedUserId || (d.PostComments.length > 0 && d.PostComments.filter(d => d.CreatedById == SessionLocator.LoggedUserId)[0] != null)).forEach((post) => {

                        post.UserImageDetailId = this.ImageId;
                        post.PostComments.filter(d => d.CreatedById == SessionLocator.LoggedUserId).forEach((comment) => {
                            comment.UserImageDetailId = this.ImageId;
                        });

                    });
                    this.BuildItemsSource();




                    if (this.IsUserMode) {

                        if (this.PostsArgs.TiggerViewModel) {
                            this.PostsArgs.TiggerViewModel.RefreshMainLog(event);
                    
                        }

              
                    }
                    
                });


            }

        }
    }

    QueryPostMethod(selectQuerytext) {


        if (selectQuerytext == "All") this.SubQueryName = "All Posts";
        else if (selectQuerytext == "User") this.SubQueryName = "User Posts";
        else {
            if (selectQuerytext == "Auto") this.SubQueryName = "Auto Posts";
        }

        if (this.postFilters.SubQueryName != selectQuerytext) {
            this.postFilters.SubQueryName = selectQuerytext;
            this.LoadingSocialList();
        }




    }

    RefreshButtonClicked() {
        //this.LoadData();

        this.LoadingSocialList();
    }

    CreateNewPostButtonClick() {
    
        if (!AppTool.IsNullOrEmpty(this.MessagePost)) {
            if (this.MessagePost.length <= 4000) {
                var entityPM: PostPM = new PostPM();
                entityPM.BodyText = this.MessagePost;
                entityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                entityPM.CreatedById = entityPM.CreatedById = SessionLocator.LoggedUserId;
                entityPM.Tenant = SessionLocator.Tenant;
                entityPM.ObjectTableId = this.postFilters.ObjectTableId;
                entityPM.EntityId = this.postFilters.EntityId;
                entityPM.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName; 
                entityPM.EntityDescription = this.postFilters.EntityDescription;
                entityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
                entityPM.UserImageDetailId = this.LoggedContactPM !=null ? this.LoggedContactPM.ImageDetailId:"";
                entityPM.IndexColor = this.LoggedContactPM != null ? this.LoggedContactPM.IndexColor : 1;
                entityPM.DefaultColor = this.LoggedContactDefaultColor;
                
                this.IsChange = true;
                //this.CurrentSession.StartBusyIndicator("Saving...");
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;

                this.postPMService.insert(entityPM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    //this.CurrentSession.StopBusyIndicator();
                    this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError && pmResponse.Result) {

                        //this.LoadData();

                        if (this.IsLoggedUser || this.IsFollower) {
                            this.PostsPMLists.push(entityPM);
                            this.BuildItemsSource();
                        }

                        this.ScrolToTop();
                        this.MessagePost = "What are you working on?"; 
                        this.HeightInPutPost = "30px";
                        this.AreaPostHeight = "90px";
                        this.IsShowButtonNewPost = false;
                        

                    }
                   

                });
             
            }
            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("Post maximum charachters should be less than 4000!");
            }
        }
        else {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("This Post seems to be empty. Please, write something to Post");
        }

    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    OnMainListSelectedItemChanged(selected: PostQueryLineClass) {

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

    }




    BackButtonClicked() {
        if (this.ComponentRef) {

            if (this.PostsArgs.TiggerViewModel) {
                if (this.IsChange) {
                    this.PostsArgs.TiggerViewModel.RefreshButtonClicked();
                }
            }

            this.ComponentRef.destroy();
        

        }
    }

    ViewPostUserFeedsButtonClick(item: PostViewModelData) {
        if (!this.IsUserMode) {
            var postFilters: PostFilters = new PostFilters();

            var postsArgs: PostsArgs = new PostsArgs();
            postsArgs.QueryName = "UserPosts";
            postsArgs.SubQueryName = "All";
            postsArgs.UserId = item.EntityPM.CreatedById;
            postsArgs.ScreenCode = "UserPostsControl";
            postsArgs.IsUserMode = true;
            postsArgs.TiggerViewModel = this;


            SessionLocator.DynamicLoader.Load("./Social/Components/SocialPostsComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.InitializePostComponent(postsArgs);
                });

        }

    }

    ViewEntityButtonClick(item: PostViewModelData) {
        if (!AppTool.IsNullOrEmpty(item.EntityPM.EntityId) && !AppTool.IsNullOrEmpty(item.EntityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(d => d.Id == item.EntityPM.ObjectTableId)[0];
            if (table) {
                this._entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {

                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: item.EntityPM.EntityId, ObjectTableName: table.Name });
                           // cmpRef.instance.BackCompleted.subscribe(($event: any) => this.RefreshButtonClicked());
                        });

                });
            }

            

        }
    }
    

    CommentPostButtonClick(item: PostViewModelData) {
        item.IsShowReplyCommentArea = true;
        item.CommentInputFocus();
    }


    RemovePostButtonClicked(item: PostViewModelData) {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this post?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                item.EntityPM.IsCancelled = true;
                this.IsChange = true;
                //this.CurrentSession.StartBusyIndicator("Saving...");
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;
                this.postPMService.update(item.EntityPM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    //this.CurrentSession.StopBusyIndicator();
                    this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError && pmResponse.Result) {
                  
                        this.PostsPMLists = this.PostsPMLists.filter(d => d.Id != item.EntityPM.Id);
                        this.BuildItemsSource();
                      //  this.LoadData();
                      
                    }


                });
            }
        });







    }

    EditPostButtonClicked(item: PostViewModelData) {
        //EditPostComponent
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.PostViewModelData = item;
        this.IsChange = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 700;
        logWindow.Height = 200;
        logWindow.Title = "Edit Post"
        logWindow.Show("./Social/Components/EditPostComponent");
        }

    LikePostButtonClick(item: PostViewModelData) {
        this.IsChange = true;

        if (item.LikeLabel == "Like") {

            var postLikePM: PostLikePM = new PostLikePM(null);
            postLikePM.Tenant = SessionLocator.Tenant;
            postLikePM.UserId = SessionLocator.LoggedUserId;
            postLikePM.UserName = SessionLocator.LoggedUserPM.EnglishName;
            postLikePM.PostId = item.EntityPM.Id;
            if (!item.EntityPM.PostLikes.filter(d => d.PostId == item.EntityPM.Id && d.UserId == SessionLocator.LoggedUserId)[0]) {
                item.EntityPM.AddPostLike(postLikePM);
            }
      

        }
        else {
            var postLikePM: PostLikePM = item.EntityPM.PostLikes.filter(d => d.PostId == item.EntityPM.Id && d.UserId == SessionLocator.LoggedUserId)[0];

            if (postLikePM != null) {
                item.EntityPM.RemovePostLike(postLikePM);
            }
          
        }

        //this.CurrentSession.StartBusyIndicator("Saving...");
        this.BusyIndicatorText = "Saving...";
        this.ShowBusyIndicator = true;

            this.postPMService.update(item.EntityPM).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                //this.CurrentSession.StopBusyIndicator();
                this.ShowBusyIndicator = false;
                if (!pmResponse.HasError && pmResponse.Result) {

                    this.RefreshLikesPost(item);
                }


            });
      
    }

    LikeCommentButtonClick(item: PostViewModelData) {
        this.IsChange = true;
        if (item.LikeLabel == "Like") {

            var postLikePM: PostLikePM = new PostLikePM(null);
            postLikePM.Tenant = SessionLocator.Tenant;
            postLikePM.UserId = SessionLocator.LoggedUserId;
            postLikePM.UserName = SessionLocator.LoggedUserPM.EnglishName;
            postLikePM.PostId = item.EntityPM.Id;
            //this.CurrentSession.StartBusyIndicator("Saving...");
            this.BusyIndicatorText = "Saving...";
            this.ShowBusyIndicator = true;

            this.postExtendedPMService.InsertPostLike(postLikePM).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                //this.CurrentSession.StopBusyIndicator();
                this.ShowBusyIndicator = false;
              if (!pmResponse.HasError && pmResponse.Result) {
                  if (!item.EntityPM.PostLikes.filter(d => d.PostId == item.EntityPM.Id && d.UserId == SessionLocator.LoggedUserId)[0]) {
                      item.EntityPM.PostLikes.push(pmResponse.Result);
                  }

                  this.PefreshCommentLike(item);
                }

            });

      
        }
        else {
            var postLikePM: PostLikePM = item.EntityPM.PostLikes.filter(d => d.PostId == item.EntityPM.Id && d.UserId == SessionLocator.LoggedUserId)[0];

            if (postLikePM != null) {
                item.EntityPM.RemovePostLike(postLikePM);
            }

            //this.CurrentSession.StartBusyIndicator("Saving...");
            this.BusyIndicatorText = "Saving...";
            this.ShowBusyIndicator = true;
            this.postExtendedPMService.DeletePostLike(item.EntityPM.Id, SessionLocator.LoggedUserId, SessionLocator.Tenant).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                //this.CurrentSession.StopBusyIndicator();
                this.ShowBusyIndicator = false;
                if (!pmResponse.HasError) {
                    this.PefreshCommentLike(item);
                }

            });
    
        }

    }

    RefreshLikesPost(item: PostViewModelData) {
        item.LikeLabel = "Like";
         item.DisplayTextOfLike = "";
         item.DisplayNumberOfLike = false;
         item.DisplayAreaLike = false;
         item.NumberPostLike = "";
         var index: number = 1;
         item.ToolTipLike = "";
         item.DisplayOtherLike = "";
         if (item.EntityPM.PostLikes.length > 0) {

   
             if (item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - 1].UserId == SessionLocator.LoggedUserId) {
                 index = 2;
             }


            item.NumberPostLike = item.EntityPM.PostLikes.length > 0 ? "(" + item.EntityPM.PostLikes.length.toString() + ")" : "";
            item.DisplayNumberOfLike = true;
            item.DisplayAreaLike = true;



            if (item.EntityPM.PostLikes.length == 1) {
                if (item.EntityPM.PostLikes.filter(d => d.UserId == SessionLocator.LoggedUserId)[0]) {
                    item.LikeLabel = "UnLike";
                    item.DisplayTextOfLike = "You like this";
                }
                else {
                    item.DisplayTextOfLike = item.EntityPM.PostLikes[0].UserName + " like this";
                }
            }
         
            else if (item.EntityPM.PostLikes.length < 5) {
                if (item.EntityPM.PostLikes.filter(d => d.UserId == SessionLocator.LoggedUserId)[0]) {
                    item.LikeLabel = "UnLike";
             
                    if (item.EntityPM.PostLikes.length == 2) item.DisplayTextOfLike = "You and ";
                    else item.DisplayTextOfLike = "You , ";
                }

                item.EntityPM.PostLikes.forEach((like) => {
                    if (like.UserId != SessionLocator.LoggedUserId) {
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

                var count: number = 0;
                var isMeLike: boolean = false;
                if (item.EntityPM.PostLikes.filter(d => d.UserId == SessionLocator.LoggedUserId)[0] && item.EntityPM.PostLikes.length > 1) {
                    item.DisplayTextOfLike = "You , ";
                    item.LikeLabel = "UnLike";
                    isMeLike = true;
                }

                item.EntityPM.PostLikes.forEach((like) => {
                    if (like.UserId != SessionLocator.LoggedUserId) {
                        count += 1;

                        if (count == 1) {
                            item.DisplayTextOfLike += like.UserName + " , "
                        }
                        else if (count == 2) {
                            item.DisplayTextOfLike += like.UserName + " and "
                        }
                        else {
                            if (like != item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - index]) {
                                item.ToolTipLike += like.UserName + " ,";

                            } else item.ToolTipLike += like.UserName;
                           
                        }
  
                    }
          
                });
                var pepoleLikeNamCoun: number = 2;
                if (isMeLike) pepoleLikeNamCoun += 1;

                item.CountOtherLike = ((item.EntityPM.PostLikes.length - pepoleLikeNamCoun).toString());
                item.DisplayOtherLike = " Others like this";

            }

        }
        else {
            item.LikeLabel = "Like";
            item.DisplayTextOfLike = "";

        }
    }

    PefreshCommentLike(item: PostViewModelData) {

        item.NumberPostLike = "";
        item.LikeLabel = "Like";
        item.ToolTipLike = "";
        if (item.EntityPM.PostLikes.length > 0) {
            item.DisplayNumberOfLike = true;
            if (item.EntityPM.PostLikes.filter(d => d.UserId == SessionLocator.LoggedUserId)[0]) {
                item.LikeLabel = "UnLike";

            }

            item.NumberPostLike = " (" + item.EntityPM.PostLikes.length + ")";

            item.EntityPM.PostLikes.forEach((like) => {
                if (like != item.EntityPM.PostLikes[item.EntityPM.PostLikes.length - 1]) {
                    item.ToolTipLike += like.UserName + " ,";

                } else item.ToolTipLike += like.UserName;

            });
        }
       
    }



    ScrolToTop() {
        var htmlid = HTMLID(this.PostListsId);
        htmlid.scrollTop(0);

    }
    ConvertBodyText(body:string) {
        var textarea = document.createElement("textarea");
        textarea.value = body;
        return textarea.value;

    }

    ngOnDestroy() {
       

        if (this.SocialPostsRefreshEvent) {
            this.SocialPostsRefreshEvent.unsubscribe();
            this.SocialPostsRefreshEvent = null;
        }
    }



    //SocialPeopelArea

    RunComponent() {
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
    }

    private timerToken: any;
    private Retries: number = 0;
    private SocialPeopleComponent: any;

    RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 100) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadSocialPeopleComponent() {

        //if (!this.viewContainerRef) {
        //    this.viewContainerRef = this.SocialPeopleViewContainerRef;
        //}
        SessionLocator.DynamicLoader.Load('./Social/Components/SocialPeopleComponent', this.SocialPeopleViewContainerRef)
            .then(cmpRef => {
                this.SocialPeopleComponent = cmpRef.instance;
                //this.Page_Post.InitializePostComponent(postsArgs);
               
            });
    }


    //IsLoadedSocial
    StopBusyIndicator() {
        if (this.IsLoadedSocial && this.IsLoadedLoggedContact && this.IsLoadedSocialContact) {
            //this.CurrentSession.StopBusyIndicator();
            this.ShowBusyIndicator = false;
            this.MessagePost = "What are you working on?";
            this.HeightInPutPost = "30px";
            this.AreaPostHeight = "90px";
            this.IsShowButtonNewPost = false;
        }

    }


    onScroll() {

        var element = document.getElementById(this.PostListsId);
        if (element != null) {
            var height = Number(element.style.height.replace("px" , ""));
      

            if ((element.scrollHeight - element.scrollTop) == element.clientHeight) {

                if (!this.IsNoResult && !this.IsLoadRun) {
                    this.PageIndex = this.LoadedPostCount;
                    this.PageSize = 10;
                    this.LoadData();
                }

              
            }

        }
    }
}



export class PostQueryHeaderClass {
    Name: string;
    PostQueryLineLists: PostQueryLineClass[] = [];
    constructor(name: string) {
        this.Name = name;
        this.PostQueryLineLists = [];

        if (this.Name == "Feed") {

            this.PostQueryLineLists.push(new PostQueryLineClass("What I follow", "Following")); 
            this.PostQueryLineLists.push(new PostQueryLineClass("To Me", "ToMe")); 
            this.PostQueryLineLists.push(new PostQueryLineClass("All Company", "All")); 


        } else {
            this.PostQueryLineLists.push(new PostQueryLineClass("All People", "People")); 

        }
  
    }

}

export class PostQueryLineClass {
    Name: string;
    Code: string;
    constructor(name: string, code:string) {
        this.Name = name;
        this.Code = code;
    }
}


export class PostViewModelData {
    CountOtherLike: string = "";
    DisplayOtherLike: string = "";
    BodyText: string;
    IsShowReplyCommentArea: boolean = false;
    CreatedByUserName: string;
    UserImageDetailId: string;
    CreateDate: Date;
    UserNameImage: string;
    UserImagebackground: string;
    EntityDescription: string;
    EntityPM: PostPM;
    DisplayEntityInformation: boolean = false;
    NumberPostLike: string;
    DisplayAreaLike: boolean;
    CommentsList: PostViewModelData[] = [];
    DisplayLineEndAreaLike: boolean;
    LikeLabel: string = "Like";
    DisplayTextOfLike: string;
    DisplayNumberOfLike: boolean = false;
    ToolTipLike: string = "";
    NumberPostComments: string = "";
    EntityImageSource: string = "";
    NumberOfComments: number;
    VisibityEditDeletPost: boolean = false;
    ObjectTableName: string = "";
    TextAreaRow: number;
    TextAreaCols: number;
    ViewMode: any;
    ObjectTableId: string = "";
   
    MessageComment: string = "Write a comment...";
    HeightInPutComment: string = "25px";
    IsShowButtonNewComment: boolean = false;
    AreaAddCommentHeight: string = "50px";
    PostType: string;


    constructor(entityPM: PostPM , viewMode:any , type:string) {
        this.EntityPM = entityPM;
        this.CreatedByUserName = this.EntityPM.CreatedByUserName;
        this.UserImageDetailId = this.EntityPM.UserImageDetailId;
        this.EntityDescription = this.EntityPM.EntityDescription; 
        this.CreateDate = this.EntityPM.CreateDate;
        this.SetImageEntity(this);
        this.CommentsList = [];
        
        this.ViewMode = viewMode;
 
        this.BodyText = viewMode.ConvertBodyText(this.EntityPM.BodyText);

        if (this.EntityPM.CreatedById == SessionLocator.LoggedUserId && !this.EntityPM.IsAutomatic) this.VisibityEditDeletPost = true;


        this.PostType = type;
        if (type == "Post") {
            entityPM.PostComments.forEach((item) => {
                this.CommentsList.push(new PostViewModelData(item, viewMode, "Comment"));
            });

            if (entityPM.PostComments.length > 0 && type == "Post") this.IsShowReplyCommentArea = true;

            if (!AppTool.IsNullOrEmpty(entityPM.EntityId) && !AppTool.IsNullOrEmpty(entityPM.ObjectTableId)) {
                var table = window.ObjectTables.filter(d => d.Id == entityPM.ObjectTableId)[0];

                this.ObjectTableName = table.Name;
                this.ObjectTableId = table.Id;

            }

            this.NumberOfComments = this.EntityPM.NumberOfComments;
            if (!AppTool.IsNullOrEmpty(this.EntityPM.EntityId) && !viewMode.InsideEntity) {
                this.DisplayEntityInformation = true;
            }

            if (this.NumberOfComments == 0) this.DisplayLineEndAreaLike = true;
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

                var Name: string [] = this.EntityPM.CreatedByUserName.split(' ');

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
                   
               if (userNameImage) this.UserNameImage = userNameImage.toUpperCase();
              
            }
        }

        this.UserImagebackground = this.EntityPM.DefaultColor;
  

    }


   

    CommentInputFocus() {
        if (this.MessageComment == "Write a comment...") {
            this.MessageComment = "";
            this.HeightInPutComment = "40px";
            this.AreaAddCommentHeight = "50px";
            this.IsShowButtonNewComment = true;
        }
    }


   SetImageEntity(postViewModelData: PostViewModelData) {
        var entityImageSource: string = "";

        var entityPM: PostPM = postViewModelData.EntityPM;
        if (!AppTool.IsNullOrEmpty(entityPM.EntityId) && !AppTool.IsNullOrEmpty(entityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(d => d.Id == entityPM.ObjectTableId)[0];
            switch (table.Name) {
                case "Customer":
                    entityImageSource = "./Images/Maintenance/Customer.png";
                    break;
                case "Opportunity":
                    entityImageSource = "./Images/Opportunity.png";
                    break;
                case "Activity":
                    entityImageSource = "./Images/Activities/AP.png";

                    if (!AppTool.IsNullOrEmpty(entityPM.EntityDescription)) {
                        var descr: string[] = entityPM.EntityDescription.split('-');
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
            };
        }
        postViewModelData.EntityImageSource = entityImageSource;

    }

   CreateNewCommentButtonClick(parentPos: PostViewModelData) {
       var entityPM: PostPM = new PostPM();
       entityPM.BodyText = parentPos.MessageComment;
       entityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
       entityPM.CreatedById = entityPM.CreatedById = SessionLocator.LoggedUserId;
       entityPM.Tenant = SessionLocator.Tenant;
       entityPM.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
       entityPM.EntityDescription = parentPos.ViewMode.postFilters.EntityDescription;
       entityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
       entityPM.UserImageDetailId = parentPos.ViewMode.LoggedContactPM ? parentPos.ViewMode.LoggedContactPM.ImageDetailId:"";
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
       parentPos.ViewMode.postPMService.insert(entityPM).subscribe(res => {
           var pmResponse: ServiceResponse = res;
           //this.CurrentSession.StopBusyIndicator();
           this.ViewMode.ShowBusyIndicator = false;
           if (!pmResponse.HasError && pmResponse.Result) {
               parentPos.EntityPM.PostComments.push(entityPM);
               parentPos.CommentsList.push(new PostViewModelData(entityPM, parentPos.ViewMode, "Comment"));
               parentPos.MessageComment = "Write a comment...";
               parentPos.HeightInPutComment = "25px";
               parentPos.AreaAddCommentHeight = "50px";
               parentPos.IsShowButtonNewComment = false;


           }
       });
   }
 
  
}



export class CommentViewModelData {
    BodyComment: string;
    constructor(entityPM: PostPM) {
        this.BodyComment = entityPM.BodyText; 
    }
}










