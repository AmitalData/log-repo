import {Component, OnInit, ComponentRef}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {SocialMessagesComponent} from '../Components/SocialMessagesComponent';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {ConversationHeaderMessagePM} from '../EntityPMs/ConversationHeaderMessagePM';
import {ConversationHeaderPM} from '../EntityPMs/ConversationHeaderPM';
import {ConversationHeaderViewModelData} from '../Components/SocialMessagesComponent';
import {ConversationHeaderParticipantExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {ConversationHeaderPMService} from '../Services/StandardPMs/ConversationHeaderPMService';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ConversationHeaderMessageExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderMessageExtendedPMService';
import {GroupByPipe} from '../../Infrastructure/Pipes/GroupByPipe';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
declare var HTMLID: any;
import {ConversationHeaderMessagePMService} from '../Services/StandardPMs/ConversationHeaderMessagePMService';

@Component({
    moduleId: module.id,
    selector: 'ConversationMessageComponent',
    templateUrl: './ConversationMessageComponent.html',
    inputs: ['SocialMessagesComponent', 'ConversationHeader', 'Area'],


})

export class ConversationMessageComponent implements OnInit {

    WaitingForResponseKey: string = Guid.newGuid();
    conversationHeaderMessageExtendedPMService: ConversationHeaderMessageExtendedPMService;
    conversationHeaderParticipantExtendedPMService: ConversationHeaderParticipantExtendedPMService;
    conversationHeaderMessagePMService: ConversationHeaderMessagePMService;
    SocialMessagesComponent: SocialMessagesComponent;
    ConversationHeaderMessageLists: ConversationMessageData[] = [];
    ConversationHeaderMessagePMLists: ConversationHeaderMessagePM[] = [];
    conversationHeaderPMService: ConversationHeaderPMService;
    ConversationHeader: ConversationHeaderViewModelData;
    IsNoData: boolean = false;
    IsStartWaitingLoading: boolean = false;
    BusyIndicatorText: string = "";
    ShowBusyIndicator: boolean = false;
    Area: string = "Message";
    LoggedContactImageDetailId: string = "";
    LoggedContactDefaultColor: string = "";
    MessageListsId: string = Guid.newGuid();
    IsLoadedMessages: boolean = false;
    ShowCheckBoxWaitingForResponse: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.conversationHeaderMessageExtendedPMService = new ConversationHeaderMessageExtendedPMService();
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService();
        this.conversationHeaderPMService = new ConversationHeaderPMService();
        this.conversationHeaderMessagePMService = new ConversationHeaderMessagePMService();
    }

    ngOnInit(

    ) {
        if (this.ConversationHeader) {
            this.BusyIndicatorText = "Loading...";
            this.ShowBusyIndicator = true;
            if (this.ConversationHeader.EntityPM.CreatedByUserId == SessionLocator.LoggedUserId) {
                this.ShowCheckBoxWaitingForResponse = true;
            }
            else {
                this.ShowCheckBoxWaitingForResponse= false;
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
    }







    private messageBody = "";
    get MessageBody() {

        return this.messageBody;
    }
    set MessageBody(newValue: string) {

        if (this.messageBody != newValue) {
            this.messageBody = newValue;

        }
    }



    LoadData() {

   
 
        this.conversationHeaderMessageExtendedPMService.GetAllConversationMessageForHeaderQuery(this.ConversationHeader.ConversationHeaderId, SessionLocator.LoggedUserId).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            this.ConversationHeaderMessagePMLists = [];

            this.IsLoadedMessages = true;
            this.StopBusyIndicator();

            if (!pmResponse.HasError && pmResponse.Result) {

                pmResponse.Result.forEach((item) => {
                    this.ConversationHeaderMessagePMLists.push(item);
                });

                this.BuildItemsSource();

               
            }
        });

    }


    private isWaitingForResponse = false;
    get IsWaitingForResponse() {

        if (this.ConversationHeader) this.isWaitingForResponse = this.ConversationHeader.EntityPM.IsWaitingForResponse;
        else this.isWaitingForResponse = false;
        return this.isWaitingForResponse;
    }
    set IsWaitingForResponse(newValue: boolean) {

        if (this.isWaitingForResponse != newValue) {
            this.isWaitingForResponse = newValue;
            this.WaitingForResponse(false);

        }
    }

    

    WaitingForResponse(isloadBusyIndicator: boolean = true) {

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

            this.conversationHeaderPMService.update(this.ConversationHeader.EntityPM).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                this.IsStartWaitingLoading = false;

                if (this.ConversationHeader.EntityPM.IsWaitingForResponse) this.ConversationHeader.VisibilityIsWaitingForResponse = true;
                else this.ConversationHeader.VisibilityIsWaitingForResponse = false;

                if (isloadBusyIndicator) {
                    this.ShowBusyIndicator = false;
                }
            });
        }
    }

    MakeMeReadMessage() {
        this.ConversationHeader.IsChange = true;
        this.conversationHeaderParticipantExtendedPMService.MakeMeReadMessage(this.ConversationHeader.ConversationHeaderId, SessionLocator.LoggedUserId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                this.ConversationHeader.EntityPM.IsRead = true;
                this.ConversationHeader.EntityPM.NumberUnreadComment = 0;
                this.ConversationHeader.IsShowNumberUnreadComment = false;
                this.ConversationHeader.NumberUnreadComment = "0";
    
                this.ConversationHeader.BackgroundConversationHeader = this.ConversationHeader.Area == "Message" ? "#F2F2F2" : "#F5F5F5";


                this.ConversationHeader.ForegroundMessageBody = "#000000";
                this.ConversationHeader.ForegroundMessageParticipants = "#000000";
                this.ConversationHeader.MarkAsReadLable = "Mark as Unread";
                this.ConversationHeader.FontWeightbody = "normal";
                this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
            }

        });
    }

    MarkAsReadButtonClick() {
        this.ConversationHeader.IsChange = true;
        this.BusyIndicatorText = "Saving...";
        this.ShowBusyIndicator = true;

        this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantReadAndUnRead(this.ConversationHeader.ConversationHeaderId, SessionLocator.LoggedUserId, this.ConversationHeader.MarkAsReadLable).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.ShowBusyIndicator = false;
            if (!pmResponse.HasError) {

                if (pmResponse.Result) {

                    if (this.ConversationHeader.MarkAsReadLable == "Mark as Unread") {

                        this.ConversationHeader.EntityPM.NumberUnreadComment = 1;

                        this.ConversationHeader.NumberUnreadComment = "1";
                        this.ConversationHeader.ForegroundMessageBody = "#000000";

                        this.ConversationHeader.BackgroundConversationHeader = this.ConversationHeader.Area == "Message" ? "#ffffff" : "#F5F5F5";

                        this.ConversationHeader.ForegroundMessageParticipants = "#000000";
                        this.ConversationHeader.FontWeightbody = "bold";
                        this.ConversationHeader.IsShowNumberUnreadComment = true;

                        if (this.ConversationHeader.EntityPM.IsWaitingForResponse) {

                            this.WaitingForResponse();
                        }
                    }
                    else {
                        this.ConversationHeader.EntityPM.NumberUnreadComment = 0;
                        this.ConversationHeader.NumberUnreadComment = "0";
                        this.ConversationHeader.IsShowNumberUnreadComment = false;

                        this.ConversationHeader.FontWeightbody = "normal";
                        this.ConversationHeader.ForegroundMessageBody = "#787878";

                        this.ConversationHeader.BackgroundConversationHeader = this.ConversationHeader.Area == "Message" ? "#F2F2F2" : "#F5F5F5";


                        this.ConversationHeader.ForegroundMessageParticipants = "#282E30";

                    }
                }
              
                this.CurrentSession.FireEvent("SociaMessagesCountRefresh");
            }

        });
    }

    DeleteConversationButtonClick() {
        this.ConversationHeader.IsChange = true;
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Are you sure you want to delete this Message?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;

                this.conversationHeaderParticipantExtendedPMService.DeleteConversationHeaderParticipant(this.ConversationHeader.ConversationHeaderId, SessionLocator.LoggedUserId).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError) {
                        if (this.SocialMessagesComponent && this.ConversationHeader.Area == "Message") {
                            this.SocialMessagesComponent.RemoveConversationHeader(this.ConversationHeader);//.ConversationHeaderLists.filter(d => d.EntityPM.Id != this.ConversationHeader.EntityPM.Id);
                            this.BackButtonClicked();
                        } else if (this.ConversationHeader.Area == "Inbox") {

                            this.CurrentSession.FireEvent("RemoveItemFromInboxMessagesRefresh");
                        }

                    }

                });
            }
        });
    }


    ReplyButtonClick() {
        if (!AppTool.IsNullOrEmpty(this.MessageBody)) {
            if (this.MessageBody.length < 999) {

                this.BusyIndicatorText = "Saving...";
                this.ShowBusyIndicator = true;

                var message: ConversationHeaderMessagePM = new ConversationHeaderMessagePM();
                message.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                message.CreatedByUserId = SessionLocator.LoggedUserPM.Id;
                message.Tenant = SessionLocator.Tenant;
                message.ConversationHeaderId = this.ConversationHeader.ConversationHeaderId;
                message.MessageBody = this.MessageBody;
                message.UserName = SessionLocator.LoggedUserPM.EnglishName;
                message.DefaultColor = this.LoggedContactDefaultColor;
                message.UserImageDetailId = this.LoggedContactImageDetailId;
               // message.

                this.MessageBody = "";

                this.conversationHeaderMessagePMService.insert(message).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    this.ShowBusyIndicator = false;
                    if (!pmResponse.HasError && pmResponse.Result) {
                        ServiceLocator.SendTotangoUserActivity("ESN", "New General Message");
                        this.ConversationHeaderMessagePMLists.push(message);
                        this.BuildItemsSource(true);

                        this.ConversationHeader.EntityPM.LasMessageUserId = message.CreatedByUserId;
                        this.ConversationHeader.EntityPM.LasMessageUserName = message.UserName;
                        this.ConversationHeader.EntityPM.LastMessageDate = message.CreateDate;
                        this.ConversationHeader.EntityPM.IsReplied = true;

                        this.ConversationHeader.MessageBody = message.MessageBody;
                        this.ConversationHeader.CreateDate = message.CreateDate;
                        this.ConversationHeader.IsChange = true;

                        if (this.ConversationHeader.EntityPM.CreatedByUserId != SessionLocator.LoggedUserId) {
                            if (this.ConversationHeader.EntityPM.LasMessageUserId != this.ConversationHeader.EntityPM.CreatedByUserId) this.ConversationHeader.IsShowArrowReceiveMessage = true;
                            else this.ConversationHeader.IsShowArrowReceiveMessage = false;
                        }
                        else this.ConversationHeader.IsShowArrowReceiveMessage = false;


                        //ShowArrowSend
                        if (this.ConversationHeader.EntityPM.LasMessageUserId != SessionLocator.LoggedUserId) {
                            if (this.ConversationHeader.EntityPM.NumberUnreadComment > 0) this.ConversationHeader.IsShowArrowSend = true;
                            else this.ConversationHeader.IsShowArrowSend = false;
                        }
                        else this.ConversationHeader.IsShowArrowSend = false;
                        if (this.ConversationHeader.EntityPM.MessageParticipantsCount >= 3) {
                            if (this.ConversationHeader.EntityPM.LasMessageUserName != null) {

                                if (this.ConversationHeader.EntityPM.LasMessageUserId == SessionLocator.LoggedUserId) {
                                    this.ConversationHeader.LasMessageUserName = "";
                                }
                                else {
                                    this.ConversationHeader.LasMessageUserName = this.ConversationHeader.EntityPM.LasMessageUserName + ": ";
                                }

                            }

                        }
                        else this.ConversationHeader.LasMessageUserName = "";
                

                        if (this.ConversationHeader.EntityPM.IsWaitingForResponse == true) {
                            if (this.ConversationHeader.EntityPM.CreatedByUserId != SessionLocator.LoggedUserId) {
                                this.ConversationHeader.EntityPM.IsWaitingForResponse = false;
                                this.ConversationHeader.VisibilityIsWaitingForResponse = false;
                                this.ConversationHeader.VisibilityIsNotWaitingForResponse = true;
                            }

                        }

                        if (this.ConversationHeader.EntityPM.NumberUnreadComment > 0) {
                            this.MakeMeReadMessage();
                        }




                        this.conversationHeaderPMService.update(this.ConversationHeader.EntityPM).subscribe(res => {
                            var pmResponse: ServiceResponse = res;
                            this.MakeMessageRepliedOrRead("Replied");
                            this.MakeMessageRead();
                            this.MakeDeleteParticioantsUnDelete();
                        });








                    }

                });

            }
            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("Message maximum charachters should be less than 1000!");
           
            }
        }
        else {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("This Message seems to be empty. Please, write something to Message");

        }

       


    }
  

    MakeMessageRepliedOrRead(type: string) {
        this.ConversationHeader.IsChange = true;
        var isLoad = false;
        if (type == "Replied") isLoad = true;
        else if (type == "Read") {
            if (this.ConversationHeader.EntityPM.IsRead == false) {
                isLoad = true;
            }
        }

        if(isLoad){

            this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantRepliedOrRead(this.ConversationHeader.ConversationHeaderId, SessionLocator.LoggedUserId, type).subscribe(res => {
                var pmResponse: ServiceResponse = res;
            });
        }
    }


    MakeMessageRead() {
        this.ConversationHeader.IsChange = true;

        this.conversationHeaderParticipantExtendedPMService.MakeConversationHeaderParticipantReadMessage(this.ConversationHeader.ConversationHeaderId, SessionLocator.LoggedUserId).subscribe(res => {
                var pmResponse: ServiceResponse = res;
            });
        
    }


    MakeDeleteParticioantsUnDelete() {
        this.ConversationHeader.IsChange = true;

        this.conversationHeaderParticipantExtendedPMService.MakeDeleteParticipantUnDelete(this.ConversationHeader.ConversationHeaderId).subscribe(res => {
            var pmResponse: ServiceResponse = res;
        });

    }


    BuildItemsSource(isScrolToBottom: boolean = false) {

        var myPipe = new GroupByPipe();
        this.ConversationHeaderMessageLists = [];

        var conversationHeaderMessageViewModelLists: ConversationHeaderMessageViewModel[] = [];
        this.ConversationHeaderMessagePMLists.forEach((item) => {
            conversationHeaderMessageViewModelLists.push(new ConversationHeaderMessageViewModel(item, this.ConversationHeader));
        });

        var list = myPipe.transform(conversationHeaderMessageViewModelLists, "Date");

        list.forEach((value, key) => {
            this.ConversationHeaderMessageLists.push(new ConversationMessageData(value));
        });
        if (isScrolToBottom) this.ScrolToBottom();

    }

    BackButtonClicked() {
        if (this.SocialMessagesComponent) {
            this.SocialMessagesComponent.IsViewMessage = false;
            if (this.ConversationHeader.IsChange) {

                this.CurrentSession.FireEvent("SociaMessagesCountRefresh");

            }

        }

    }


    StopBusyIndicator() {
        if (this.IsLoadedMessages) {
   
            this.ShowBusyIndicator = false;
        
        }

    }
 
    ScrolToBottom() {
   
        var htmlid = document.getElementById(this.MessageListsId);
        htmlid.scrollTop = htmlid.scrollHeight + 30;

    }
}


