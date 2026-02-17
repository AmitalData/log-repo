
import {UserPM} from '../../EntityPMs/UserPM'
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow'
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {PasswordChangeService} from '../../Services/Others/PasswordChangeService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {UserExtendedPMService} from '../../Services/ExtendedPMs/UserExtendedPMService';

export class UserMenuButtonsHandler {
    public EntityPM: UserPM;
    public entityArgs: EntityArgs
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
    private CurrentSession = SessionLocator.SelectedSession;

    private isAnonymizeUser: boolean;
    private StopFlags() {
        this.isAnonymizeUser = false;
        this.isButtonClicked = false;
    }
 


    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    if (this.isAnonymizeUser) {
                      
                        this.UserAnonymize();
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
                case "ResetUserPassword":
                    {
                        this.ResetUserPassword();
                        this.isButtonClicked = false;
                        break;
                    }
                case "Anonymize":
                    {

                        this.isAnonymizeUser = true;
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



    private ResetUserPassword() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Title = TextCodeTranslator.Translate("User.O.ResetUserPassword");
        confirmWindow.Show(TextCodeTranslator.Translate("User.M.YouWantToResetPassword"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
               this.ResetPassword();
            }

        });
      
    }



    ResetPassword() {
        var myService: PasswordChangeService = new PasswordChangeService();
        myService.ResetUserPassword(this.EntityPM.Id, this.EntityPM.Tenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {

                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show(TextCodeTranslator.Translate("User.M.UserPasswordResetCompletedSuccessfully") + ": " + result + ".");

                }

            }

        });
    }


    UserAnonymize() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(TextCodeTranslator.Translate("User.M.Anonymize"));
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
