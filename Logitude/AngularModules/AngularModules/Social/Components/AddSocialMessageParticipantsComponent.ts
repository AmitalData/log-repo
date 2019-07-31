import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool} from '../../Infrastructure/Tools';

import {PostPM} from '../EntityPMs/PostPM';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {SocialMessageParticipantsComponent} from '../Components/SocialMessageParticipantsComponent';
import {ConversationHeaderParticipantPM} from '../EntityPMs/ConversationHeaderParticipantPM';



import {ConversationHeaderParticipantExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService';


@Component({
    moduleId: module.id,
    selector: 'AddSocialMessageParticipantsComponent',
    templateUrl: './AddSocialMessageParticipantsComponent.html',


})

export class AddSocialMessageParticipantsComponent implements OnInit {

    SocialMessageParticipantsComponent: SocialMessageParticipantsComponent;
    conversationHeaderParticipantExtendedPMService: ConversationHeaderParticipantExtendedPMService;
    ConversationHeaderParticipantPMLists: ConversationHeaderParticipantPM[] = [];
    ConversationHeaderId: string = "";
    UserIds: string = "";
    ValidationErrorsList: string[] = [];
    ExcludedResult: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService();
    }

    ngOnInit(

    ) {



    }


    SetWindowArgs(args: any) {
        if (args) {
            this.SocialMessageParticipantsComponent = args.SocialMessageParticipantsComponent;
            this.ConversationHeaderId = args.ConversationHeaderId;
            this.ExcludedResult = args.SocialMessageParticipantsComponent.ExcludedResult;
            
        }
        

    }

   


    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }
    ParticipantUserLists: any;
    UserList: string[] = [];
    SaveButtonClick() {
        this.ValidationErrorsList = [];
        if (!AppTool.IsNullOrEmpty(this.UserIds)) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            this.UserList = this.UserIds.split(';');
            var isNoName: boolean = false;
            this.ConversationHeaderParticipantPMLists = [];
            this.UserList.forEach((userid) => {
                var participantUser: any = this.ParticipantUserLists.filter(d => d.Id == userid)[0];
                var name: string = "";
             
                if (participantUser) name = participantUser.EnglishName;
                else isNoName = true;
                this.ConversationHeaderParticipantPMLists.push(this.GetNewConversationHeaderParticipantPM(userid, name));
            });

            this.conversationHeaderParticipantExtendedPMService.SaveConversationHeaderParticipantPMLists(this.ConversationHeaderParticipantPMLists).subscribe(res => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                if (!pmResponse.HasError && pmResponse.Result) {
                    if (!isNoName) {
                        this.SocialMessageParticipantsComponent.BluidLists(pmResponse.Result);
                    } else this.SocialMessageParticipantsComponent.LoadData();
                  
                    //this.SocialMessageParticipantsComponent.IsChange = true;
                }
                this.CloseButtonClicked();
            });
        }
        else {
            this.ValidationErrorsList.push("Please select participants");

        }

    }


    GetNewConversationHeaderParticipantPM(userId: string , name:string) {
        var newConversationHeaderParticipantPM: ConversationHeaderParticipantPM = new ConversationHeaderParticipantPM();
        newConversationHeaderParticipantPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
        newConversationHeaderParticipantPM.ConversationHeaderId = this.ConversationHeaderId;
        newConversationHeaderParticipantPM.Tenant = SessionLocator.Tenant;
        newConversationHeaderParticipantPM.IsLeft = false;
        newConversationHeaderParticipantPM.IsRead = false;
        newConversationHeaderParticipantPM.Replied = false;
        newConversationHeaderParticipantPM.IsDelete = false;
        newConversationHeaderParticipantPM.ParticipantUserId = userId;
        newConversationHeaderParticipantPM.ParticipantName = name;
        newConversationHeaderParticipantPM.LastReadDate = DateTool.GetCurrentDateTimeAsUtc();
        return newConversationHeaderParticipantPM;
    }






}
