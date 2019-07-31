import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {ConversationHeaderExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderExtendedPMService';
import {MessageFilters} from '../DataContracts/MessageFilters';
import {PostsArgs} from '../../Infrastructure/DataContracts/PostsArgs';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {ConversationHeaderPM} from '../EntityPMs/ConversationHeaderPM';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService'
import {ConversationHeaderPMService} from '../Services/StandardPMs/ConversationHeaderPMService';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
declare var HTMLID: any;

declare var System: any;
declare var window: any;
@Component({
    moduleId: module.id,
    selector: 'SocialMessagesComponent',
    templateUrl: './SocialMessagesComponent.html',


})

export class SocialMessagesComponent implements OnInit {
    Small: any;
    ShowBusyIndicator: boolean = false;
    BusyIndicatorText: string = "";

    SocialMessagesComponent: SocialMessagesComponent;
    IsViewMessage: boolean = false;
    conversationHeaderExtendedPMService: ConversationHeaderExtendedPMService;
    conversationHeaderPMService: ConversationHeaderPMService;
    private SocialMessageRefreshEvent: any = null;
    private RefreshSocialContactLogo: any = null;
    
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
    ConversationHeader: ConversationHeaderViewModelData;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.conversationHeaderExtendedPMService = new ConversationHeaderExtendedPMService();
        this.conversationHeaderPMService = new ConversationHeaderPMService();
        this.SocialMessagesComponent = this;
         this.Listen();
    }

    ngOnInit(

    ) {


    }

    Listen() {
        if (!this.SocialMessageRefreshEvent) {
            this.SocialMessageRefreshEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "SocialMessagesRefresh") {
                    this.LoadingMessageList();
                }
            });
        }

        if (!this.RefreshSocialContactLogo) {
            this.RefreshSocialContactLogo = this.CurrentSession.SessionEvent.subscribe(arg => {
                if (arg) {
                    if (arg.Name == "RefreshSocialLogo") {
                        this.PostsArgs.LoggedContactImageDetailId = arg.ImageId;
                    }
                }
            });
        }

    }



    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SocialMessageRefreshEvent);
        AppTool.KillEventEmitter(this.RefreshSocialContactLogo);

 
    }


    InitializeMessageComponent(args: PostsArgs) {
        this.PostsArgs = args;
   
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

            this.MessageFilters.AreaMessage = "Message";

            this.ScreenCode = args.ScreenCode;

            this.LoadingMessageList();
        }
    }


    ScrolToTop() {
        var htmlid = HTMLID(this.MessageListsId);
        htmlid.scrollTop(0);

    }

    LoadingMessageList() {

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

        this.PageSize = size<10 ? 10:size;

      

        this.IsNoResult = false;
        this.ScrolToTop();
        this.LoadData();

    }


    LoadData() {
        if (!this.IsLoadRun) {
            this.IsLoadRun = true;
            this.IsNoData = false;
            //this.CurrentSession.StartBusyIndicator("Loading...");
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
                        this.ConversationHeaderLists.push(new ConversationHeaderViewModelData(item, this.InsideEntity));

                    });
              

                }
                else {
                    this.IsLoadedMessages = true;
                    this.StopBusyIndicator();
                }

                if (this.ConversationHeaderLists.length == 0) this.IsNoData = true;
            });
        }
    }


    OnSelectMessage(item: ConversationHeaderViewModelData) {
        this.ConversationHeader = item;

        this.IsViewMessage = true;
       

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
        if (this.IsLoadedMessages ) {
            //this.CurrentSession.StopBusyIndicator();
            this.ShowBusyIndicator = false;
           
        }

    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


   


    OnmMouseleave(item:ConversationHeaderViewModelData) {
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
    }

    IsStartWaitingLoading: boolean = false;
    WaitingForResponse(item: ConversationHeaderViewModelData) {
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
               
            
                this.ShowBusyIndicator = false;
            });
        }
    }

    NewMessageButtonClick(postsArgs:any = null) {
        var areaMessage = "Inbox";
        if (!postsArgs) {
            postsArgs = this.PostsArgs;
            areaMessage = "Message";
        }
            var windowArgs: any = {};
            var logWindow = new LogitudeWindow();
            windowArgs.PostsArgs = postsArgs;
            windowArgs.AreaMessage = areaMessage;
            logWindow.WindowArgs = windowArgs;
            logWindow.Width = 550;
            logWindow.Height = 390;
            logWindow.Title = "New Message"
            logWindow.Show("./Social/Components/NewSocialMessageComponent");
    
    }
    GotoInboxButtonClick() {


        SessionLocator.DynamicLoader.Load("./Social/Components/SocialInboxMessageComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.InitializeMessageComponent(this.PostsArgs, this);
            });

    }
    RefreshButtonClick() {
        this.LoadingMessageList();
        this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
    }

    RemoveConversationHeader(item: ConversationHeaderViewModelData) {
        if (item != null) {
            var index = this.ConversationHeaderLists.indexOf(item);
            if (index > -1) {
                this.ConversationHeaderLists.splice(index, 1);
                
            }
        }

    }


    RemoveConversationHeaderFromInBox(item: ConversationHeaderViewModelData) {
        var conversationHeader: ConversationHeaderViewModelData = this.ConversationHeaderLists.filter(d => d.ConversationHeaderId == item.ConversationHeaderId)[0];
        if (conversationHeader) {
            this.RemoveConversationHeader(conversationHeader);
        }

    }


}

