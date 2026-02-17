import {Component, OnInit, ComponentRef}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../Infrastructure/Tools';
import {ConversationHeaderViewModelData} from '../Components/SocialMessagesComponent';
declare var HTMLID: any;
import {SocialMessagesComponent} from '../Components/SocialMessagesComponent';

import {ConversationMessageComponent} from '../Components/ConversationMessageComponent';

import {MessageFilters} from '../DataContracts/MessageFilters';
import {PostsArgs} from '../../Infrastructure/DataContracts/PostsArgs';
import {ConversationHeaderExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderExtendedPMService';
import {ConversationHeaderMessagePM} from '../EntityPMs/ConversationHeaderMessagePM';
import {ConversationHeaderPM} from '../EntityPMs/ConversationHeaderPM';
import {ConversationHeaderPMService} from '../Services/StandardPMs/ConversationHeaderPMService';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {ConversationHeaderParticipantExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService';
@Component({
    moduleId: module.id,
    selector: 'SocialInboxMessageComponent',
    templateUrl: './SocialInboxMessageComponent.html',


})

export class SocialInboxMessageComponent implements OnInit {

    IsChange: boolean = false;
    ConversationMessage: ConversationMessageComponent;
    public ComponentRef: ComponentRef<SocialInboxMessageComponent>;
    InBoxMessageQuerySelectedValue: string = "All";
    ShowBusyIndicator: boolean = false;
    BusyIndicatorText: string = "";
    ConversationHeader: ConversationHeaderViewModelData;
    SocialMessagesComponent: SocialMessagesComponent;
    IsViewMessage: boolean = false;
    conversationHeaderExtendedPMService: ConversationHeaderExtendedPMService;
    conversationHeaderPMService: ConversationHeaderPMService;
    private SocialInboxMessageRefreshEvent: any = null;
    PostsArgs: PostsArgs;
    MessageFilters: MessageFilters;
    ConversationHeaderPMLists: ConversationHeaderPM[];
    public ConversationHeaderLists: ConversationHeaderViewModelData[] = [];
    ScreenCode: string = "";
    PageIndex: number = 0;
    PageSize: number = 0;
    IsNoResult: boolean = false;
    IsLoadRun: boolean = false;
    IsNoData: boolean = false;
    IsLoadedMessages: boolean = false;
    LoadedMessagesCount: number = 0;
    InsideEntity: boolean = false;
    MessageListsId: string = Guid.newGuid();
    ConversationHeaderListsId: string;
    conversationHeaderParticipantExtendedPMService: ConversationHeaderParticipantExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.conversationHeaderExtendedPMService = new ConversationHeaderExtendedPMService();
        this.conversationHeaderPMService = new ConversationHeaderPMService();
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService();
        
       this.Listen();
    }




    ngOnInit(

    ) {



    }



    Listen() {
        if (!this.SocialInboxMessageRefreshEvent) {
            this.SocialInboxMessageRefreshEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "SocialInboxMessagesRefresh") {
                    this.LoadingMessageList();
                    this.IsChange = true;
                }
                if (s == "RemoveItemFromInboxMessagesRefresh") {
                    if (this.ConversationHeader) {
                        this.RemoveConversationHeader(this.ConversationHeader);
                        
                    }
                }

            });
        }
    }


    ngOnDestroy() {


        if (this.SocialInboxMessageRefreshEvent) {
            this.SocialInboxMessageRefreshEvent.unsubscribe();
            this.SocialInboxMessageRefreshEvent = null;
        }
    }

    InitializeMessageComponent(args: PostsArgs, socialMessagesComponent: SocialMessagesComponent) {
        this.PostsArgs = args;

        this.SocialMessagesComponent = socialMessagesComponent;
        if (AppTool.IsNullOrEmpty(this.ScreenCode) || this.ScreenCode == args.ScreenCode) {

            if (AppTool.IsNullOrEmpty(args.UserId)) {
                args.UserId = SessionLocator.LoggedUserId;
            }


            this.InsideEntity = args.InsideEntity;
            this.MessageFilters = new MessageFilters()
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
    }

 

    BackButtonClicked() {
        if (this.ComponentRef) {
            if (this.IsChange || this.ConversationHeaderLists.filter(d => d.IsChange)[0] ) {
                this.CurrentSession.FireEvent("SocialMessagesRefresh");
                this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
                
            }
            this.ComponentRef.destroy();
        }
    }



    InBoxMessageQueryChangedMethod(index:number) {

        var queryName: string = "";
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

    }
  
    MarkAsReadButtonClick (item: ConversationHeaderViewModelData) {
        this.BusyIndicatorText = "Saving...";
        this.ShowBusyIndicator = true;
        this.IsChange = true;
        this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantReadAndUnRead(item.ConversationHeaderId, SessionLocator.LoggedUserId, item.MarkAsReadLable).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.ShowBusyIndicator = false;
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

                            this.WaitingForResponse(item);
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
    }
   
    DeleteConversationButtonClick(item: ConversationHeaderViewModelData) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this Message?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;
  
                this.conversationHeaderParticipantExtendedPMService.DeleteConversationHeaderParticipant(item.ConversationHeaderId, SessionLocator.LoggedUserId).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError) {
                    
                        this.RemoveConversationHeader(item);

                    }

                });
            }
        });
    }

  RemoveConversationHeader(item: ConversationHeaderViewModelData) {
      if (item != null) {
          var index = this.ConversationHeaderLists.indexOf(item);
          if (index > -1) {
              this.ConversationHeaderLists.splice(index, 1);
              this.ConversationHeader = this.ConversationHeaderLists[0];
              this.IsChangeConversationHeaderMessageComponent = !this.IsChangeConversationHeaderMessageComponent;
              this.SocialMessagesComponent.RemoveConversationHeaderFromInBox(item);
              if (item.EntityPM.NumberUnreadComment>0) {
                  this.IsChange = true;
              }
          }
      }

  }



    RefreshButtonClick() {
        this.LoadingMessageList(this.InBoxMessageQuerySelectedValue);
   
    }


    IsStartWaitingLoading: boolean = false;
    WaitingForResponse(item: ConversationHeaderViewModelData) {
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

            this.conversationHeaderPMService.update(item.EntityPM).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                this.IsStartWaitingLoading = false;

                if (item.EntityPM.IsWaitingForResponse) item.VisibilityIsWaitingForResponse = true;
                else item.VisibilityIsWaitingForResponse = false;

                if (this.InBoxMessageQuerySelectedValue == "Waiting for response") {
                    this.ConversationHeaderLists = this.ConversationHeaderLists.filter(d => d.ConversationHeaderId != item.ConversationHeaderId);

                    if (this.ConversationHeaderLists.length == 0) this.IsNoData = true;
                }

                this.ShowBusyIndicator = false;
            });
        }
    }

    ScrolToTop() {
        var htmlid = HTMLID(this.MessageListsId);
        htmlid.scrollTop(0);

    }

    LoadingMessageList(queryName: string = "All") {
        this.MessageFilters.QueryName = queryName;
        this.ConversationHeaderPMLists = [];
        this.ConversationHeaderLists = [];
        this.LoadedMessagesCount = 0;
        this.PageIndex = 0;

        var NumberofRow: string = Number(window.innerHeight / 50).toString();
        var size: number = 10;
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

    }


    LoadData() {
        if (!this.IsLoadRun) {
            this.IsLoadRun = true;
            this.IsNoData = false;

            this.BusyIndicatorText = "Loading...";
            this.ShowBusyIndicator = true;
            this.MessageFilters.PageIndex = this.PageIndex;
            this.MessageFilters.PageSize = this.PageSize;

            this.conversationHeaderExtendedPMService.GetMessageByFiltered(this.MessageFilters).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                this.IsLoadedMessages = true;
                this.StopBusyIndicator();

                this.IsLoadRun = false;
                this.IsNoResult = true;
                if (!pmResponse.HasError && pmResponse.Result) {

                    this.LoadedMessagesCount += pmResponse.Result.length;
                    if (pmResponse.Result.length > 0 && pmResponse.Result.length == this.PageSize) this.IsNoResult = false;


                    pmResponse.Result.forEach((item) => {
                        this.ConversationHeaderPMLists.push(item);
                    });

                    pmResponse.Result.forEach((item) => {
                        this.ConversationHeaderLists.push(new ConversationHeaderViewModelData(item, this.InsideEntity,"Inbox"));

                    });
                    this.ConversationHeader = this.ConversationHeaderLists[0];
                    this.IsChangeConversationHeaderMessageComponent = !this.IsChangeConversationHeaderMessageComponent;
                }
                else {
                    this.IsLoadedMessages = true;
                    this.StopBusyIndicator();
                }

                if (this.ConversationHeaderLists.length == 0) this.IsNoData = true;
            });
        }
    }

    onScroll() {

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
    }


    StopBusyIndicator() {
        if (this.IsLoadedMessages) {
            this.ShowBusyIndicator = false;

        }

    }



  


    OnmMouseleave(item: ConversationHeaderViewModelData) {
        this.ConversationHeaderLists.forEach((item) => {
            item.VisibilityIsNotWaitingForResponse = false;
            
        });
    }


    OnmMouseOver(item: ConversationHeaderViewModelData) {
        if (item.EntityPM.IsWaitingForResponse == true && item.EntityPM.CreatedByUserId == SessionLocator.LoggedUserId) {
            item.VisibilityIsNotWaitingForResponse = false;
        }
        else if (item.EntityPM.CreatedByUserId != SessionLocator.LoggedUserId) {
            item.VisibilityIsNotWaitingForResponse = false;
        }
        else item.VisibilityIsNotWaitingForResponse = true;

        this.ConversationHeaderLists.forEach((item) => {
        
                item.IsShowUnreadadndDeleteMessageArea = false;
          
        });

        if (item != this.ConversationHeader) {
            item.IsShowUnreadadndDeleteMessageArea = true;
        }

    }


    NewMessageButtonClick() {
        this.IsChange = true;
        if (this.SocialMessagesComponent) this.SocialMessagesComponent.NewMessageButtonClick(this.PostsArgs);

    }



    IsChangeConversationHeaderMessageComponent: boolean = false;
    OnSelectMessage(item: ConversationHeaderViewModelData) {

        if (item != this.ConversationHeader) {
            this.ConversationHeader = item;
            this.IsChangeConversationHeaderMessageComponent = !this.IsChangeConversationHeaderMessageComponent;
            item.IsShowUnreadadndDeleteMessageArea = false;
        }
    }
}
