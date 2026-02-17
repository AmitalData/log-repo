import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool} from '../../Infrastructure/Tools';

import {PostPM} from '../EntityPMs/PostPM';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {ConversationHeaderParticipantPM} from '../EntityPMs/ConversationHeaderParticipantPM';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ConversationHeaderViewModelData} from '../Components/SocialMessagesComponent'

import {ConversationHeaderParticipantExtendedPMService} from '../Services/ExtendedPMs/ConversationHeaderParticipantExtendedPMService';


@Component({
    moduleId: module.id,
    selector: 'SocialMessageParticipantsComponent',
    templateUrl: './SocialMessageParticipantsComponent.html',


})

export class SocialMessageParticipantsComponent implements OnInit {

    
    conversationHeaderParticipantExtendedPMService: ConversationHeaderParticipantExtendedPMService;
    ConversationHeaderParticipantPMLists: ConversationHeaderParticipantPM[] = [];
    ConversationHeaderParticipantPMSelected: ConversationHeaderParticipantPM;
    ConversationHeaderId: string = "";
    ExcludedResult: string[];
    Area: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.conversationHeaderParticipantExtendedPMService = new ConversationHeaderParticipantExtendedPMService();
    }

    ngOnInit(

    ) {



    }

    ConversationHeader: ConversationHeaderViewModelData;
    SetWindowArgs(args: any) {
        if (args) {
            this.ConversationHeader = args.ConversationHeader;

            if (this.ConversationHeader) {
                this.ConversationHeaderId = this.ConversationHeader.ConversationHeaderId;
                this.Area = this.ConversationHeader.Area;
            }
       
            
        }
        if (!AppTool.IsNullOrEmpty(this.ConversationHeaderId)) {
            this.LoadData();
        }
   
    }

    AddConversationHeaderParticipantPMButtonClicked() {
        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        windowArgs.SocialMessageParticipantsComponent = this;
        windowArgs.ConversationHeaderId = this.ConversationHeaderId;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 510;
        logWindow.Height = 200;
        logWindow.Title = "New Participants"
        logWindow.Show("./Social/Components/AddSocialMessageParticipantsComponent");
    }

    
    LoadData() {

        this.ExcludedResult = [];
        this.ConversationHeaderParticipantPMLists = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.conversationHeaderParticipantExtendedPMService.GetAllConversationHeaderParticipantPMByConversationHeaderId(this.ConversationHeaderId).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                this.BluidLists(pmResponse.Result);

       
            }

        });

    }
    MessageOthers: string = "";

    BluidLists(conversationHeaderParticipantPMLists: ConversationHeaderParticipantPM[], addParticipant: boolean = false) {

        conversationHeaderParticipantPMLists.forEach((item) => {
            this.ConversationHeaderParticipantPMLists.push(item);
            this.ExcludedResult.push(item.ParticipantUserId);
        });

        this.ConversationHeader.MessageOthers = "+ " + (this.ConversationHeaderParticipantPMLists.length - 3).toString() + " more";
        if (addParticipant) this.ConversationHeader.IsChange = true;
 
    }




    SortItemSource() {
    
        this.ConversationHeaderParticipantPMLists = this.ConversationHeaderParticipantPMLists.sort();
        this.ConversationHeaderParticipantPMLists.sort((a, b) => {
            if (a.ParticipantName.toLowerCase() < b.ParticipantName.toLowerCase()) {
                return -1;
            }
            else if (a.ParticipantName.toLowerCase() > b.ParticipantName.toLowerCase()) {
                return 1;
            }
            else {

                return 0;
            }
        });
    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }


    SaveButtonClicked() {
        this.CloseButtonClicked();
    }








}
