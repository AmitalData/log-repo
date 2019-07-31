import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../Infrastructure/Tools';
import {PostPMService} from '../Services/StandardPMs/PostPMService';
import {PostPM} from '../EntityPMs/PostPM';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';

import {PostViewModelData} from '../Components/SocialPostsComponent';


@Component({
    moduleId: module.id,
    selector: 'EditPostComponent',
    templateUrl: './EditPostComponent.html',


})

export class EditPostComponent implements OnInit {
    PostViewModelData: PostViewModelData;
    postPMService: PostPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.postPMService = new PostPMService();
    }

    ngOnInit(

    ) {



    }

    SetWindowArgs(args: any) {
        this.PostViewModelData = args.PostViewModelData;
        if (this.PostViewModelData != null) {
            this.BodyText = this.PostViewModelData.EntityPM.BodyText;
        }
    }

    private bodyText = "";
    get BodyText() {
        
        return this.bodyText;
    }
    set BodyText(newValue: string) {
     
        if (this.bodyText != newValue) {
            this.bodyText = newValue;
           
        }
    }

    CloseButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();
    }



    SaveButtonClick() {
        if (!AppTool.IsNullOrEmpty(this.BodyText)) {
            if (this.BodyText.length <= 4000) {
                if (this.BodyText != this.PostViewModelData.EntityPM.BodyText) {
                    this.PostViewModelData.EntityPM.BodyText = this.BodyText;
                    this.PostViewModelData.BodyText = this.PostViewModelData.ViewMode.ConvertBodyText(this.BodyText);
                    
                    this.CurrentSession.StartBusyIndicatorSaving();
                    this.postPMService.update(this.PostViewModelData.EntityPM).subscribe(res => {
                        var pmResponse: ServiceResponse = res;
                        this.CurrentSession.StopBusyIndicator();
                        if (!pmResponse.HasError && pmResponse.Result) {

                            this.CurrentSession.CloseCurrentWindow();
                        }


                    });
                }
                else this.CurrentSession.CloseCurrentWindow();
            }
            else {
                var messageWindow: MessageWindow = new MessageWindow();
                messageWindow.Show("Post maximum charachters should be less than 4000!");
            }
        }
        else {

            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("Post Body field is required");
        }
    
       // this.CurrentSession.CloseCurrentWindow();
    }





}
