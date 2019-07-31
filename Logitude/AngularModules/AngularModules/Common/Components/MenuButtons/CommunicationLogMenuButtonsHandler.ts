import {CommunicationLogPM} from '../../EntityPMs/CommunicationLogPM'
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow'
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {CommunicationLogExtendedPMService} from '../../../Common/Services/ExtendedPMs/CommunicationLogExtendedPMService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {PasswordChangeService} from '../../Services/Others/PasswordChangeService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';

export class CommunicationLogMenuButtonsHandler {
    public EntityPM: CommunicationLogPM;
    public entityArgs: EntityArgs
    communicationLogExtendedPMService: CommunicationLogExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.communicationLogExtendedPMService = new CommunicationLogExtendedPMService();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    switch (menuButton.EventCode) {
                        case "Resend": {
                            if (this.EntityPM.InOut == "I") {
                                menuButton.IsDisabled = true;
                            }
                            else if (this.EntityPM.CommunicationStatusTypeCode == "D" && this.EntityPM.To == "QBO")
                                menuButton.IsDisabled = true;
                            else if (this.EntityPM.To == "Profact")
                                menuButton.IsDisabled = true;
                            break;
                        }
                        case "ViewMessage": {
                            menuButton.IsHidden = true;
                            break;
                        }

                        case "Actions": {
                            menuButton.IsHidden = true;
                            break;
                        }
                    }
                });
            }
        }

        return menuButtons;
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                switch (menuButton.EventCode) {
                    case "Resend": {
                        this.ResendButtonClcik();
                        break;
                    }

                    case "ViewMessage": {
                        if (this.EntityPM.CommunicationLogTypeCode == "E") {
                            this.ViewMessage();
                        }
                        break;
                    }


                }
            }
        }
    }

    ResendButtonClcik() {

        this.CurrentSession.StartBusyIndicator("Resending...");
        this.communicationLogExtendedPMService.SendCommunicationLogToQueue(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(res => {
            this.CurrentSession.StopBusyIndicator();
        });
    }




    ViewMessage() {

    }







}
