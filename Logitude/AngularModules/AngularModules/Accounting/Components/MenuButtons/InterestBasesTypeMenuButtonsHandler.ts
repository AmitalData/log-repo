declare var window: any;
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { InterestBasesTypeListService } from '../../Services/StandardLists/InterestBasesTypeListService';
import { InterestBasesTypeList } from '../../EntityLists/InterestBasesTypeList';
import { InterestBasesTypePM } from '../../EntityPMs/InterestBasesTypePM';

export class InterestBasesTypeMenuButtonsHandler {
    public EntityPM: InterestBasesTypePM;
    public entityArgs: EntityArgs
    public ObjectTableName: string = "InterestBasesType"
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }


        }
    }



    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {
                        case "InterestBasesTypeInactive":
                            {
                                //if (this.EntityPM.InActive == true) {
                                //    button.IsHidden = false;
                                //} 
                                //else {
                                //    button.IsDisabled = false;
                                //    button.IsHidden = false;
                                //}
                                break;
                            }
                    }
                }
            }
        }

        return menuButtons;
    }

    public MenuButtonClick(menuButton: MenuButtonPM) {

        var errors = [];

        if (errors.length == 0) {
            switch (menuButton.EventCode) {
                case "InterestBasesTypeInactive":
                    {
                        this.UpdateInactiveField(true);
                        break;
                    }
            }
        } else {
            this.entityArgs.EditComponent.ValidationErrorsList = [];
            this.entityArgs.EditComponent.ValidationErrorsList = errors;
        }
    }

    private UpdateInactiveField(inactive:boolean) {
       
        var myInterestBasesTypeListService: InterestBasesTypeListService = new InterestBasesTypeListService();
        myInterestBasesTypeListService.getSingle(this.EntityPM.Id)
            .subscribe((response: ServiceResponse) => {
                var interestBasesType: InterestBasesTypeList = response.Result as InterestBasesTypeList;
                this.EntityPM.InActive = inactive;
                    this.SaveChenges();            
            });
    }

    SaveChenges() {
        this.entityArgs.EditComponent.SaveChanges();
        this.entityArgs.EditComponent.SaveCompleted.subscribe(($event) => {
            if ($event == true) {
                this.entityArgs.EditComponent.ReloadEntityPM();
            }
        });
    }


}
