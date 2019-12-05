import {Component, OnInit, ViewChildren, QueryList}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {LocationDirective} from '../../Infrastructure/Utilities/LocationDirective';
import {PostsArgs} from '../../Infrastructure/DataContracts/PostsArgs';
import {ConversationHeaderExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderExtendedPMService';

import {AppTool} from '../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
    selector: 'SocialMainComponent',
    templateUrl: './SocialMainComponent.html',


})

export class SocialMainComponent implements OnInit {
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    conversationHeaderExtendedPMService: ConversationHeaderExtendedPMService;
    isLoaderReady: boolean;
    PostsArgs: PostsArgs;
    private SocialMessagesCountRefreshEvent: any = null;

    MessageTabTitle: string = "Message";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    

        this.conversationHeaderExtendedPMService = new ConversationHeaderExtendedPMService();
        this.Listen();
    }

    ngOnInit(

    ) {

     

    }



    Listen() {
        if (!this.SocialMessagesCountRefreshEvent) {
            this.SocialMessagesCountRefreshEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "SociaMessagesCountRefresh") {
                    this.GetCountUnReadMassage();
                }
            });
        }
    }


    ngOnDestroy() {


        if (this.SocialMessagesCountRefreshEvent) {
            this.SocialMessagesCountRefreshEvent.unsubscribe();
            this.SocialMessagesCountRefreshEvent = null;
        }
    }






    InitializeSocialMainComponent(args: PostsArgs) {

        this.PostsArgs = args;

        if (!this.PostsArgs){
            var postsArgs: PostsArgs = new PostsArgs();
            postsArgs.QueryName = "Following";
            postsArgs.SubQueryName = "All";
            postsArgs.UserId = SessionLocator.LoggedUserId;
            postsArgs.ScreenCode = "PostControl";
            
            this.PostsArgs = postsArgs;
        }

        this.PostsArgs.EntityId = AppTool.IsNullOrEmpty(this.PostsArgs.EntityId) ? "" : this.PostsArgs.EntityId;
        this.PostsArgs.AreaMessage = AppTool.IsNullOrEmpty(this.PostsArgs.AreaMessage) ? "" : this.PostsArgs.AreaMessage;
        this.PostsArgs.EntityDescription = AppTool.IsNullOrEmpty(this.PostsArgs.EntityDescription) ? "" : this.PostsArgs.EntityDescription;
        this.PostsArgs.ObjectTableId = AppTool.IsNullOrEmpty(this.PostsArgs.ObjectTableId) ? "" : this.PostsArgs.ObjectTableId;
        this.PostsArgs.QueryName = AppTool.IsNullOrEmpty(this.PostsArgs.QueryName) ? "" : this.PostsArgs.QueryName;
        this.PostsArgs.RegardingEntity = AppTool.IsNullOrEmpty(this.PostsArgs.RegardingEntity) ? "" : this.PostsArgs.RegardingEntity;
        this.PostsArgs.ScreenCode = AppTool.IsNullOrEmpty(this.PostsArgs.ScreenCode) ? "" : this.PostsArgs.ScreenCode;
        this.PostsArgs.SubQueryName = AppTool.IsNullOrEmpty(this.PostsArgs.SubQueryName) ? "" : this.PostsArgs.SubQueryName;
        this.PostsArgs.UserId = AppTool.IsNullOrEmpty(this.PostsArgs.UserId) ? SessionLocator.LoggedUserId : this.PostsArgs.UserId;

        this.GetCountUnReadMassage();
        this.LoadLoggedContactMessageInfo();


   
    }


    LoadLoggedContactMessageInfo() {
        this.conversationHeaderExtendedPMService.GetLoggedContactMessageInfo(SessionLocator.LoggedUserPM.Id, SessionLocator.LoggedUserPM.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError && pmResponse.Result) {
                if (!AppTool.IsNullOrEmpty(pmResponse.Result)) {
                    this.PostsArgs.LoggedContactDefaultColor = pmResponse.Result.split('@')[0];
                    this.PostsArgs.LoggedContactImageDetailId = pmResponse.Result.split('@')[1];
                }

            }

            this.RunComponent();
        });
    }





    private SetSelectedItem() {

        this.SelectedTabCode = "SOP";

    }


    RunComponent() {
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
    }


    GetCountUnReadMassage() {

        if (this.PostsArgs) {
            this.conversationHeaderExtendedPMService.GetCountUnReadConversationHeaderPMs(this.PostsArgs.UserId, this.PostsArgs.EntityId, this.PostsArgs.ObjectTableId, this.PostsArgs.AreaMessage).subscribe(res => {
                var pmResponse: ServiceResponse = res;

                if (!pmResponse.HasError && (pmResponse.Result || pmResponse.Result == 0)) {

                    this.MessageTabTitle = "Message (" + pmResponse.Result.toString() + ")";
                }
            });
        }
    }


    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }




    private selectedTabCode: string;
    get SelectedTabCode() { return this.selectedTabCode; }
    set SelectedTabCode(newValue: string) {
        if (this.selectedTabCode != newValue) {
            this.selectedTabCode = newValue;
            this.SelectionChanged();
        }
    }


    private Page_Message: any = null;
    private Page_Post: any = null;
    SelectionChanged() {
        if (this.isLoaderReady) {
            if (this.selectedTabCode != null) {

                let myLocation: LocationDirective = this.AllLocations.toArray().filter(d => d.Code == this.selectedTabCode)[0];
                if (myLocation != null) {

                    switch (this.selectedTabCode) {
                        //Post
                        case "SOP": {
                            if (this.Page_Post == null) {
                                SessionLocator.DynamicLoader.Load('./Social/Components/SocialPostsComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_Post = cmpRef.instance;

                                        this.Page_Post.InitializePostComponent(this.PostsArgs);
                                    });
                            }

                            break;
                        }

                        //Message
                        case "SOM": {
                            if (this.Page_Message == null) {
                                SessionLocator.DynamicLoader.Load('./Social/Components/SocialMessagesComponent', myLocation.viewContainerRef)
                                    .then(cmpRef => {
                                        this.Page_Message = cmpRef.instance;
                                        this.Page_Message.InitializeMessageComponent(this.PostsArgs);
                                    });
                            }

                            break;
                        }


                    }
                }
            }
        }
    }




    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }

    

   




}
