
import {ContactPM} from '../../EntityPMs/ContactPM'
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow'
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {PasswordChangeService} from '../../Services/Others/PasswordChangeService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {UserExtendedPMService} from '../../Services/ExtendedPMs/UserExtendedPMService';

export class ContactMenuButtonsHandler {
    public EntityPM: ContactPM;
    public entityArgs: EntityArgs
    private CurrentSession = SessionLocator.SelectedSession;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                menuButtons.forEach(menuButton => {
                    switch (menuButton.EventCode) {
                        case "Anonymize": {
                           

                            break;
                        }
                    }
                });
            }
        }

        return menuButtons;
    }


    private isAnonymizeContact: boolean;
    private StopFlags() {
        this.isAnonymizeContact = false;
        this.isButtonClicked = false;
    }


    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    if (this.isAnonymizeContact) {
                        this.ContactAnonymize();
                    }
                }

                this.StopFlags();

            });
        }
    }


    isButtonClicked: boolean = false;
    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.isButtonClicked = true;
            switch (menuButton.EventCode) {
                case "Anonymize":
                    {
                        this.isAnonymizeContact = true;
                        this.entityArgs.EditComponent.SaveChanges();
                        break;
                    }
                default: {
                    this.isButtonClicked = false;
                    break;
                }
            }
        }
    }

    
    ContactAnonymize() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator.Translate("Contact.M.Anonymize"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {

                var service: UserExtendedPMService = new UserExtendedPMService();
                service.Anonymization(this.EntityPM.Id).subscribe(res => {
                    var pmResponse: ServiceResponse = res;
                    if (!pmResponse.HasError) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }

      
                });


            }
        });
    }



}