export class ConversationMessageData {

    EntityPM: ConversationHeaderMessagePM;
    Key: any;
    ConversationHeaderMessageViewModelLists: ConversationHeaderMessageViewModel[] = [];
    constructor(item:any) {
        this.Key = item.key;
        this.ConversationHeaderMessageViewModelLists = item.value;
    }
      


    }
export class ConversationHeaderMessageViewModel {

    EntityPM: ConversationHeaderMessagePM;
    Date: any;
    Time: any;
    CreatedByUserName: string;
    UserImageDetailId: string;
    UserImagebackground: string;
    UserNameImage: string;
    CreateDate: Date;
    MessageBody: string;
    IsShowArrowReceiveMessage: boolean = false;
    constructor(entityPM: ConversationHeaderMessagePM, conversationHeader: ConversationHeaderViewModelData) {
        this.Date = DateTool.GetDateFormats(entityPM.CreateDate).DateString;
        this.EntityPM = entityPM;
        this.CreatedByUserName = entityPM.UserName;
        this.UserImageDetailId = entityPM.UserImageDetailId;
        this.CreateDate = entityPM.CreateDate;
        this.MessageBody = entityPM.MessageBody;
        this.UserImagebackground = entityPM.DefaultColor;

        if (!this.UserImageDetailId) {
            if (this.CreatedByUserName) {

                var Name: string[] = this.CreatedByUserName.split(' ');

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
                this.UserNameImage = userNameImage;

            }
        }

   

        if (conversationHeader.EntityPM.CreatedByUserId != SessionLocator.LoggedUserId) {

            if (entityPM.CreatedByUserId == conversationHeader.EntityPM.CreatedByUserId) this.IsShowArrowReceiveMessage = false;
            else this.IsShowArrowReceiveMessage = true;

        }
        else this.IsShowArrowReceiveMessage = false;
      





    }



}