export class ConversationHeaderViewModelData {
   
    EntityPM: ConversationHeaderPM;
    ViewMode: any;
    MessageBody: string = "";
    BackgroundConversationHeader: string = "";
    DisplayEntityInformation: boolean = false;
    ObjectTableName: string = "";
    ObjectTableId: string;
    EntityImageSource: string;
    EntityDescription: string;
    MessageParticipants: string;
    ForegroundMessageParticipants: string = "";
    CreateDate: Date;
    MessageOthers: string;
    ForegroundMessageBody: string;
    VisibilityIsWaitingForResponse: boolean = false;
    VisibilityIsNotWaitingForResponse: boolean = false;
    FontWeightbody: string = "";
    IsShowNumberUnreadComment: boolean = false;
    IsShowArrowSend: boolean = false;
    IsShowArrowReceiveMessage: boolean = false;
    NumberUnreadComment: string;
    LasMessageUserName: string;
    ConversationHeaderId: string;
    IsShowUnreadadndDeleteMessageArea: boolean = false;
    WaitingForResponse: boolean = false;
    Area: string = "";
    IsChange: boolean = false;
    private _entityResourceService: EntityResourceService
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(entityPM: ConversationHeaderPM, insideEntity: boolean , area:string ="Message") {
        this.EntityPM = entityPM;
        this.MessageBody = entityPM.LasMessageBody;
        this._entityResourceService = new EntityResourceService();
        this.ConversationHeaderId = entityPM.Id;
        this.Area = area;
        if (entityPM.MessageParticipantsCount >= 3) {
            if (entityPM.LasMessageUserName != null) {

                if (entityPM.LasMessageUserId == SessionLocator.LoggedUserId) {
                    this.LasMessageUserName = "";
                }
                else {
                    this.LasMessageUserName= entityPM.LasMessageUserName + ": ";
                }

            }

        }
        else this.LasMessageUserName = "";






        this.NumberUnreadComment = entityPM.NumberUnreadComment ? entityPM.NumberUnreadComment.toString():"";

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
        

        if (entityPM.CreatedByUserId == SessionLocator.LoggedUserId) this.WaitingForResponse = true;
        if (entityPM.IsWaitingForResponse == true && entityPM.CreatedByUserId == SessionLocator.LoggedUserId) this.VisibilityIsWaitingForResponse = true;
        else this.VisibilityIsWaitingForResponse = false;
       


        
        if (!AppTool.IsNullOrEmpty(this.EntityPM.EntityId) && (!insideEntity || area == "Inbox")) {
            this.DisplayEntityInformation = true;
        }
        this.CreateDate = this.EntityPM.LastMessageDate;

        this.EntityDescription = this.EntityPM.EntityDescription; 

        if (!AppTool.IsNullOrEmpty(entityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(d => d.Id == entityPM.ObjectTableId)[0];

            this.ObjectTableName = table.Name;
            this.ObjectTableId = table.Id;

        }
     
  
            if (entityPM.MessageParticipantsCount >= 3) {
                this.MessageParticipants = entityPM.MessageParticipants;
            }
            else {

                var ParticipantsName: string[] = [];
                if (entityPM.MessageParticipantsCount < 3) {
                    ParticipantsName = entityPM.MessageParticipants.split(',');


                    if (!AppTool.IsNullOrEmpty(ParticipantsName[0])) {
                        if (ParticipantsName[0] == SessionLocator.LoggedUserPM.EnglishName) {

                            if (!AppTool.IsNullOrEmpty(ParticipantsName[1])) {
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
                    var countothers: number = entityPM.MessageParticipantsCount - 3;
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
            if (entityPM.CreatedByUserId != SessionLocator.LoggedUserId) {
                if (entityPM.LasMessageUserId != entityPM.CreatedByUserId) this.IsShowArrowReceiveMessage = true;
                else this.IsShowArrowReceiveMessage = false;
            }
            else this.IsShowArrowReceiveMessage = false;


            //ShowArrowSend
            if (entityPM.LasMessageUserId != SessionLocator.LoggedUserId) {
                if (entityPM.NumberUnreadComment > 0) this.IsShowArrowSend = true;
                else this.IsShowArrowSend = false;
            }
            else this.IsShowArrowSend = false;


        this.SetImageEntity(this);

    }


    private markAsReadLable: string = "Mark as Unread";
    get MarkAsReadLable() {
        if (this.EntityPM) {

            if (this.EntityPM.NumberUnreadComment > 0) {
                this.markAsReadLable = "Mark as Read";
            }
            else {
                this.markAsReadLable = "Mark as Unread";
            }

        }
        return this.markAsReadLable;
    }
    set MarkAsReadLable(newValue: string) {

        if (this.markAsReadLable != newValue) {
            this.markAsReadLable = newValue;

        }
    }


    SetImageEntity(conversationHeaderViewModelData: ConversationHeaderViewModelData) {
        var entityImageSource: string = "";

        var entityPM: ConversationHeaderPM = conversationHeaderViewModelData.EntityPM;
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
        conversationHeaderViewModelData.EntityImageSource = entityImageSource;

    }

  
    ViewMessageOthers(conversationHeaderViewModelData: ConversationHeaderViewModelData) {

        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.ConversationHeader = conversationHeaderViewModelData;
        //windowArgs.Area = conversationHeaderViewModelData.Area;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 550;
        logWindow.Height = 390;
        logWindow.Title = "Participants"
        logWindow.Show("./Social/Components/SocialMessageParticipantsComponent");
    }

    ViewEntityButtonClick(item: ConversationHeaderViewModelData) {
        if (!AppTool.IsNullOrEmpty(item.EntityPM.EntityId) && !AppTool.IsNullOrEmpty(item.EntityPM.ObjectTableId)) {
            var table = window.ObjectTables.filter(d => d.Id == item.EntityPM.ObjectTableId)[0];
            if (table) {
                this._entityResourceService.getEntityResourceByTableName(table.Name, 0).subscribe(response => {
                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {

                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: item.EntityPM.EntityId, ObjectTableName: table.Name });
                        });

                });
            }



        }
    }

   


}
