import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool} from '../../Infrastructure/Tools';

import {PostPM} from '../EntityPMs/PostPM';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {ConversationHeaderPM} from '../EntityPMs/ConversationHeaderPM';
import {ConversationHeaderParticipantPM} from '../EntityPMs/ConversationHeaderParticipantPM';
import {ConversationHeaderMessagePM} from '../EntityPMs/ConversationHeaderMessagePM';

import {PostsArgs} from '../../Infrastructure/DataContracts/PostsArgs';
import {ConversationHeaderPMService} from '../Services/StandardPMs/ConversationHeaderPMService';
import {ConversationHeaderMessagePMService} from '../Services/StandardPMs/ConversationHeaderMessagePMService';
import {ConversationHeaderParticipantExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService';


@Component({
    moduleId: module.id,
    selector: 'NewSocialMessageComponent',
    templateUrl: './NewSocialMessageComponent.html',


})

export class NewSocialMessageComponent implements OnInit {
    IsSaveConversationHeaderParticipantComplete: boolean = false;
    IsSaveConversationHeaderMessagePMComplete: boolean = false;
    WaitingForResponseKey: string = Guid.newGuid();
    UserIds: string;
    PostsArgs: PostsArgs;
    NewConversationHeaderPM: ConversationHeaderPM;
    conversationHeaderPMService: ConversationHeaderPMService;
    conversationHeaderMessagePMService: ConversationHeaderMessagePMService;
    conversationHeaderParticipantExtendedPMService: ConversationHeaderParticipantExtendedPMService;
    RegardingEntity: string = "";
    UserList: string[] = [];
    ExcludedResult: string[] = [];
 
    ValidationErrorsList: string[] = [];
    ConversationHeaderParticipantPMLists: ConversationHeaderParticipantPM[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.conversationHeaderPMService = new ConversationHeaderPMService();
        this.conversationHeaderMessagePMService = new ConversationHeaderMessagePMService();
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService();

        this.ExcludedResult = [];
        this.ExcludedResult.push(SessionLocator.LoggedUserId);

    }

    ngOnInit(

    ) {



    }

    EntityId: string = "";
    ObjectTableId: string = "";
    EntityDescription: string = "";
    AreaMessage: string = "";
    SetWindowArgs(args: any) {
        if (args) {
            this.PostsArgs = args.PostsArgs;
            this.AreaMessage = args.AreaMessage;
            if (this.PostsArgs) {

                this.ObjectTableId = !AppTool.IsNullOrEmpty(this.PostsArgs.ObjectTableId) ? this.PostsArgs.ObjectTableId : null;
                this.EntityId = !AppTool.IsNullOrEmpty(this.PostsArgs.EntityId) ? this.PostsArgs.EntityId : null;
                this.EntityDescription = !AppTool.IsNullOrEmpty(this.PostsArgs.EntityDescription) ? this.PostsArgs.EntityDescription : null;
                this.RegardingEntity = !AppTool.IsNullOrEmpty(this.PostsArgs.RegardingEntity) ? this.PostsArgs.RegardingEntity : null;
            }
        }
    }

    Placeholder: string = "Type message here...";
    private messageBody = "";
    get MessageBody() {

        return this.messageBody;
    }
    set MessageBody(newValue: string) {

        if (this.messageBody != newValue) {
            this.messageBody = newValue;

        }
    }


    private isWaitingForResponse = true;
    get IsWaitingForResponse() {

        return this.isWaitingForResponse;
    }
    set IsWaitingForResponse(newValue: boolean) {

        if (this.isWaitingForResponse != newValue) {
            this.isWaitingForResponse = newValue;

        }
    }



    GetNewConversationHeaderParticipantPM(userId: string) {
        var newConversationHeaderParticipantPM: ConversationHeaderParticipantPM = new ConversationHeaderParticipantPM();
        newConversationHeaderParticipantPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newConversationHeaderParticipantPM.ConversationHeaderId = this.NewConversationHeaderPM.Id;
        newConversationHeaderParticipantPM.Tenant = SessionLocator.Tenant;
        newConversationHeaderParticipantPM.IsLeft = false;
        newConversationHeaderParticipantPM.IsRead = false;
        newConversationHeaderParticipantPM.Replied = false;
        newConversationHeaderParticipantPM.IsDelete = false;
        newConversationHeaderParticipantPM.ParticipantUserId = userId;
        newConversationHeaderParticipantPM.LastReadDate = DateTool.GetCurrentDateTimeAsUtc();
        return newConversationHeaderParticipantPM;
    }



    StartSaving() {

        this.IsSaveConversationHeaderParticipantComplete = false;
        this.IsSaveConversationHeaderMessagePMComplete = false;
       

        this.CurrentSession.StartBusyIndicator("Sending...");
        this.NewConversationHeaderPM = new ConversationHeaderPM();
        this.NewConversationHeaderPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        this.NewConversationHeaderPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.NewConversationHeaderPM.Tenant = SessionLocator.Tenant;
        this.NewConversationHeaderPM.IsWaitingForResponse = this.IsWaitingForResponse;
        this.NewConversationHeaderPM.EntityId = this.EntityId;
        this.NewConversationHeaderPM.ObjectTableId = this.ObjectTableId;
        this.NewConversationHeaderPM.EntityDescription = this.EntityDescription;

        this.conversationHeaderPMService.insert(this.NewConversationHeaderPM).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {

                this.SaveConversationHeaderParticipantPM();

                var conversationHeaderMessagePM: ConversationHeaderMessagePM = new ConversationHeaderMessagePM();
                conversationHeaderMessagePM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
                conversationHeaderMessagePM.CreatedByUserId = SessionLocator.LoggedUserId;
                conversationHeaderMessagePM.Tenant = SessionLocator.Tenant;
                conversationHeaderMessagePM.ConversationHeaderId = this.NewConversationHeaderPM.Id;
                conversationHeaderMessagePM.MessageBody = this.MessageBody;
                conversationHeaderMessagePM.UserName = SessionLocator.LoggedUserPM.EnglishName;
                conversationHeaderMessagePM.RegardingEntity = this.RegardingEntity;
                this.conversationHeaderMessagePMService.insert(conversationHeaderMessagePM).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                  

                    this.IsSaveConversationHeaderMessagePMComplete = true;
            

                    if (pmResponse.HasError && pmResponse.ErrorsArray) {
                        pmResponse.ErrorsArray.forEach((error) => {
                            this.ValidationErrorsList.push(error);
                        });
                    }

                    this.StopBusyIndicator(!pmResponse.HasError);


                });


            }
            else {

                if (pmResponse.HasError && pmResponse.ErrorsArray) {
                    pmResponse.ErrorsArray.forEach((error) => {
                        this.ValidationErrorsList.push(error);
                    });
                }
                this.CurrentSession.StopBusyIndicator();
            }

        });


    }

    SaveConversationHeaderParticipantPM() {

        this.UserList = this.UserIds.split(';');
        this.ConversationHeaderParticipantPMLists = [];
        this.UserList.forEach((userid) => {

            this.ConversationHeaderParticipantPMLists.push(this.GetNewConversationHeaderParticipantPM(userid));
        });

        var userParticipant: string = this.UserList.filter(d => d == SessionLocator.LoggedUserId)[0];
        if (!userParticipant) {
            this.ConversationHeaderParticipantPMLists.push(this.GetNewConversationHeaderParticipantPM(SessionLocator.LoggedUserId));
       
        }


        this.conversationHeaderParticipantExtendedPMService.SaveConversationHeaderParticipantPMLists(this.ConversationHeaderParticipantPMLists).subscribe(res => {
            var pmResponse: ServiceResponse = res;

            this.IsSaveConversationHeaderParticipantComplete = true;

            if (pmResponse.HasError && pmResponse.ErrorsArray) {
                pmResponse.ErrorsArray.forEach((error) => {
                    this.ValidationErrorsList.push(error);
                });
            }

            this.StopBusyIndicator(!pmResponse.HasError);

        });




    }


   
    StopBusyIndicator(isCloseWindow: boolean) {
        if (this.IsSaveConversationHeaderParticipantComplete && this.IsSaveConversationHeaderMessagePMComplete) {
            this.CurrentSession.StopBusyIndicator();
            if (isCloseWindow) {
                if (this.AreaMessage == "Inbox") this.CurrentSession.FireEvent("SocialInboxMessagesRefresh");
                else if (this.AreaMessage == "Message") {
                    this.CurrentSession.FireEvent("SocialMessagesRefresh");
                }
                this.CurrentSession.CloseCurrentWindow();
            }
        }
    }



    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }

    MessageInputFocus() {
        this.Placeholder = "";
    }

    SaveButtonClick() {
        this.ValidationErrorsList = [];
        if (!AppTool.IsNullOrEmpty(this.MessageBody) && this.MessageBody!= "Type message here...") {
            if (this.MessageBody.length <= 999) {

                if (AppTool.IsNullOrEmpty(this.UserIds)) {
                    this.ValidationErrorsList.push("Please select at least one recipient");
                } else this.StartSaving();
    
            }
            else this.ValidationErrorsList.push("Message Body field Must be less than 1000 characters");
        }
        else this.ValidationErrorsList.push("Message Body field is required");


    }



 




}
